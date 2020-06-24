using RPS_DJDIII.Assets.Scripts.DataScriptables.ObjectsData;
using RPS_DJDIII.Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SmokeScreenBehaviour : MonoBehaviour, IDataUser<SmokeScreenData>
    {

        [SerializeField] private SmokeScreenData _dataFile;
        public SmokeScreenData DataHolder
        {
            get => _dataFile;
            set => value = _dataFile;
        }

        [SerializeField] private float _dLifeTime;
        [SerializeField] private float _dStartingOpacity;
        [SerializeField] private Vector3 _dCScale;

        [SerializeField] private bool _dVarOpacity;
        [SerializeField] private AnimationCurve _dOpacityCurve;
        private Mesh _dtestMesh;

        private Material _myMat;
        private float _currentLife;

        // Start is called before the first frame update
        void Awake()
        {
            _myMat = GetComponent<MeshRenderer>().material;
            if (DataHolder == null)
            {
                Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
                throw new UnityException();

            }
            GetData();

            transform.localScale = _dCScale;
            float q = _myMat.color.a;
            q *= _dStartingOpacity;
            _currentLife = 0;

        }

        // Update is called once per frame
        void Update()
        {
            if (_dVarOpacity)
            {
                // If this is not doing anything, it might be because
                // The material is not set to have transparency.
                float q = _myMat.color.a;
                q *= _dOpacityCurve.Evaluate(_dLifeTime - _currentLife);

            }

            _currentLife += Time.deltaTime;
            if (_currentLife >= _dLifeTime)
                Destroy(gameObject);

        }

        public void GetData()
        {

            _dCScale = DataHolder.customScale;
            _dLifeTime = DataHolder.LifeTime;
            _dVarOpacity = DataHolder.variableOpacity;
            _dOpacityCurve = DataHolder.opacityOverLifetime;
            _dStartingOpacity = DataHolder.startingOpacity;
            _dtestMesh = DataHolder.testingMesh;

        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.gray;
            Gizmos.DrawMesh(_dtestMesh, transform.position,
            transform.rotation);


        }


    }
}