using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), 
typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] private ProjectileData _projectileData;
    
    
    public ProjectileTypes dMyType {get; private set;}
    private ProjectileTypes _dLoseToType;

    [SerializeField] private float _dVelocity;
    [SerializeField] private Vector3 _dCScale;
    
    private GameObject _dBirthParticles;
    private GameObject _dTrailParticles;
    private GameObject _dDeathParticles;
    private GameObject _dTieParticles;


    private Rigidbody _rigidBody;
    private MeshCollider _collider;


    // Start is called before the first frame update
    void Awake()
    {


        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<MeshCollider>();

        if(_projectileData == null)
        {
            Debug.LogError("Object doesn't have a ProjectileData assigned.");
            throw new UnityException();

        }

        // Read data from the projectile scriptable object
        GetData();

        _collider.convex = true;
        _collider.isTrigger = true;
       
        dMyType = _projectileData.Type;
        transform.localScale = _dCScale;

        // Make it go move forward
        _rigidBody.isKinematic = false;
        _rigidBody.drag = 0.0f;
        _rigidBody.angularDrag = 0.0f;
        _rigidBody.useGravity = false;
        _rigidBody.AddForce(_dVelocity * transform.forward);


    }

    // When encoutnering other triggers; other projectiles
    private void OnTriggerEnter(Collider other) 
    {
        ProjectileBehaviour encountered 
            = other.GetComponent<ProjectileBehaviour>();

        if(encountered == null)
            return;
        
        if(encountered.dMyType == _projectileData.LosesToType)
        {
            Lose();
        }
        else if(encountered.dMyType == dMyType)
        {

            SpawnSmoke();

        }
        
    }

    // Destroy this projectile, spawning some particles in the process
    private void Lose()
    {
        Instantiate(_dDeathParticles,
            transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void SpawnSmoke()
    {

        Instantiate(_dTieParticles,
            transform.position, transform.rotation);

    }

    private void GetData()
    {
        dMyType = _projectileData.Type;
        _dVelocity = _projectileData.Velocity;
        _dLoseToType = _projectileData.LosesToType;
        _dBirthParticles = _projectileData.BirthPartcilesPrefab;
        _dDeathParticles = _projectileData.DeathParticlesPrefab;
        _dTieParticles = _projectileData.Tieprefab;
        _dTrailParticles = _projectileData.TrailParticlesPrefab;
        _dCScale = _projectileData.customScale;


    }


    private void OnDrawGizmos() 
    {
        switch(dMyType)
        {
            case ProjectileTypes.PAPER:
                Gizmos.color = Color.green;
                break;
            case ProjectileTypes.ROCK:
                Gizmos.color = Color.red;
                break;
            case ProjectileTypes.SCISSORS:
                Gizmos.color = Color.blue;
                break;
            default:
                 Gizmos.color = Color.white;
                 break;
        }
        Gizmos.DrawRay(transform.position, transform.forward * 10);
        Gizmos.DrawWireMesh(_projectileData.testingMesh , 
            transform.position, transform.rotation);

            
    }

}
