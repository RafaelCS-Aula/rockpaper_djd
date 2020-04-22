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

    
    private GameObject selectedProjectilePrefab;

    [SerializeField] private ProjectileTypes selectedProjectile;
    [SerializeField] private GameObject rockProjectile;
    [SerializeField] private int currentRockMana;

    [SerializeField] private GameObject paperProjectile;
    [SerializeField] private int currentPaperMana;
    [SerializeField] private GameObject scissorProjectile;
    [SerializeField] private int currentScissorMana;


    

    // Start is called before the first frame update
    void Awake()
    {

        if(DataHolder == null)
        {
            Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
            throw new UnityException();

        }

        GetData();

        
    }

    // Update is called once per frame
    void Update()
    {
        currentRockMana = Mathf.Clamp(currentRockMana, 0, _dMaxRMana);
        currentPaperMana = Mathf.Clamp(currentPaperMana, 0, _dMaxPMana);
        currentScissorMana = Mathf.Clamp(currentScissorMana, 0, _dMaxSMana);


    }

    // needs direction
    public void Shoot()
    {
        switch(selectedProjectile)
        {
            case(ProjectileTypes.ROCK):
                selectedProjectilePrefab = rockProjectile;
                break;
            case(ProjectileTypes.PAPER):
                selectedProjectilePrefab = paperProjectile;
                break;
            case(ProjectileTypes.SCISSORS):
                selectedProjectilePrefab = scissorProjectile;
                break;
        }


        Instantiate(selectedProjectilePrefab);

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
