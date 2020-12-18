using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackBehaviour : AIBehaviour
{
    public GameObject IndicatorPrefab;
    private GameObject ipf;

    [SerializeField]
    private bool hasWeapon = false;
    private Player P;
    [SerializeField]
    private GameObject[] weapons;
    private GameObject closeWeapon;
    public override void OnEnter()
    {
        ipf = Instantiate(IndicatorPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
        Debug.Log("Enemy Entering AttackState");
        P = GetComponent<Player>(); //Dit werkt alleen als ie aan de component zelf vast zit.
    }
    public override void Execute()
    {
        ipf.transform.position = transform.position + new Vector3(0, 4, 0);
        Vector3 position = transform.position;
        float distanceToPlayer = (position  - Player.playerLocation).magnitude;
        //Debug.Log(distanceToPlayer);
        if (hasWeapon)
        {
            AgentEnemy.targetPosition = Player.playerLocation;
            Debug.Log(distanceToPlayer);
            float timer = 2;
            if (timer > 0 && distanceToPlayer < 5)
            {
                timer = 2;
                Debug.Log("Enemy Attack");
                //P.TakeDamage(1);
            }
            else
            {
                timer = timer - Time.deltaTime;
            }
        }
        else
        {
            closeWeapon = weapons.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
            AgentEnemy.targetPosition = closeWeapon.transform.position;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "Weapon")
        {
            hasWeapon = true;
        }
    }
    public override void OnExit()
    {
        Debug.Log("Enemy Leaving AttackState");
        Destroy(ipf);
    }
}
