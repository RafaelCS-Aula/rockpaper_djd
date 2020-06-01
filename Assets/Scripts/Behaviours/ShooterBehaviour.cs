using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSoundHandler))]
public class ShooterBehaviour : MonoBehaviour, IDataUser<ShooterData>,
    ISoundPlayer<PlayerSoundHandler>, IUseTeams
{

    [SerializeField] private ShooterData _dataFile;
    public ShooterData DataHolder
    {
        get => _dataFile; 
        set => value = _dataFile;
    }
    public int teamID { get; set; }

    public PlayerSoundHandler audioHandler { get ; set ; }

    [Header("Data from Data Holder")]
    [SerializeField] private float _dFireRate;
    [SerializeField] private float _currentFireRate = 0;

    [SerializeField] private bool _dStartR;
    [SerializeField] private bool _dStartP;
    [SerializeField] private bool _dStartS;

    [SerializeField] private int _dStartRMana;
    [SerializeField] private int _dStartPMana;
    [SerializeField] private int _dStartSMana;

    [SerializeField] private int _dMaxRMana;
    [SerializeField] private int _dMaxPMana;
    [SerializeField] private int _dMaxSMana;

    [Header("Behaviour fields")]
    [SerializeField] private int _selectedInventoryIndex = 0;
    private int _oldInventoryIndex = 0;

    [SerializeField] private GameObject _selectedProjectile;
    [SerializeField] private GameObject _rockProjectile;
    [SerializeField] private int _currentRockMana;

    [SerializeField] private GameObject _paperProjectile;
    [SerializeField] private int _currentPaperMana;
    [SerializeField] private GameObject _scissorProjectile;
    [SerializeField] private int _currentScissorMana;


    private ProjectileTypes[] _inventory = new ProjectileTypes[3];

    private CameraRig cameraRig;

    [HideInInspector]
    public Vector3 shootingTarget;

    // Start is called before the first frame update
    void Awake()
    {
        audioHandler = GetComponent<PlayerSoundHandler>();
        if(DataHolder == null)
        {
            Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
            throw new UnityException();

        }

        teamID = Random.Range(0, 100);

        cameraRig = GetComponentInChildren<CameraRig>();
        //shootingTarget = transform.forward;
        GetData();

        if(_dStartR)
        {
            _inventory[0] = ProjectileTypes.ROCK;
            _currentRockMana = _dStartRMana;
        }
            
        if(_dStartP)
        {
            _inventory[1] = ProjectileTypes.PAPER;
            _currentPaperMana = _dStartPMana;
        }
            
        if(_dStartS)
        {
            _inventory[2] = ProjectileTypes.SCISSORS;
            _currentScissorMana = _dStartSMana;
        }
            
        


        
    }

    // Update is called once per frame
    void Update()
    {
        
        _currentFireRate += Time.deltaTime;

        _currentRockMana = Mathf.Clamp(_currentRockMana, 0, _dMaxRMana);
        _currentPaperMana = Mathf.Clamp(_currentPaperMana, 0, _dMaxPMana);
        _currentScissorMana = Mathf.Clamp(_currentScissorMana, 0, _dMaxSMana);

        _selectedInventoryIndex = Mathf.Clamp(
            _selectedInventoryIndex, 0, _inventory.Length - 1);


    }

    /// <summary>
    /// Selects weapon using relative inventory order. Selecting the
    /// "next" or "previous" weapon in the inventory.
    /// </summary>
    /// <param name="indexDelta"> Negative or positive number indicating if the
    /// selection goes to the next or previous weapon.</param>
    public void SelectWeapon(int indexDelta)
    {
        indexDelta = Mathf.Clamp(indexDelta, -1,1);
        int newIndex = _selectedInventoryIndex + indexDelta;
        if(newIndex >= 0 && newIndex < _inventory.Length)
        {
            _oldInventoryIndex = _selectedInventoryIndex;
            _selectedInventoryIndex = newIndex;
        }
        // WRAPPING
        else if(newIndex > _inventory.Length - 1)
        {
            _oldInventoryIndex = _selectedInventoryIndex;
            _selectedInventoryIndex = 0;
        }
        else if(newIndex < 0)
        {
            _oldInventoryIndex = _selectedInventoryIndex;
            _selectedInventoryIndex = _inventory.Length - 1;

        }

    }

    /// <summary>
    /// For selection of weapon trough the numbers on the keyboard
    /// directly.
    /// </summary>
    /// <param name="input"> The number inputed by the user on their KB</param>
    public void SelectWeapon(uint input)
    {
        uint newIndex = input - 1;
        
        if(newIndex < _inventory.Length && newIndex != _selectedInventoryIndex)
        {
            _oldInventoryIndex = _selectedInventoryIndex;
            _selectedInventoryIndex = (int)newIndex;

        }
            
    }

    /// <summary>
    /// Makes current weapon the last weapon the agent had selected before the 
    /// current one.
    /// </summary>
    public void SelectLastWeapon()
    {
        if(_oldInventoryIndex != _selectedInventoryIndex)
        {
            int newIndex = _oldInventoryIndex;
            _oldInventoryIndex = _selectedInventoryIndex;
            _selectedInventoryIndex = newIndex;
        }


    }


    public void Shoot()
    {
        if (_currentFireRate <= _dFireRate)
            return;

        switch (_inventory[_selectedInventoryIndex])
        {
            case(ProjectileTypes.ROCK):
                _selectedProjectile = _rockProjectile;
                if(_currentRockMana <= 0) return;
                else _currentRockMana --;
                break;
            case(ProjectileTypes.PAPER):
                _selectedProjectile = _paperProjectile;
                if(_currentPaperMana <= 0) return;
                else _currentPaperMana --;
                break;
            case(ProjectileTypes.SCISSORS):
                _selectedProjectile = _scissorProjectile;
                if(_currentScissorMana <= 0) return;
                else _currentScissorMana --;
                break;
            default:
                _selectedProjectile = null;
                break;

        }

        if (_selectedProjectile != null)
        {

            shootingTarget = cameraRig.GetCenterTarget();

            Quaternion t 
                = Quaternion.LookRotation(
                    shootingTarget - transform.position, transform.up);

            // Play shooting sound
            audioHandler.PlayAudio(audioHandler.dShot, 0.7f);

            GameObject b = Instantiate(
                _selectedProjectile, 
                transform.position + transform.forward * 1.5f, t);

            b.GetComponent<ProjectileBehaviour>().teamID = this.teamID;

            _currentFireRate = 0;

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    /// <returns>Returns true if it used the amount given, false if
    /// the mana count for the given type was already at max.</returns>
    public bool AddMana(int amount, ProjectileTypes type)
    {
        Debug.Log("adding");
        switch (type)
        {
            case (ProjectileTypes.ROCK):
                if (_currentRockMana + amount >= _dMaxRMana)
                {
                    _currentRockMana = _dMaxRMana;
                    break;
                }
                    
                if (_currentRockMana == _dMaxRMana)
                    return false;
                else
                    _currentRockMana += amount;
                break;
            case (ProjectileTypes.PAPER):
                if (_currentPaperMana + amount >= _dMaxPMana)
                {
                    _currentPaperMana = _dMaxPMana;
                    break;
                }
                    
                if (_currentPaperMana == _dMaxPMana)
                    return false;
                else
                    _currentPaperMana += amount;
                break;
            case (ProjectileTypes.SCISSORS):
                if (_currentScissorMana + amount >= _dMaxSMana)
                {
                    _currentScissorMana = _dMaxSMana;
                    break;
                }
                    
                if (_currentScissorMana == _dMaxSMana)
                    return false;
                else
                    _currentScissorMana += amount;
                break;                
        }
        return true;
        
    }

    public (float max, float current)GetMana(ProjectileTypes type)
    {
        switch (type)
        {
            case (ProjectileTypes.ROCK):
                return (_dMaxRMana, _currentRockMana);
            case (ProjectileTypes.PAPER):
                return (_dMaxPMana, _currentPaperMana);
            case (ProjectileTypes.SCISSORS):
                return (_dMaxSMana, _currentScissorMana);
        }
        return (0, 0);
    }

    public ProjectileTypes GetSelectedWeapon() => 
        _inventory[_selectedInventoryIndex];

    public void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(shootingTarget, 0.5f);
        Gizmos.DrawRay(transform.position,shootingTarget - transform.position);

                
    }


    public void GetData()
    {

        _dFireRate = DataHolder.FireRate;

        _dMaxRMana = DataHolder.MaxRockMana;
        _dMaxPMana = DataHolder.MaxPaperMana;
        _dMaxSMana = DataHolder.MaxScissorMana;

        _dStartR = DataHolder.StartWithRock;
        _dStartP = DataHolder.StartWithPaper;
        _dStartS = DataHolder.StartWithScissor;    

        _dStartRMana = DataHolder.StartingRockMana;
        _dStartPMana = DataHolder.StartingPaperMana;
        _dStartSMana = DataHolder.StartingScissorMana;



    }

    public void InteractEnemy(IUseTeams other) { }
    public void InteractFriend(IUseTeams other) { }

}
