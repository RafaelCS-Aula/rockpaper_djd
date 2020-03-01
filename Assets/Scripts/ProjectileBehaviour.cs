using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), 
typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] private ProjectileData Projectile;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Rigidbody rigidbody;

    private ParticleSystem trailParticles;
    private ParticleSystem deathParticles;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        rigidbody = GetComponent<Rigidbody>();

        if(Projectile == null)
        {
            Debug.LogError("Object doesn't have a ProjectileData assigned.");
            throw new UnityException();

        }

        // Read data from the projectile scriptable object
        meshFilter.mesh = Projectile.ProjectileMesh;
        meshRenderer.materials[0] = Projectile.MeshMaterial;
        meshCollider.sharedMesh = Projectile.ProjectileMesh;
        meshCollider.isTrigger = true;

        if(Projectile.TrailParticles != null)
        {
            trailParticles 
                = gameObject.AddComponent<ParticleSystem>();
            trailParticles = Projectile.TrailParticles;

        }
        if(Projectile.DeathParticles != null)
        {
            deathParticles 
                = gameObject.AddComponent<ParticleSystem>();
           deathParticles = Projectile.DeathParticles;

        }
        

        // Make it go move
        rigidbody.drag = 0.0f;
        rigidbody.angularDrag = 0.0f;
        rigidbody.AddForce(Projectile.Velocity * Vector3.forward);

    }

    
}
