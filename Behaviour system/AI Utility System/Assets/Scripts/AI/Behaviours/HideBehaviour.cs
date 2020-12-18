using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HideBehaviour : AIBehaviour
{
    private bool hasAttacked;
    private bool isHidden;
    public GameObject IndicatorPrefab;
    private GameObject ipf;
    private AgentEnemy AE;
    [SerializeField]
    private Transform[] HidePoints;
    private Vector3 hideTarget;
    private float timer = 1;


    public override void OnEnter()
    {
        Debug.Log("Ally Entering HideState");
        hasAttacked = false;
        ipf = Instantiate(IndicatorPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
    }
    public override void Execute()
    {
        if (!isHidden)
        {
            Transform closeHideSpot = HidePoints.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
            hideTarget = closeHideSpot.transform.position;
            AgentAlly.targetPosition = hideTarget;
        }
        else
        {
            if (timer < 0)
            {
                timer = 5;
                Debug.Log("Ally Throwing smokebomb");
                AgentEnemy.isStunned = true;
            }
            else
            {
                timer = timer - Time.deltaTime;
            }
        }

        ipf.transform.position = transform.position + new Vector3(0, 4, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HidePoints")
        {
            isHidden = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HidePoints")
        {
            isHidden = false;
        }
    }

    public override void OnExit()
    {
        Debug.Log("Ally Leaving HideState");
        Destroy(ipf);
    }
}
