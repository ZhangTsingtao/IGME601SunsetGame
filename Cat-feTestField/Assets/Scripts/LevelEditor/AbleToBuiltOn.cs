using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace TsingIGME601
{
    public class AbleToBuiltOn : MonoBehaviour
    {
        void Start()
        {
            Invoke("AddGrid", 1f);
        }

        void AddGrid()
        {
            transform.AddComponent<Grid>();
        }
    }

}
