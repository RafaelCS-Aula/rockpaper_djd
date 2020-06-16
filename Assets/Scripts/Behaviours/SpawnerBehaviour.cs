using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    // TODO: this
    public class SpawnerBehaviour : MonoBehaviour, IDataUser<ManaPickupData>
    {
        [SerializeField] private ManaPickupData _dataHolder;
        public ManaPickupData DataHolder
        {
            get => _dataHolder;
            set => value = _dataHolder;
        }


        public void GetData()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}