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
    private Transform closeSafeHideSpot;
    int layerMask;
    private float timer = 1;


    public override void OnEnter()
    {
        layerMask = LayerMask.GetMask("Ground", "Enemy");
        Debug.Log("Ally Entering HideState");
        hasAttacked = false;
        ipf = Instantiate(IndicatorPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
    }
    public override void Execute()
    {
        if (!isHidden)
        {
            Transform closeHideSpot = HidePoints.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
            AgentEnemy.targetPosition = closeHideSpot.transform.position;
            Debug.Log("close hide spot" + closeHideSpot.transform.position);
            Debug.Log("transform.position" + transform.position);
        }
        else
        {
            if (timer > 0)
            {
                timer = 1;
                Debug.Log("Ally Throwing smokebomb");
                AE.TakeDamage(1);
                AE.isStunned = true;
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
    public override void OnExit()
    {
        Debug.Log("Ally Leaving HideState");
        Destroy(ipf);
    }
}
