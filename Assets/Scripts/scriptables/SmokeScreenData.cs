using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmokeScreen", 
    menuName = "Projectile/Smoke Screen Data", order = 1)]
public class SmokeScreenData : ScriptableObject
{
    //TODO: Fix because cant set default as cube
    [Header("Gameplay")]
    public float LifeTime;

    [Header("Rendering")]
    [Tooltip("Material fo the smoke screen")]
    public Material MeshMaterial;
    public Mesh ScreenMesh;
    public Vector3 CustomMeshScale = new Vector3(1,1,1);
}
