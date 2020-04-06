using UnityEngine;
using RPS_DJDIII.Assets.Scripts;

[CreateAssetMenu(fileName = "Projectile", 
    menuName = "Projectile/Projectile Data", order = 0)]
public class ProjectileData : ScriptableObject
{
    [Header("Specs")]
    public ProjectileTypes Type;
    public ProjectileTypes LosesToType;
    public float Velocity; 

    [Header("Smoke Screen")]
    [Tooltip("When coliding with a projectile of it's own type.")]
    public GameObject SmokeScreenprefab;

    [Header("Rendering")]
    public Mesh ProjectileMesh;
    public Material MeshMaterial;

    [Header("VFX")]
    public ParticleSystem TrailParticles;
    public ParticleSystem BirthPartciles;
    public ParticleSystem DeathParticles;

}
