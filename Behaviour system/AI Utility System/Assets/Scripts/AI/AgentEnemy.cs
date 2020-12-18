using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentEnemy : MonoBehaviour, IDamageable
{
    [Header("Variables")]
    [SerializeField]
    private float maxAngle;
    [SerializeField]
    private float range;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    public static bool isInFov;
    [SerializeField]
    private float health;

    public bool isStunned = false;
    public static Vector3 targetPosition;

    [Header("Gameobjects")]
    [SerializeField]
    public Player target;
    public AIBehaviourSelector AISelector { get; private set; }
    public BlackBoard BlackBoard { get; private set; }

    private NavMeshAgent agent;

    private void Start()
    {
        OnInitialize();
        agent.stoppingDistance = 0.6f;
    }
    public void OnInitialize()
    {
        AISelector = GetComponent<AIBehaviourSelector>();
        BlackBoard = GetComponent<BlackBoard>();
        BlackBoard.OnInitialize();
        AISelector.OnInitialize(BlackBoard);
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        //FloatValue.
        AISelector.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
        FloatValue distance = BlackBoard.GetFloatVariableValue(VariableType.Distance);
        distance.Value = transform.position.magnitude;
        agent.SetDestination(targetPosition); //Deze werkt pas als de behaviours werken.
        //agent.SetDestination(Player.playerLocation);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        CheckHealth(health);
        FloatValue hp = BlackBoard.GetFloatVariableValue(VariableType.Health);
        if (hp)
        {
            hp.Value -= damage;
        }
        AISelector.EvaluateBehaviours();
    }
    public void CheckHealth(float health)
    {
        if (health < 0)
        {
            Destroy(this);
        }
    }
    private void OnDrawGizmosSelected()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= attackRadius)
        {
            //for attack
            Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * attackRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * attackRadius;

            if (!isInFov) Gizmos.color = Color.red; else Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (target.transform.position - transform.position).normalized * attackRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        float stunTimer = 0.5f;
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer < 0)
            {
                stunTimer = 0.5f;
                isStunned = false;
            }
            else
            {
                stunTimer -= Time.deltaTime;
            }
        }
        else
        {
            Vector3 directionBetween = (target.position - checkingObject.position).normalized;
            directionBetween.y *= 0;
            RaycastHit hit;
            if (Physics.Raycast(checkingObject.position, (target.position - checkingObject.position).normalized, out hit, maxRadius))
            {
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
                {
                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);
                    if (angle <= maxAngle)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}