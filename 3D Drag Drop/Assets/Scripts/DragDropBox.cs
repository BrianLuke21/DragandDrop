using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropBox : MonoBehaviour {
        Vector3 dist;
        Vector3 startPos;
        float posX;
        float posZ;
        float posY;
        float gridScale = 2f;

        void OnMouseDown()
        {
            startPos = transform.position;
            dist = Camera.main.WorldToScreenPoint(transform.position);
            posX = Input.mousePosition.x - dist.x;
            posY = Input.mousePosition.y - dist.y;
            posZ = Input.mousePosition.z - dist.z;
        }

        void OnMouseDrag()
        {
            float disX = Input.mousePosition.x - posX;
            float disY = Input.mousePosition.y - posY;
            float disZ = Input.mousePosition.z - posZ;
            Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
            transform.position = new Vector3(Mathf.Round(lastPos.x / gridScale) * gridScale, startPos.y, Mathf.Round(lastPos.z / gridScale) * gridScale);
        }
}
