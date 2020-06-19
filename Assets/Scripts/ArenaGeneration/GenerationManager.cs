using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
   
    [Header("----- Content Settings ------")]
    [SerializeField] private bool _setCentralPiece = false;
    [SerializeField] private ArenaPiece _centralPiece;
        
    [SerializeField] private List<ArenaPiece> piecesForGeneration;

    [Header("------ Generation Settings --------")]

    [SerializeField] private bool _corridorGeneration = false;
    [SerializeField] private bool useAllPiecesOfList = true;
    [SerializeField] private bool _allowDuplicates = false;
    [SerializeField] private int _maxSidePieces;
    [SerializeField] private int _groupTolerance = 0;
    
    [SerializeField] private float _pieceDistance = 0.0001f;

    [Header("------ Vertical Level Settings --------")]
    [SerializeField] private int _upperLevels = 0;

    [Tooltip("Choose random pieces and put other pieces above them.")]
    [SerializeField] private bool _upperIslandGeneration;
    [SerializeField] private int _upperIslandsMaxPieces = 1;

    [Header("--")]
    [SerializeField] private int _lowerLevels = 0;

    [Tooltip("Choose random pieces and put other pieces under them.")]
    [SerializeField] private bool _lowerIslandGeneration;
    [SerializeField] private int _lowerIslandsMaxPieces = 1;


    private List<ArenaPiece> _placedPieces;


    private List<List<ArenaPiece>> _sortedPieces;


    /// Already placed piece being used to judge others
    private ArenaPiece _selectedPiece;

    /// Piece being evaluated against selectedPiece
    private ArenaPiece _evaluatingPiece;


    private int largestGroup;
    private int smallestLargestGroup;
    
    // Start is called before the first frame update
    void Awake()
    {
        
        if(_allowDuplicates && useAllPiecesOfList)
        {
            Debug.LogError(" 'ALLOW DUPLICATES' AND 'USE ALL PIECES' CANNOT BE"
             +"ON AT THE SAME TIME IT IS AN INFINITE LOOP");
            Debug.Break();

        }
        _sortedPieces = new List<List<ArenaPiece>>();

        foreach (ArenaPiece a in piecesForGeneration)
            a.Setup();

        piecesForGeneration.Sort();
        largestGroup = 
            piecesForGeneration[0].largestGroupCount;
        smallestLargestGroup = 
            piecesForGeneration[piecesForGeneration.Count - 1].largestGroupCount;

        
        // Seperate pieces into seperate lists based on largest group
        _sortedPieces = SplitList();

        // Place the first piece
        PickFirstPiece();

        // Make base level of Arena and add those pieces to the list
        _placedPieces.AddRange(MakeHorizontalArena(_placedPieces[0]));

        if(_upperLevels > 0 || _lowerLevels > 0)
            MakeVerticalArena(_placedPieces);



        //TODO: Serialise the _placedPieces list to Json so it can be
        // loaded back in again. And we can laod and save premade arenas

    }

    /// <summary>
    /// Populates a horizontal level of the arena
    /// </summary>
    /// <param name="startingPiece"> The first piece of the arena</param>
    /// <returns> The pieces placed during this method's process</returns>
    private List<ArenaPiece> MakeHorizontalArena(ArenaPiece startingPiece)
    {
        // Pieces placed by this method call
        List<ArenaPiece> arena = new List<ArenaPiece>(); 

        // Make an array listing all the sizes of the biggest groups in the 
        // sorted pieces groups
        int[] sizeArray;
        sizeArray = new int[_sortedPieces.Count];

        for(int i = 0; i < sizeArray.Length; i++)
            sizeArray[i] = _sortedPieces[i][0].largestGroupCount;

         // Check what list of the sorted list the selected belongs to
        int myPieceList = 0;

        for (int i = -1; i < arena.Count; i++)
        {
            if(i == -1)
                _selectedPiece = startingPiece;
            else
                _selectedPiece = arena[i];


            for (int x = 0; x < sizeArray.Length; x++)
            {
                if (_selectedPiece.largestGroupCount == sizeArray[x])
                {
                    myPieceList = x;
                    break;

                }

            }

                    

            // Reset wasAnalyzed in all the pieces that are yet to be evaluated.
            foreach(ArenaPiece a in _sortedPieces[myPieceList])
                a.wasAnalysed = false;

            // Pick a piece to evaluate against our selected placed one
            selectPiece:

            int rng = Random.Range(0,_sortedPieces[myPieceList].Count);

            // INFINITE LOOP IF ALL WERE ANALYSED ALREADY
            if(!_sortedPieces[myPieceList][rng].wasAnalysed)
                _evaluatingPiece = _sortedPieces[myPieceList][rng];
            else
                goto selectPiece;

            // Compare all the connectors on both pieces and get a transform to
            // where to place the evaluated piece
            (bool valid, Transform trn) evaluationResult =
                 _selectedPiece.EvaluatePiece(_evaluatingPiece, 
                 _pieceDistance, 
                 _groupTolerance);

            // If things worked out, spawn the piece in the correct position
            if(evaluationResult.valid)
            {
                Instantiate(_evaluatingPiece,
                evaluationResult.trn.position, evaluationResult.trn.rotation);

                arena.Add(_evaluatingPiece);
                
                if(!_allowDuplicates)
                    _sortedPieces[myPieceList].RemoveAt(rng);

                // if we're at the piece limit break out of the method
                if(!useAllPiecesOfList && arena.Count >= _maxSidePieces)
                    return arena;

            }
            else // No valid connectors in the given piece
                _evaluatingPiece.wasAnalysed = true;

            // if this one has no more free connectors, move on to the next 
            // placed piece
            if(_selectedPiece.isFull() || _corridorGeneration)
                continue;
            else // else choose another piece to evaluate for this one
                goto selectPiece;
        }

        return arena;
    }

    private void MakeVerticalArena(ICollection<ArenaPiece> arena)
    {
        

        // Placed peices
        List<ArenaPiece> placedHaveTopConns = new List<ArenaPiece>();
        List<ArenaPiece> placedHaveBotConns = new List<ArenaPiece>();


        (bool t, bool b) verticals;
        (bool valid, Transform trn) evaluationResult;
        
        foreach(ArenaPiece a in arena)
        {
            verticals = a.GetVerticalConnectors();
            if(verticals.t)
                placedHaveTopConns.Add(a);
            if(verticals.b)
                placedHaveTopConns.Add(a);
        }
        
        // Upper Levels
        if(_upperIslandGeneration)
        {
            int limit = 
                (_upperIslandsMaxPieces > placedHaveTopConns.Count) ?
                 _upperIslandsMaxPieces : placedHaveTopConns.Count;

            for(int i = 0; i < limit; i++)
            {
                int rng = Random.Range(0,placedHaveTopConns.Count);
                _selectedPiece = placedHaveTopConns[rng];

                pickEvaluatingPiece:

                int sortRng = Random.Range(0,_sortedPieces.Count);
                int listRng = Random.Range(0,_sortedPieces[sortRng].Count);

                _evaluatingPiece = _sortedPieces[sortRng][listRng];
                if(_evaluatingPiece != null)
                    evaluationResult =_selectedPiece.EvaluatePieceVertical(
                        _evaluatingPiece, true);
                else
                    goto pickEvaluatingPiece;

                if(evaluationResult.valid)
                {
                    _placedPieces.Add(_evaluatingPiece);

                    Instantiate(_evaluatingPiece,
                    evaluationResult.trn.position,
                     evaluationResult.trn.rotation);

                }
                    

            }

        }
        else
        {

            _placedPieces.AddRange(MakeHorizontalArena
            (placedHaveTopConns[Random.Range(0, placedHaveTopConns.Count)]));

        }

        // Lower Levels
        if(_lowerIslandGeneration)
        {
            int limit = 
                (_lowerIslandsMaxPieces > placedHaveBotConns.Count) ?
                 _lowerIslandsMaxPieces : placedHaveBotConns.Count;

            for(int i = 0; i < limit; i++)
            {
                int rng = Random.Range(0,placedHaveBotConns.Count);
                _selectedPiece = placedHaveBotConns[rng];

                pickEvaluatingPiece:

                int sortRng = Random.Range(0,_sortedPieces.Count);
                int listRng = Random.Range(0,_sortedPieces[sortRng].Count);

                _evaluatingPiece = _sortedPieces[sortRng][listRng];
                if(_evaluatingPiece != null)
                    evaluationResult =_selectedPiece.EvaluatePieceVertical(
                        _evaluatingPiece, false);
                else
                    goto pickEvaluatingPiece;

                if(evaluationResult.valid)
                {

                    _placedPieces.Add(_evaluatingPiece);

                    Instantiate(_evaluatingPiece,
                    evaluationResult.trn.position,
                     evaluationResult.trn.rotation);

                    
                }
                    

            }


        }
        else
        {

            _placedPieces.AddRange(MakeHorizontalArena
            (placedHaveBotConns[Random.Range(0, placedHaveBotConns.Count)]));

        }
            
    }

    /// <summary>
    /// Select and place the first piece of the arena
    /// </summary>
    private void PickFirstPiece()
    {
        ArenaPiece choosen = null;
        int choosenIndex = Random.Range(0, _sortedPieces[0].Count);

        _placedPieces = new List<ArenaPiece>();
        if (_setCentralPiece)
        {
            choosen = _centralPiece;
            
        }
        else
        {
            choosen = _sortedPieces[0][choosenIndex];

        }

        if(!_allowDuplicates && _setCentralPiece)
            _sortedPieces[0].RemoveAt(0);

        _placedPieces.Add(choosen);
        Instantiate(_placedPieces[0].gameObject);
    }

    /// <summary>
    /// Seperate pieces into seperate lists based on largest group
    /// </summary>
    private List<List<ArenaPiece>> SplitList()
    {
        int lastConsidered = largestGroup + 1;
        List<ArenaPiece> considererdList = new List<ArenaPiece>();
        List<List<ArenaPiece>> sortedList = new List<List<ArenaPiece>>();

        
        for (int i = 0; i < piecesForGeneration.Count; i++)
        {
            // Piece belongs in a new list made for its size            
            if (piecesForGeneration[i].largestGroupCount < lastConsidered)
            {
                considererdList = new List<ArenaPiece>();
                considererdList.Add(piecesForGeneration[i]);
                lastConsidered = piecesForGeneration[i].largestGroupCount;
                sortedList.Add(considererdList);

            }
            // piece belongs in the already made list
            else if (piecesForGeneration[i].largestGroupCount == lastConsidered)
            {

                considererdList.Add(piecesForGeneration[i]);

            }

        }

        return sortedList;
    }


}

