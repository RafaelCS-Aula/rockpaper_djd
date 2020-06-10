using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
   

    [SerializeField] private bool _setCentralPiece = false;
    [SerializeField] private ArenaPiece _centralPiece;
    private List<ArenaPiece> _placedPieces;

    [SerializeField] private List<ArenaPiece> piecesForGeneration;
    private List<List<ArenaPiece>> _sortedPieces;


    /// Already placed piece being used to judge others
    private GameObject _selectedPiece;

    /// Piece being evaluated against selectedPiece
    private GameObject _evaluatingPiece;


    private int largestGroup;
    private int smallestLargestGroup;
    
    // Start is called before the first frame update
    void Start()
    {


        piecesForGeneration.Sort();
        largestGroup = 
            piecesForGeneration[0].largestGroupCount;
        smallestLargestGroup = 
            piecesForGeneration[piecesForGeneration.Count - 1].largestGroupCount;

        
        // Seperate pieces into seperate lists based on largest group
        SplitList();

        // Place the first piece
        PickFirstPiece();




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

    private void PickFirstPiece()
    {
        _placedPieces = new List<ArenaPiece>();
        if (_setCentralPiece)
        {
            _placedPieces.Add(_centralPiece);
        }
        else
        {
            _placedPieces.Add(
                _sortedPieces[0][Random.Range(
                    0, _sortedPieces[0].Count - 1)]);

        }

        Instantiate(_placedPieces[0]);
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
