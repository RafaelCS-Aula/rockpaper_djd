using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), 
typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour, IDataUser<ProjectileData>
{

    [SerializeField] private ProjectileData _dataFile;
    public ProjectileData DataHolder 
    {
        get => _dataFile; 
        set => value = _dataFile;
    }
    
    
    public ProjectileTypes dMyType {get; private set;}
    private ProjectileTypes _dLoseToType;

    [SerializeField] private float _dVelocity;
    [SerializeField] private Vector3 _dCScale;
    
    private GameObject _dBirthParticles;
    private GameObject _dTrailParticles;
    private GameObject _dDeathParticles;
    private GameObject _dTieParticles;
    private Mesh _dTestCollisionMesh;

    private Rigidbody _rigidBody;
    private MeshCollider _collider;


    // Start is called before the first frame update
    void Awake()
    {


        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<MeshCollider>();

        if(DataHolder == null)
        {
            Debug.LogError("Object doesn't have a ProjectileData assigned.");
            throw new UnityException();

        }

        // Read data from the projectile scriptable object
        GetData();

        _collider.convex = true;
        _collider.isTrigger = true;
       
        dMyType = DataHolder.Type;
        transform.localScale = _dCScale;

        if(_dTestCollisionMesh != null)
        {
            _collider.sharedMesh = _dTestCollisionMesh;

        }

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
        
        if(encountered.dMyType == _dLoseToType)
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
        if(_dDeathParticles != null)
            Instantiate(_dDeathParticles,
                transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void SpawnSmoke()
    {
        if(_dTieParticles != null)
            Instantiate(_dTieParticles,
                transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void GetData()
    {
        dMyType = DataHolder.Type;
        _dVelocity = DataHolder.Velocity;
        _dLoseToType = DataHolder.LosesToType;
        _dBirthParticles = DataHolder.BirthPartcilesPrefab;
        _dDeathParticles = DataHolder.DeathParticlesPrefab;
        _dTieParticles = DataHolder.Tieprefab;
        _dTrailParticles = DataHolder.TrailParticlesPrefab;
        _dCScale = DataHolder.customScale;
        _dTestCollisionMesh = DataHolder.testingMesh;


    }


   private void OnDrawGizmos() 
    {
        switch(DataHolder.Type)
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
        Gizmos.DrawWireMesh(DataHolder.testingMesh , 
            transform.position, transform.rotation);

            
    }

}
