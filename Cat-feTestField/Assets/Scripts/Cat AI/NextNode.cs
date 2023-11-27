using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoosaIGM601
{
public class NextNode
    {
        public bool openPath;
        public Vector3 worldPosition;

        public int gCost;
        public int hCost;

        public int gridX;
        public int gridY;
        //private bool jumpNode;

        public NextNode parent;

        public NextNode(bool _openPath, Vector3 _worldPos, int _gridX, int _gridY)
        {
            openPath = _openPath;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
            //jumpNode = _jumpNode;
        }

        public int fCost{
            get{
                return gCost + hCost;
            }
        }
    }
}

