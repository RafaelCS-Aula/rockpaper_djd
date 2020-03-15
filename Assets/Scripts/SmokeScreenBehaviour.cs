using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SmokeScreenBehaviour : MonoBehaviour
{

    [SerializeField] private SmokeScreenData smokeScreen;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private float currentLife;

    // Start is called before the first frame update
    void Start()
    {
        /*meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh =*/

        currentLife = 0;

    }

    // Update is called once per frame
    void Update()
    {
        currentLife += Time.deltaTime;
        if(currentLife > smokeScreen.LifeTime)
            Destroy(this);
    }
}
