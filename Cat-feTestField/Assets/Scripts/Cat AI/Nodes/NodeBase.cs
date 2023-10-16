using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBase
{
    public string Name { get; protected set; }

    protected List<NodeBase> Children = new List<NodeBase>();

    protected System.Func<BehaviorTree.NodeStatus> OnEnterFunction;
    protected System.Func<BehaviorTree.NodeStatus> OnTickFunction;

    public BehaviorTree.NodeStatus LastStatus { get; protected set; } = BehaviorTree.NodeStatus.Unknown;

    public NodeBase(string _Name = "", 
        System.Func<BehaviorTree.NodeStatus> _OnEnterFunction = null,
        System.Func<BehaviorTree.NodeStatus> _OnTickFunction = null)
    {
        Name = _Name;
        OnEnterFunction = _OnEnterFunction;
        OnTickFunction = _OnTickFunction;
    }

    public NodeBase Add<T>(string _Name,
        System.Func<BehaviorTree.NodeStatus> _OnEnter = null,
        System.Func<BehaviorTree.NodeStatus> _OnTick = null) where T : NodeBase, new()
    {
        T newNode = new T();
        newNode.Name = _Name;
        newNode.OnEnterFunction = _OnEnter;
        newNode.OnTickFunction = _OnTick;
        Children.Add(newNode);

        return Add(newNode);
    }

    public NodeBase Add<T>(T newNode) where T : NodeBase
    {
        Children.Add(newNode);

        return newNode;
    }

    public virtual void Reset()
    {
        LastStatus = BehaviorTree.NodeStatus.Unknown;

        foreach(var child in Children)
        {
            child.Reset();
        }
    }

    public void Tick(float deltaTime)
    {
        bool tickedAnyNodes = OnTick(deltaTime);

        //If no actions happened, reset
        if (!tickedAnyNodes)
            Reset();
    }

    protected virtual void OnEnter()
    {
        if (OnEnterFunction != null)
        {
            LastStatus = OnEnterFunction.Invoke();
        }
        else
        {
            LastStatus = Children.Count > 0 ? BehaviorTree.NodeStatus.InProgress : BehaviorTree.NodeStatus.Succeeded;
        }
    }

    protected virtual bool OnTick(float deltaTime)
    {
        bool tickedAnyNodes = false;

        if(LastStatus == BehaviorTree.NodeStatus.Unknown)
        {
            OnEnter();
            tickedAnyNodes = true;
        }

        if(OnTickFunction != null)
        {
            LastStatus = OnTickFunction.Invoke();
            tickedAnyNodes = true;

            if(LastStatus != BehaviorTree.NodeStatus.InProgress)
            {
                return tickedAnyNodes;
            }
        }

        if(Children.Count == 0)
        {
            if(OnTickFunction == null)
            {
                LastStatus = BehaviorTree.NodeStatus.Succeeded;
            }

            return tickedAnyNodes;
        }

        //Run ticks on any children
        foreach(var child in Children)
        {
            if(child.LastStatus == BehaviorTree.NodeStatus.InProgress)
            {
                tickedAnyNodes |= child.OnTick(deltaTime);
                return tickedAnyNodes;
            }

            if(child.LastStatus != BehaviorTree.NodeStatus.Unknown)
                continue;

            tickedAnyNodes |= child.OnTick(deltaTime);

            //Inherit child status by default
            LastStatus = child.LastStatus;

            if(child.LastStatus == BehaviorTree.NodeStatus.InProgress)
            {
                return tickedAnyNodes;
            }
            else if(child.LastStatus == BehaviorTree.NodeStatus.Failed &&
                !ContinueEvaluatingIfChildFailed())
            {
                return tickedAnyNodes;
            }

            else if(child.LastStatus == BehaviorTree.NodeStatus.Succeeded &&
                !ContinueEvaluatingIfChildSucceeded())
            {
                return tickedAnyNodes;
            }
        }

        OnTickedAllChildren();

        return tickedAnyNodes;
    }

    //Helped to determine where it should move next in behavior tree (overwritten in Node_Sequence and Node_Selector)
    protected virtual bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }

    protected virtual bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }

    protected virtual void OnTickedAllChildren()
    {

    }
}
