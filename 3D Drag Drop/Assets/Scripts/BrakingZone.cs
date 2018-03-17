using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakingZone : MonoBehaviour {

    private float maxBrakeTorque;
    private float minCarSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AI Car")
        {
            other.transform.root.GetComponent<CarEngine>().isBraking = true;
        }
    }
}
