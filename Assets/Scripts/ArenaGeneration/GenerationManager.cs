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

    [SerializeField] private bool useAllPiecesOfList = true;
    [SerializeField] private bool _allowDuplicates = false;
    [SerializeField] private int _maxSidePieces;
    [SerializeField] private int _groupTolerance = 0;
    
    [SerializeField] private float _pieceDistance = 0.0001f;

    [Header("------ Vertical Level Settings --------")]
    [SerializeField] private int _upperLevels = 0;
    [SerializeField] private bool _upperIslandGeneration;
    [SerializeField] private int _upperIslandsMaxPieces = 1;

    [Header("--")]
    [SerializeField] private int _lowerLevels = 0;

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


        foreach (ArenaPiece a in piecesForGeneration)
            a.Setup();

        piecesForGeneration.Sort();
        largestGroup = 
            piecesForGeneration[0].largestGroupCount;
        smallestLargestGroup = 
            piecesForGeneration[piecesForGeneration.Count - 1].largestGroupCount;

        
        // Seperate pieces into seperate lists based on largest group
        SplitList();

        // Place the first piece
        PickFirstPiece();

        // Make base level of Arena
        MakeBaseArena();

        if(_upperLevels > 0 || _lowerLevels > 0)
            MakeVerticalArena();

        /* List<int> i = new List<int>();
        i.Add(2);
        i.Add(1);
        i.Add(9);
        i.Add(5);
        i.Add(4);

        i.Sort();
        for(int e = 0; e < i.Count; e++)
            print(i[e]);*/




    }

    /// <summary>
    /// Selects the pieces that are evaluated and then spawned
    /// </summary>
    private void MakeBaseArena()
    {
        // Make an array listing all the sizes of the biggest groups in the 
        // sorted pieces groups
        int[] sizeArray;
        sizeArray = new int[_sortedPieces.Count];

        for(int i = 0; i < sizeArray.Length; i++)
            sizeArray[i] = _sortedPieces[i][0].largestGroupCount;

         // Check what list of the sorted list the selected belongs to
        int myPieceList = 0;

        for (int i = 0; i < _placedPieces.Count; i++)
        {
            _selectedPiece = _placedPieces[i];
            
            
            for(int x = 0; x < sizeArray.Length; x++)
                if(_selectedPiece.largestGroupCount == sizeArray[x])
                    myPieceList = sizeArray[x];

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

                _placedPieces.Add(_evaluatingPiece);
                
                if(!_allowDuplicates)
                    _sortedPieces[myPieceList].RemoveAt(rng);

                // if we're at the piece limit break out of the method
                if(!useAllPiecesOfList && _placedPieces.Count >= _maxSidePieces)
                    return;

            }
            else // No valid connectors in the given piece
                _evaluatingPiece.wasAnalysed = true;

            // if this one has no more free connectors, move on to the next 
            // placed piece
            if(_selectedPiece.isFull())
                continue;
            else // else choose another piece to evaluate for this one
                goto selectPiece;
        }
    }

    private void MakeVerticalArena()
    {
        // Upper Pieces
        foreach(ArenaPiece a in _placedPieces)
            if(a.)



    }

    /// <summary>
    /// Select and place the first piece of the arena
    /// </summary>
    private void PickFirstPiece()
    {
        ArenaPiece choosen = null;
        int choosenIndex = Random.Range(0, _sortedPieces[0].Count - 1);

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
    private void SplitList()
    {
        int lastConsidered = largestGroup;
        List<ArenaPiece> considererdList = new List<ArenaPiece>();
        List<List<ArenaPiece>> sortedList = new List<List<ArenaPiece>>();

        _sortedPieces.Add(considererdList);
        for (int i = 0; i < piecesForGeneration.Count; i++)
        {
            if (piecesForGeneration[i].largestGroupCount < lastConsidered)
            {
                considererdList = new List<ArenaPiece>();
                considererdList.Add(piecesForGeneration[i]);
                sortedList.Add(considererdList);

            }
            else if (piecesForGeneration[i].largestGroupCount == lastConsidered)
            {

                considererdList.Add(piecesForGeneration[i]);

            }
        }

        _sortedPieces = sortedList;
    }


}

