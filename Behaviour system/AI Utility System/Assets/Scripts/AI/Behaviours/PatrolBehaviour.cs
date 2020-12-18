using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatrolBehaviour : AIBehaviour
{
    public GameObject IndicatorPrefab;
    private GameObject ipf;
    [SerializeField]
    private GameObject[] patrolTransforms;
    [SerializeField]
    private GameObject patrolPrefab;
    private GameObject pp;
    private Vector3 patrolTarget;
    private Vector3 patrolExitLocation;
    private Vector3 offsetVector = new Vector3(0f, 1.4f, 0f);
    int patrolInt = 0;
    private bool patrolPointReached;
    [SerializeField]


    //Een check toevoegen die kijkt of de speler in FOV is als dat zo is dan zal de motivatie om de speler aan te gaan vallen omhoog gaan.
    public override void OnEnter()
    {
        patrolPointReached = false;
        Debug.Log("Enemy Entering PatrolState");
        ipf = Instantiate(IndicatorPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
        if(patrolExitLocation == new Vector3(0, 0, 0))
        {
            //get a new location because this is the first time.
            patrolTarget = (patrolTransforms[patrolInt].transform.position - offsetVector);
            patrolInt++;
        }
        else
        {
            //This is not the first time we enter this state so it needs to get on where it left.
            patrolTarget = patrolExitLocation;
        }
        pp = Instantiate(patrolPrefab, patrolExitLocation, Quaternion.identity);

    }
    public override void Execute()
    {
        ipf.transform.position = transform.position + new Vector3(0, 4, 0);
        AgentEnemy.targetPosition = patrolTarget;
        if(patrolPointReached == true)
        {
            //Patroltarget is reached so make the next target the new target.
            patrolTarget = (patrolTransforms[patrolInt].transform.position - offsetVector);
            patrolInt++;
            if (patrolInt == 3)
            {
                patrolInt = 0;
            }
            patrolPointReached = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PatrolPoint")
        {
            patrolPointReached = true;
        }
        if (other.tag == "ExitPatrolPoint")
        {
            patrolPointReached = true;
            Destroy(pp);
        }

    }
    public override void OnExit()
    {
        Debug.Log("Enemy Leaving PatrolState");
        patrolExitLocation = transform.position;
        Destroy(ipf);
    }
}
