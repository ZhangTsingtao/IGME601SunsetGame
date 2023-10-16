using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Sequence : NodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return false;
    }

    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }

    protected override void OnTickedAllChildren()
    {
        LastStatus = Children[^1].LastStatus;
    }
}
