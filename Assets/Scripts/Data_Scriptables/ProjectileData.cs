using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", 
    menuName = "Data/Projectile/Projectile Data", order = 0)]
public class ProjectileData : ScriptableObject
{
    [Header("Specs")]
    public ProjectileTypes Type;
    public ProjectileTypes LosesToType;
    public float Velocity; 
    public Vector3 customScale = new Vector3(1,1,1);

    [Header("VFX")]
    public GameObject TrailParticlesPrefab;
    public GameObject BirthPartcilesPrefab;
    public GameObject DeathParticlesPrefab;
    [Tooltip("When coliding with a projectile of it's own type.")]
    public GameObject Tieprefab;

    [Header("Testing")]
    public Mesh testingMesh;


}
