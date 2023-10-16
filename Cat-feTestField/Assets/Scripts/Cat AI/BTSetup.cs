using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]

public class BTSetup : MonoBehaviour
{
    //Components for wandering
    //[SerializeField] float actionComponents; 
    //[SerializeField] float wanderRange = 10f;

    protected BehaviorTree LinkedBT;
    //Cat Script 
    //protected CatAgent Agent;
    //Awareness System
    //protected AwarenessSystem Sensors;

    // Start is called before the first frame update
    void Awake()
    {
        //Agent = GetComponent<CatAgent>();
        LinkedBT = GetComponent <BehaviorTree>();
        //Sensors = GetComponent<AwarenessSystem>();

        var Root = LinkedBT.RootNode;

        var wanderRoot = Root.Add<Node_Sequence>("Wander");
        wanderRoot.Add<Node_Action>("Perform Wander",
            () =>
            {
                //Vector3 location = Agent.PickLocationInRange(wanderRange);

                //Agent.MoveTo(location);
                return BehaviorTree.NodeStatus.InProgress;
            }
            //() =>
            //{
                //return Agent.AtDestination ? BehaviorTree.NodeStatus.Succeeded : BehaviorTree.NodeStatus.InProgress;
            //}
            );
    }

}
