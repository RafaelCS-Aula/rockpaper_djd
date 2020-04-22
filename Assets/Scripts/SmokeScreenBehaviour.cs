using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SmokeScreenBehaviour : MonoBehaviour, IDataUser<SmokeScreenData>
{

    public SmokeScreenData DataHolder {get; set;}

    [SerializeField] private float _dLifeTime;
    [SerializeField] private float _dStartingOpacity;
    [SerializeField] private Vector3 _dCScale;

    [SerializeField] private bool _dVarOpacity;
   [SerializeField] private AnimationCurve _dOpacityCurve;

    private Material _myMat;
    private float _currentLife;

    // Start is called before the first frame update
    void Awake()
    {
        _myMat = GetComponent<MeshRenderer>().material;
        if(DataHolder == null)
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
        if(_dVarOpacity)
        {
            // If this is not doing anything, it might be because
            // The material is not set to have transparency.
            float q = _myMat.color.a;
            q *= _dOpacityCurve.Evaluate(_dLifeTime - _currentLife);

        }

        _currentLife += Time.deltaTime;
        if(_currentLife > _dLifeTime)
            Destroy(this);
 
    }

    public void GetData()
    {

        _dCScale = DataHolder.customScale;
        _dLifeTime = DataHolder.LifeTime;
        _dVarOpacity = DataHolder.variableOpacity;
        _dOpacityCurve = DataHolder.opacityOverLifetime;
        _dStartingOpacity = DataHolder.startingOpacity;

    }

    private void OnDrawGizmos() 
    {
        
        Gizmos.color = Color.gray;
       /* Gizmos.DrawMesh( smokeScreen.ScreenMesh, transform.position, 
        transform.rotation);*/

            
    }


}
