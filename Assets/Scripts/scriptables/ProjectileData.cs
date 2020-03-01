using UnityEngine;
using RPS_DJDIII.Assets.Scripts;

[CreateAssetMenu(fileName = "Projectile", 
    menuName = "Projectile/ProjectileData", order = 0)]
public class ProjectileData : ScriptableObject
{
    [Header("Specs")]
    [SerializeField] private ProjectileTypes Type;
    [SerializeField] private ProjectileTypes LosesToType;
    [SerializeField] private float Velocity; 


    [Header("Rendering")]
    [SerializeField] private Mesh Projectilemesh;
    [SerializeField] private Material MeshMaterial;

    [Header("VFX")]
    [SerializeField] private ParticleSystem TrailParticles;
    [SerializeField] private ParticleSystem BirthPartciles;
    [SerializeField] private ParticleSystem DeathParticles;



    

}
