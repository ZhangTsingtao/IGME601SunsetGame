using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    public enum NodeStatus
    {
        Unknown,
        InProgress,
        Failed,
        Succeeded
    }

    public NodeBase RootNode { get; private set; } = new NodeBase("Root");

    // Start is called before the first frame update
    void Start()
    {
        RootNode.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        RootNode.Tick(Time.deltaTime);
    }
}
