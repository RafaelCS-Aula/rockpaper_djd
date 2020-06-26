using RPS_DJDIII.Assets.Scripts.DataScriptables.ObjectsData;
using RPS_DJDIII.Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    // TODO: this
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

        private  List<GameObject> _dObjects;
        private float _dSpawnInterval;
        private int _dSingleSpawnIndex;
        private bool _dIsTriggerAction;
        private bool _dStartSpawn;
        private bool _dSingleSpawn;

        private float _spawnTimer = 0;


        public void Initialize()
        {   
            GetData();

            if(_dStartSpawn)
                SpawnObjects();

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

        public void SpawnObjects()
        {
            _spawned = new List<GameObject>();

            if(!_dSingleSpawn)
            {
                foreach(GameObject g in _dObjects)
                _spawned.Add(Instantiate(g));
            }
            else
                _spawned.Add(Instantiate(_dObjects[_dSingleSpawnIndex]));
            
        }

        public void SpawnObjects(GameObject specific)
        {
            _spawned = new List<GameObject>();
            _spawned.Add(specific);
        }



        // Update is called once per frame
        void Update()
        {
            if(!_dIsTriggerAction)
            {
                _spawnTimer += 1 * Time.deltaTime;
                if(_spawnTimer >= _dSpawnInterval)
                {
                    SpawnObjects();
                    _spawnTimer = 0;
                }
                    
            }

        }

    
    }
}