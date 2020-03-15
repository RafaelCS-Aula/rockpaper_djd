using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", 
    menuName = "Projectile/Smoke Screen", order = 1)]
public class SmokeScreenData : ScriptableObject
{
    //TODO: Fix because cant set default as cube
    [Header("Gameplay")]
    public float LifeTime;

    [Header("Default Mesh (Cube) info")]
    [Tooltip("Length on the X axis")]
    public float Width;

    [Tooltip("Length on the Y axis")]
    public float Height;

    [Tooltip("Length on the Z axis")]
    public float Depth;

    [Header("Rendering")]
    [Tooltip("Material fo the screen")]
    public Material MeshMaterial;

    [Header("Custom Mesh")]
    [Tooltip("Use a custom mesh, the standard one is a cube")]
    public bool UseCustomMesh = false;
    public Mesh CustomMesh;
    public Vector3 CustomMeshScale = new Vector3(1,1,1);
}
