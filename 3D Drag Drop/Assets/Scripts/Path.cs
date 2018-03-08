using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color lineColor;
    public GameObject mainPath;

    private List<Transform> nodes = new List<Transform>();

    //OnDrawGizmosSelected if needed
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] pathATransforms = GetComponentsInChildren<Transform>();
        Transform[] mainNodes = mainPath.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();
        
        //Path A
        for (int i = 0; i < pathATransforms.Length; i++)
        {
            if (pathATransforms[i] != transform)
            {
                nodes.Add(pathATransforms[i]);
            }
        }

        //Main Path
        for (int i = 0; i < mainNodes.Length; i++)
        {
            if (mainNodes[i] != mainPath.transform)
            {
                nodes.Add(mainNodes[i]);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;
            if (i > 0)
            {
                previousNode = nodes[i - 1].position;
            }
            else if(i == 0 && nodes.Count > 1)
            {
                previousNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
}
