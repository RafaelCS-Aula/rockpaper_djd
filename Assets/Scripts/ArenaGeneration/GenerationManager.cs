using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
   
    [Header("----- Content Settings ------")]
    [SerializeField] private List<ArenaPiece> piecesForGeneration;

    [Header("-- Starting Piece Settings --")]
    [SerializeField] private bool _setStartingPiece = false;
    [SerializeField] private List<ArenaPiece> _possibleStartingPieces;
    [SerializeField] private int _connectorCountTolerance = 0;

    [Header("------ Horizontal Level Generation Settings --------")]

    [SerializeField] private bool _corridorGeneration = false;
    [SerializeField] private int _maxLevelPieceCount;
    [SerializeField] private int _groupTolerance = 0;
    [SerializeField] private bool _clippingCorrection = false;
    [SerializeField] private float _pieceDistance = 0.0001f;

    [Header("------ Vertical Level Settings --------")]
    [SerializeField] private bool _createUpperLevel;

    [Tooltip("Choose random pieces and put other pieces above them.")]
    [SerializeField] private bool _upperLevelIslandGeneration;
    [SerializeField] private int _upperIslandsCount = 1;

    [Header("--")]
    [SerializeField] private bool _createLowerLevel;

    [Tooltip("Choose random pieces and put other pieces under them.")]
    [SerializeField] private bool _lowerLevelIslandGeneration;
    [SerializeField] private int _lowerIslandsCount = 1;


    private List<ArenaPiece> _placedPieces;


    private List<List<ArenaPiece>> _sortedPieces;


    /// Already placed piece being used to judge others
    private ArenaPiece _selectedPiece;

    /// Piece being evaluated against selectedPiece
    private ArenaPiece _evaluatingPiece;


    private int largestGroup;
    
    // Start is called before the first frame update
    void Awake()
    {
        
       Create();
        

    }

    private void Create()
    {
        _sortedPieces = new List<List<ArenaPiece>>();

        foreach (ArenaPiece a in piecesForGeneration)
            a.Setup(_clippingCorrection);

        piecesForGeneration.Sort();
        largestGroup = 
            piecesForGeneration[0].ConnectorsCount;


        
        // Seperate pieces into seperate lists based on largest group
        _sortedPieces = SplitList();

        // Place the first piece
        PickFirstPiece();

        // Make base level of Arena and add those pieces to the list
        _placedPieces.AddRange(MakeHorizontalArena(_placedPieces[0]));

        if(_createUpperLevel || _createLowerLevel)
            MakeVerticalArena(_placedPieces);


        foreach (ArenaPiece item in _placedPieces)
        {
            print(item.gameObject);
        }
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
            sizeArray[i] = _sortedPieces[i][0].ConnectorsCount;

         // Check what list of the sorted list the selected belongs to
        int myPieceList = 0;

        for (int i = -1; i < arena.Count; i++)
        {
            if(i == -1)
                _selectedPiece = startingPiece;
            else
                _selectedPiece = arena[i];


            // Reset wasAnalyzed in all the pieces that are yet to be evaluated.
            foreach(ArenaPiece a in _sortedPieces[myPieceList])
                a.wasAnalysed = false;

            // Pick a piece to evaluate against our selected placed one
            selectPiece:

            int rng;
            int wideRng = Random.Range(0,_sortedPieces.Count);
            myPieceList = wideRng;
            if(_sortedPieces[myPieceList].Count != 0)
                rng = Random.Range(0,_sortedPieces[myPieceList].Count);
            else
            {
                 myPieceList++;
                goto selectPiece;
            
            }

            _evaluatingPiece = _sortedPieces[myPieceList][rng];
            

            // Compare all the connectors on both pieces and get a transform to
            // where to place the evaluated piece

            GameObject spawnedPiece = Instantiate(_evaluatingPiece).gameObject;
            ArenaPiece spawnedScript = spawnedPiece.GetComponent<ArenaPiece>();

            (bool valid, Transform trn) evaluationResult =
                 _selectedPiece.EvaluatePiece(spawnedScript, 
                 _pieceDistance, 
                 _groupTolerance);

            // If things worked out, spawn the piece in the correct position
            if(evaluationResult.valid)
            {

                arena.Add(spawnedScript);
                
                if(arena.Count >= _maxLevelPieceCount)
                    return arena;

            }
            else
            {
                // No valid connectors in the given piece
                Destroy(spawnedPiece);
                _evaluatingPiece.wasAnalysed = true;

            } 

            // if this one has no more free connectors, move on to the next 
            // placed piece
            if(_corridorGeneration)
            {
                

                continue;
            }


            if(_selectedPiece.isFull())
                continue;
            else // else choose another piece to evaluate for this one
                goto selectPiece;
        }

        return arena;
    }

    /// <summary>
    /// Create the vertical upper and lower levels of the arena
    /// </summary>
    /// <param name="arena">The placed pieces that will be selected</param>
    private void MakeVerticalArena(ICollection<ArenaPiece> arena)
    {
        
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
                placedHaveBotConns.Add(a);
        }
        
        // Upper Levels
        if(_upperLevelIslandGeneration && placedHaveTopConns.Count > 0)
        {

            for(int i = 0; i < _upperIslandsCount; i++)
            {
                int rng = Random.Range(0,placedHaveTopConns.Count);
                _selectedPiece = placedHaveTopConns[rng];

                pickEvaluatingPiece:

                int sortRng = Random.Range(0,_sortedPieces.Count);
                int listRng = Random.Range(0,_sortedPieces[sortRng].Count);

                _evaluatingPiece = _sortedPieces[sortRng][listRng];

                GameObject spawnedPiece =
                    Instantiate(_evaluatingPiece).gameObject;
                ArenaPiece spawnedScript =
                    spawnedPiece.GetComponent<ArenaPiece>();

                if(_evaluatingPiece != null)
                    evaluationResult =_selectedPiece.EvaluatePieceVertical(
                        spawnedScript, true);
                else
                    goto pickEvaluatingPiece;

                if(evaluationResult.valid)
                {
                    _placedPieces.Add(spawnedScript);

                }
                else
                    Destroy(spawnedPiece);
                    

            }

        }
        else if(!_upperLevelIslandGeneration && placedHaveTopConns.Count > 0)
        {

            _placedPieces.AddRange(MakeHorizontalArena
            (placedHaveBotConns[Random.Range(0, placedHaveTopConns.Count)]));

        }

        // Lower Levels
        if(_lowerLevelIslandGeneration && placedHaveBotConns.Count > 0)
        {

            for(int i = 0; i < _lowerIslandsCount; i++)
            {
                int rng = Random.Range(0,placedHaveBotConns.Count);
                _selectedPiece = placedHaveBotConns[rng];

                pickEvaluatingPiece:

                int sortRng = Random.Range(0,_sortedPieces.Count);
                int listRng = Random.Range(0,_sortedPieces[sortRng].Count);

                _evaluatingPiece = _sortedPieces[sortRng][listRng];

                GameObject spawnedPiece =
                                   Instantiate(_evaluatingPiece).gameObject;
                ArenaPiece spawnedScript =
                    spawnedPiece.GetComponent<ArenaPiece>();

                if (_evaluatingPiece != null)
                    evaluationResult = _selectedPiece.EvaluatePieceVertical(
                        spawnedScript, false);
                else
                    goto pickEvaluatingPiece;

                if (evaluationResult.valid)
                {
                    _placedPieces.Add(spawnedScript);


                }
                else
                    Destroy(spawnedPiece);


            }


        }
        else if(!_lowerLevelIslandGeneration && placedHaveBotConns.Count > 0)
        {

        
            _placedPieces.AddRange(MakeHorizontalArena
            (placedHaveTopConns[Random.Range(0, placedHaveBotConns.Count)]));

        }
            
    }

    /// <summary>
    /// Select and place the first piece of the arena
    /// </summary>
    private void PickFirstPiece()
    {
        // To be safe if the starters list fails
        chooseStarter:

        ArenaPiece choosen = null;
        
        
        int choosenIndex = 0;
        _placedPieces = new List<ArenaPiece>();

        if (_setStartingPiece)
        {
            if(_possibleStartingPieces.Count == 0)
            {
                Debug.LogError(
                    "'Set starting piece' is on but no ArenaPieces were given");
                _setStartingPiece = false;
                goto chooseStarter;
            }

            choosenIndex = Random.Range(0, _possibleStartingPieces.Count);
            choosen =     _possibleStartingPieces[choosenIndex];    
    
        }
        else
        {
            
            if(_corridorGeneration)
            {
                choosenIndex = Random.Range(0, _sortedPieces[0].Count);
                choosen = _sortedPieces[0][choosenIndex];

            }
                
            else
            {
                choosenIndex = Random.Range(
                    0, _sortedPieces[_sortedPieces.Count - 1].Count);
                    
                choosen = _sortedPieces[_sortedPieces.Count - 1][choosenIndex];
            }
                
        }

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
            if (piecesForGeneration[i].ConnectorsCount < lastConsidered)
            {
                considererdList = new List<ArenaPiece>();
                considererdList.Add(piecesForGeneration[i]);
                lastConsidered = piecesForGeneration[i].ConnectorsCount;
                sortedList.Add(considererdList);

            }
            // piece belongs in the already made list
            else if (piecesForGeneration[i].ConnectorsCount >= 
            lastConsidered - _connectorCountTolerance)
            {

                considererdList.Add(piecesForGeneration[i]);

            }

        }

        return sortedList;
    }


}

