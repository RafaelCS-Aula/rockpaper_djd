using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
   

    [SerializeField] private bool _setCentralPiece = false;
    [SerializeField] private ArenaPiece _centralPiece;

    [SerializeField] private bool allowDuplicates = false;
    [SerializeField] private int maxPieces;



    private List<ArenaPiece> _placedPieces;

    [SerializeField] private List<ArenaPiece> piecesForGeneration;
    private List<List<ArenaPiece>> _sortedPieces;


    /// Already placed piece being used to judge others
    private ArenaPiece _selectedPiece;

    /// Piece being evaluated against selectedPiece
    private ArenaPiece _evaluatingPiece;


    private int largestGroup;
    private int smallestLargestGroup;
    
    // Start is called before the first frame update
    void Start()
    {

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

        MakeArena();


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
    private void MakeArena()
    {
        // Make an array listing all the sizes of the biggest groups in the 
        // sorted pieces groups
        int[] sizeArray;
        sizeArray = new int[_sortedPieces.Count];

        for(int i = 0; i < sizeArray.Length; i++)
            sizeArray[i] = _sortedPieces[i][0].largestGroupCount;

        /////////////////
         
        int myPieceList = 0;

        for (int i = 0; i < _placedPieces.Count; i++)
        {
            _selectedPiece = _placedPieces[i];
            
            // Check what list of the sorted list the selected belongs to
            for(int x = 0; x < sizeArray.Length; x++)
                if(_selectedPiece.largestGroupCount == sizeArray[x])
                    myPieceList = sizeArray[x];

            //////////////////

            selectPiece:

            int rng = Random.Range(0,_sortedPieces[myPieceList].Count);
            _evaluatingPiece = _sortedPieces[myPieceList][rng];
            
            (bool valid, Transform trn) evaluationResult =
                 _selectedPiece.EvaluatePiece(_evaluatingPiece);

            if(evaluationResult.valid)
            {
                Instantiate(_evaluatingPiece,
                evaluationResult.trn.position, evaluationResult.trn.rotation);
                _placedPieces.Add(_evaluatingPiece);
                
                if(!allowDuplicates)
                    _sortedPieces[myPieceList].RemoveAt(rng);
            }
            if(_selectedPiece.isFull())
                continue;
            else
                goto selectPiece;
        }
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

        if(!allowDuplicates && _setCentralPiece)
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

