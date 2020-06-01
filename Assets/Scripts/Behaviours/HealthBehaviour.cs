using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour, IUseTeams
{

    [SerializeField] private int _maxHp;
    [SerializeField] private int _currentHp;
    [SerializeField] private int _startingHp;

    public int teamID { get; set; }
    
    // Start is called before the first frame update
    void Awake()
    {
        // TODO: way to have all IDs match in the same game object
        //gameObject.get


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
