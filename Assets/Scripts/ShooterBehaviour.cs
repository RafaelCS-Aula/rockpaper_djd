using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour, IDataUser<ShooterData>
{

    public ShooterData DataHolder {get; set;}

    [SerializeField] private float _dFireRate;

    [SerializeField] private bool _dStartR;
    [SerializeField] private bool _dStartP;
    [SerializeField] private bool _dStartS;

    [SerializeField] private int _dStartRMana;
    [SerializeField] private int _dStartPMana;
    [SerializeField] private int _dStartSMana;

    [SerializeField] private int _dMaxRMana;
    [SerializeField] private int _dMaxPMana;
    [SerializeField] private int _dMaxSMana;

    
    [SerializeField] private int _selectedInventoryIndex = 0;
    private int _oldInventoryIndex = 0;

    [SerializeField] private ProjectileTypes selectedProjectile;
    [SerializeField] private GameObject rockProjectile;
    [SerializeField] private int currentRockMana;

    [SerializeField] private GameObject paperProjectile;
    [SerializeField] private int currentPaperMana;
    [SerializeField] private GameObject scissorProjectile;
    [SerializeField] private int currentScissorMana;


    private GameObject[] _inventory = new GameObject[3];

    // Start is called before the first frame update
    void Awake()
    {

        if(DataHolder == null)
        {
            Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
            throw new UnityException();

        }

        GetData();

        if(_dStartR)
            _inventory[0] = rockProjectile;
        if(_dStartP)
            _inventory[1] = paperProjectile;
        if(_dStartS)
            _inventory[2] = scissorProjectile;

        
    }

    // Update is called once per frame
    void Update()
    {
        currentRockMana = Mathf.Clamp(currentRockMana, 0, _dMaxRMana);
        currentPaperMana = Mathf.Clamp(currentPaperMana, 0, _dMaxPMana);
        currentScissorMana = Mathf.Clamp(currentScissorMana, 0, _dMaxSMana);


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

    // needs direction
    public void Shoot()
    {
        
       
        Instantiate(_inventory[_selectedInventoryIndex]);

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

}
