using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{

    [SerializeField] private ShooterData shooterData;

    [SerializeField] private ProjectileTypes selectedProjectile;
    private GameObject selectedProjectilePrefab;

    [SerializeField] private GameObject rockProjectile;
    [SerializeField] private int currentRockMana;

    [SerializeField] private GameObject paperProjectile;
    [SerializeField] private int currentPaperMana;
    [SerializeField] private GameObject scissorProjectile;
    [SerializeField] private int currentScissorMana;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
