using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowBehaviour : AIBehaviour
{
    public GameObject IndicatorPrefab;
    private GameObject ipf;
    public override void OnEnter()
    {
        Debug.Log("Ally Entering FollowState");
        ipf = Instantiate(IndicatorPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
    }
    public override void Execute()
    {
        ipf.transform.position = transform.position + new Vector3(0, 4, 0);
        AgentAlly.targetPosition = Player.playerLocation;
        IndicatorPrefab.transform.position = transform.position + new Vector3(0, 4, 0);

    }
    public override void OnExit()
    {
        Debug.Log("Ally Leaving FollowState");
        Destroy(ipf);
    }
}
