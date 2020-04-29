using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ManaPickupBehaviour : MonoBehaviour, IDataUser<ManaPickupData>
{

    [SerializeField] ManaPickupData _dataHolder;
    public ManaPickupData DataHolder 
    {
        get => _dataHolder;
        set => value = _dataHolder;
    }

    private ProjectileTypes _dManaGiven;
    private int _dAmount;
    private bool _dIsTemporary;
    private float _dLifetime;
    private bool _dGivesWeapon;
    private bool _dNeedsWeapon;

    private float currentTime;

    // Start is called before the first frame update
    void Awake()
    {
        GetData();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;   
        currentTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_dIsTemporary)
        {
            currentTime += Time.deltaTime;
            if(currentTime <= _dLifetime)
                Destroy(this);
        }
    }

    public void GetData()
    {
        _dManaGiven = DataHolder.manaGiven;
        _dAmount = DataHolder.amount;
        _dIsTemporary = DataHolder.isTemporary;
        _dLifetime = DataHolder.lifeTime;
        _dGivesWeapon = DataHolder.givesWeapon;
        _dNeedsWeapon = DataHolder.needsWeapon;
    }

    private void OnDrawGizmos() 
    {
        switch(_dataHolder.manaGiven)
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
        //Gizmos.DrawRay(transform.position, transform.forward * 10);
        Gizmos.DrawSphere(transform.position, 0.4f);
        

    }
}
