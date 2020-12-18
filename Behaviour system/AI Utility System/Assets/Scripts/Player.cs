using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private NavMeshAgent agent;

    public static Vector3 playerLocation;

    [SerializeField]
    private float health;

    void Update()
    {
        playerLocation = transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                Debug.Log("Moving to: " + hit.point);
            }
        }
        
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        CheckHealth(health);
    }
    public void CheckHealth(float health)
    {
        if(health < 0)
        {
            Destroy(this);
        }
    }
}
