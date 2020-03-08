using RPS_DJDIII.Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), 
typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] private ProjectileData Projectile;

    public ProjectileTypes MyType {get; private set;}

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Rigidbody rigidBody;

    private ParticleSystem trailParticles;
    private ParticleSystem deathParticles;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        rigidBody = GetComponent<Rigidbody>();

        if(Projectile == null)
        {
            Debug.LogError("Object doesn't have a ProjectileData assigned.");
            throw new UnityException();

        }

        // Read data from the projectile scriptable object
        meshFilter.mesh = Projectile.ProjectileMesh;
        meshRenderer.materials[0] = Projectile.MeshMaterial;
        meshCollider.sharedMesh = Projectile.ProjectileMesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
       
        MyType = Projectile.Type;

        if(Projectile.TrailParticles != null)
        {
            trailParticles 
                = gameObject.AddComponent<ParticleSystem>();
            trailParticles = Projectile.TrailParticles;

            var e = trailParticles.emission;
            e.enabled = true;
        }
        if(Projectile.DeathParticles != null)
        {
            deathParticles 
                = gameObject.AddComponent<ParticleSystem>();
           deathParticles = Projectile.DeathParticles;
           var e = deathParticles.emission;
           e.enabled = false;

        }


        // Make it go move
        rigidBody.isKinematic = true;
        rigidBody.drag = 0.0f;
        rigidBody.angularDrag = 0.0f;
        rigidBody.AddForce(Projectile.Velocity * Vector3.forward);
        rigidBody.useGravity = false;

    }

    // When encoutnering other triggers; other projectiles
    private void OnTriggerEnter(Collider other) 
    {
        ProjectileBehaviour encountered 
            = other.GetComponent<ProjectileBehaviour>();

        if(encountered == null)
            return;
        
        if(encountered.MyType == Projectile.LosesToType)
        {
            Lose();
        }
        
    }

    private void Lose()
    {
        deathParticles?.Emit(20);
        Destroy(this.gameObject);
    }

}
