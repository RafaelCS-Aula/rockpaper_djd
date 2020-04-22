using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SmokeScreenBehaviour : MonoBehaviour
{

    [SerializeField] private SmokeScreenData _smokeScreenData;

    private float currentLife;

    // Start is called before the first frame update
    void Awake()
    {
        //transform.localScale = smokeScreen.CustomMeshScale;

        currentLife = 0;

    }

    // Update is called once per frame
    void Update()
    {
        currentLife += Time.deltaTime;
       /* if(currentLife > smokeScreen.LifeTime)
            Destroy(this);*/
 
    }

    private void OnDrawGizmos() 
    {
        
        Gizmos.color = Color.gray;
       /* Gizmos.DrawMesh( smokeScreen.ScreenMesh, transform.position, 
        transform.rotation);*/

            
    }


}
