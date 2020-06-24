using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPS_DJDIII.Assets.Scripts.Enums;

namespace RPS_DJDIII.Assets.Scripts.ArenaGeneration
{

    public class GenerationManager : MonoBehaviour
    {
    
        [Header("----- Content Settings ------")]
        [SerializeField] private List<ArenaPiece> piecesForGeneration;

        [Header("Starting Piece Settings ---")]
        [SerializeField] private bool _setStartingPiece = false;
        [SerializeField] private List<ArenaPiece> _possibleStartingPieces;
        [SerializeField] private uint _connectorCountTolerance = 0;

        [Header("------ General Generation Settings --------")]

        [SerializeField] private GenerationTypes _generationMethod;
        [SerializeField] private uint _maxLevelPieceCount;
        [SerializeField] private uint _groupTolerance = 0;
        [SerializeField] private bool _clippingCorrection = false;
        [SerializeField] private float _pieceDistance = 0.0001f;

        [Header("Star & Branch Generation Settings --------")]
        [SerializeField] private uint _branchPieceCount;
        [SerializeField] private int _branchSizeVariance = 0;

        [Header("---")]
        [SerializeField] private uint _branchGenPieceSkipping = 0;
        [SerializeField] private uint _PieceSkippingVariance = 0;

        /*[Header("------ Vertical Level Settings --------")]
        [SerializeField] private bool _createUpperLevel;

        [Tooltip("Choose random pieces and put other pieces above them.")]
        [SerializeField] private bool _upperLevelIslandGeneration;
        [SerializeField] private int _upperIslandsCount = 1;

        [Header("--")]
        [SerializeField] private bool _createLowerLevel;

        [Tooltip("Choose random pieces and put other pieces under them.")]
        [SerializeField] private bool _lowerLevelIslandGeneration;
        [SerializeField] private int _lowerIslandsCount = 1;*/


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

            /*if(_createUpperLevel || _createLowerLevel)
                MakeVerticalArena(_placedPieces);*/


            /*foreach (ArenaPiece item in _placedPieces)
            {
                print(item.gameObject);
            }*/
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
            int placedAmount = 0;
            int jumpsTaken = 0;
            int failureCount = 0;
            uint maxFailures = _maxLevelPieceCount;
            // Pieces placed by this method call
            List<ArenaPiece> arena = new List<ArenaPiece>(); 
            //List<ArenaPiece> spawnedArena = new List<ArenaPiece>();

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
                {
                    _selectedPiece = arena[i];
                    //placedAmount++;
                }
                    


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
                    goto selectPiece;
                
                }

        
                _evaluatingPiece = _sortedPieces[myPieceList][rng];
                
                

                GameObject spawnedPiece = Instantiate(_evaluatingPiece).gameObject;
                ArenaPiece spawnedScript = spawnedPiece.GetComponent<ArenaPiece>();

                (bool valid, Transform trn) evaluationResult =
                    _selectedPiece.EvaluatePiece(spawnedScript, 
                    _pieceDistance, 
                    _groupTolerance);

                // If things worked out, spawn the piece in the correct position
                if(evaluationResult.valid)
                {
                    spawnedPiece.name = $"{i}";
                    arena.Add(spawnedScript);
                    placedAmount++;
                    print("Found piece.");
                    spawnedScript.gameObject.transform.SetParent(_selectedPiece.transform);

                    if(arena.Count >= _maxLevelPieceCount)
                        return arena;

                }
                else
                {
                    if (failureCount > maxFailures)
                        return arena;

                    // No valid connectors in the given piece
                    Destroy(spawnedPiece);
                    _evaluatingPiece.wasAnalysed = true;
                    failureCount++;
                    
                    goto selectPiece;

                } 

                
                // if this one has no more free connectors, move on to the next 
                // placed piece
                switch(_generationMethod)
                {
                    case GenerationTypes.CORRIDOR:
                    continue;

                    // For some reason the first branch gets double pieces
                    case GenerationTypes.STAR:
                        int[] multi = new int[] {-1, 1};
                        int variance = 
                            multi[Random.Range(0,2)] * _branchSizeVariance;

                        if(placedAmount < _branchPieceCount + variance)
                        {
                            
                            continue;
                        }
                            

                        else if(placedAmount >= _branchPieceCount + variance &&
                                !startingPiece.IsFull())
                        {
                            print($"placed: {placedAmount}, arena: {arena.Count}");
                            placedAmount = 0;
                            _selectedPiece = startingPiece;

                            // it works dont ask me why
                            i += 1;
                            print($"Selected piece is now {_selectedPiece.gameObject} - index {i}");
                            goto selectPiece;
                        }
                        return arena;

                    case GenerationTypes.BRANCH:
                        int[] mult = new int[] {-1, 1};
                        int multiplier =  
                            mult[Random.Range(0,2)] * _branchSizeVariance;

                        if(placedAmount < _branchPieceCount + multiplier)
                            continue;
                        else if(placedAmount >= _branchPieceCount + multiplier)
                        {
                            int variableVariance;
                            variableVariance =mult[(int)Random.Range(0,
                                _PieceSkippingVariance + 1)];

                            int dist = 
                            (int)_branchGenPieceSkipping + variableVariance * jumpsTaken;

                            int jump = (int)Mathf.Clamp(dist,
                            1, arena.Count - 1);

                            if(!arena[0 + jump].IsFull())
                            {
                                placedAmount = 0;
                                _selectedPiece = arena[0 + jump];
                                jumpsTaken++;
                                goto selectPiece;

                            }
                                
                        }
                        break;
                }


                if(_selectedPiece.IsFull())
                    continue;
                else // else choose another piece to evaluate for this one
                    goto selectPiece;
            }

            return arena;
        }

        // Vertical generation is a no-go for now
        /// <summary>
        /// Create the vertical upper and lower levels of the arena
        /// </summary>
        /// <param name="arena">The placed pieces that will be selected</param>
    /* private void MakeVerticalArena(ICollection<ArenaPiece> arena)
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
                (placedHaveTopConns[Random.Range(0, placedHaveTopConns.Count)]));

            }

            // Lower Levels
            if (_lowerLevelIslandGeneration && placedHaveBotConns.Count > 0)
            {
                int placed = 0;
                while(placed < _lowerIslandsCount)
                {
                    int rng = Random.Range(0, placedHaveBotConns.Count);
                    _selectedPiece = placedHaveBotConns[rng];

                    foreach (ArenaPiece a in placedHaveBotConns)
                        a.wasAnalysed = false;

                    pickEvaluatingPiece:

                    int sortRng = Random.Range(0, _sortedPieces.Count);
                    int listRng = Random.Range(0, _sortedPieces[sortRng].Count);

                    _evaluatingPiece = _sortedPieces[sortRng][listRng];

                    GameObject spawnedPiece =
                                    Instantiate(_evaluatingPiece).gameObject;
                    ArenaPiece spawnedScript =
                        spawnedPiece.GetComponent<ArenaPiece>();

                    if (_evaluatingPiece != null && !_evaluatingPiece.wasAnalysed)
                        evaluationResult = _selectedPiece.EvaluatePieceVertical(
                            spawnedScript, false);
                    else
                        goto pickEvaluatingPiece;

                    if (evaluationResult.valid)
                    {
                        _placedPieces.Add(spawnedScript);
                        placed++;

                    }
                    else
                    {
                        _evaluatingPiece.wasAnalysed = true;
                        Destroy(spawnedPiece);
                        goto pickEvaluatingPiece;
                    }



                }


            }
            else if (!_lowerLevelIslandGeneration && placedHaveBotConns.Count > 0)
            {

                int rng = Random.Range(0, placedHaveBotConns.Count);
                _selectedPiece = placedHaveBotConns[rng];

                foreach (ArenaPiece a in placedHaveBotConns)
                    a.wasAnalysed = false;

                pickEvaluatingPiece:

                int sortRng = Random.Range(0, _sortedPieces.Count);
                int listRng = Random.Range(0, _sortedPieces[sortRng].Count);

                _evaluatingPiece = _sortedPieces[sortRng][listRng];

                GameObject spawnedPiece =
                                Instantiate(_evaluatingPiece).gameObject;
                ArenaPiece spawnedScript =
                    spawnedPiece.GetComponent<ArenaPiece>();

                if (_evaluatingPiece != null && !_evaluatingPiece.wasAnalysed)
                    evaluationResult = _selectedPiece.EvaluatePieceVertical(
                        spawnedScript, false);
                else
                    goto pickEvaluatingPiece;

                if (evaluationResult.valid)
                {
                    _placedPieces.Add(spawnedScript);
                    _placedPieces.AddRange(MakeHorizontalArena(spawnedScript));

                }
                else
                {
                    _evaluatingPiece.wasAnalysed = true;
                    Destroy(spawnedPiece);
                    goto pickEvaluatingPiece;
                }
            }
        }*/

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
                
                if(_generationMethod == GenerationTypes.CORRIDOR ||
                _generationMethod == GenerationTypes.BRANCH)
                {

                    choosenIndex = Random.Range(
                        0, _sortedPieces[_sortedPieces.Count - 1].Count);
                        
                    choosen = _sortedPieces[_sortedPieces.Count - 1][choosenIndex];

                }
                    
                else
                {

                    choosenIndex = Random.Range(0, _sortedPieces[0].Count);
                    choosen = _sortedPieces[0][choosenIndex];


                }
                    
            }
            GameObject piece = Instantiate(choosen.gameObject);
            _placedPieces.Add(piece.GetComponent<ArenaPiece>());
            
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
}

