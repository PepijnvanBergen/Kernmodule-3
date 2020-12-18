using System.Linq;
using UnityEngine;
public class AIBehaviourSelector : MonoBehaviour
{
    private AIBehaviour[] behaviours;
    private AIBehaviour currentBehaviour;

    public void OnInitialize(BlackBoard bb)
    {
        behaviours = GetComponents<AIBehaviour>();
        foreach(AIBehaviour bhv in behaviours)
        {
            bhv.OnInitialize(bb);
        }
    }

    public void EvaluateBehaviours()
    {
        AIBehaviour newBehaviour = behaviours.ToList().OrderByDescending(x => x.GetNormalizedScore()).First();
        if(newBehaviour != currentBehaviour)
        {
            Debug.Log(newBehaviour.GetType().Name);
            currentBehaviour?.OnExit();
            currentBehaviour = newBehaviour;
            currentBehaviour?.OnEnter();
        }

    }

    public void OnUpdate()
    {
        EvaluateBehaviours();
        currentBehaviour?.Execute();
    }

}
