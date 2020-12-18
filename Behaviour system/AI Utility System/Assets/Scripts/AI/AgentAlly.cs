using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentAlly : MonoBehaviour
{
    //Vraag aan Vincent/Valentijn, mag de Ally ook beslissingen maken gebaseerd op een aantal variables. PlayerHealth, PlayerEnemyDistance bijvoorbeeld.
    private NavMeshAgent agent;
    private AgentEnemy AE;
    public AIBehaviourSelector AISelector { get; private set; }
    [SerializeField]
    public BlackBoard BlackBoard { get; private set; }
    public static Vector3 targetPosition;
    private void Start()
    {

        OnInitialize();
        agent.stoppingDistance = 0.3f;
    }
    public void OnInitialize()
    {
        AISelector = GetComponent<AIBehaviourSelector>();
        BlackBoard = GetComponent<BlackBoard>();
        BlackBoard.OnInitialize();
        AISelector.OnInitialize(BlackBoard);
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FloatValue distance = BlackBoard.GetFloatVariableValue(VariableType.Distance);
        AISelector.OnUpdate();
        agent.SetDestination(targetPosition);

    }
}
