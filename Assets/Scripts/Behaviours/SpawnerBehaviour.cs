using RPS_DJDIII.Assets.Scripts.DataScriptables.ObjectsData;
using RPS_DJDIII.Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    /// <summary>
    /// Controller for spawning other gameobjects
    /// </summary>
    public class SpawnerBehaviour : MonoBehaviour, IDataUser<SpawnerData>,
     IArenaInitializable
    {
        private List<GameObject> _spawned = new List<GameObject>();

        [SerializeField] private SpawnerData _dataHolder;
        public SpawnerData DataHolder
        {
            get => _dataHolder;
            set => value = _dataHolder;
        }
        public bool isInitialized { get; set; }

        private List<GameObject> _dObjects = new List<GameObject>();
        private float _dSpawnInterval;
        private int _dSingleSpawnIndex;
        private bool _dIsTriggerAction;
        private bool _dStartSpawn;
        private bool _dSingleSpawn;

        private float _spawnTimer = 0;


        private void Awake()
        {
            isInitialized = false;

            if (_dStartSpawn)
            {
                GetData();
                SpawnObjects();
            }
        }

        public void Initialize()
        {
            GetData();


            SpawnObjects();
            isInitialized = true;

        }

        public void Initialize(GameObject specialObject)
        {
            GetData();

            SpawnObjects(specialObject);
            isInitialized = true;
        }

        public void GetData()
        {
            _dObjects = DataHolder.objectsToSpawn;
            _dSingleSpawn = DataHolder.singleSpawn;
            _dSingleSpawnIndex = DataHolder.singleSpawnIndex;
            _dSpawnInterval = DataHolder.spawnInterval;
            _dStartSpawn = DataHolder.startSpawn;
            _dIsTriggerAction = DataHolder.isTriggerAction;


        }

        /// <summary>
        /// Spawn every objecy in the object to spawn list
        /// </summary>
        public void SpawnObjects()
        {
            foreach (GameObject g in _spawned)
            {

                Destroy(g);
            }

            _spawned = new List<GameObject>();

            if (!_dSingleSpawn)
            {
                foreach (GameObject g in _dObjects)
                {
                    if (g != null) _spawned.Add(Instantiate(g));

                }
            }
            else
                _spawned.Add(Instantiate(_dObjects[_dSingleSpawnIndex]));

        }

        /// <summary>
        /// Override the list of objects of this spawner and make it spawn
        /// any other thing
        /// </summary>
        /// <param name="specific"> Object to spawn in this spawner</param>
        public void SpawnObjects(GameObject specific)
        {
            _spawned = new List<GameObject>();
            _spawned.Add(specific);
        }



        // Update is called once per frame
        void Update()
        {
            if (!_dIsTriggerAction)
            {
                _spawnTimer += 1 * Time.deltaTime;
                if (_spawnTimer >= _dSpawnInterval)
                {
                    SpawnObjects();
                    _spawnTimer = 0;
                }

            }

        }


    }
}