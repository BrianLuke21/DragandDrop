using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform path;
    public Transform startPath;
    private float maxSteerAngle = 45f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;
    public float maxMotorTorque = 80f;
    public float maxBrakeTorque = 200f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public Collider stopSign;
    public bool isBraking = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float frontSensorPos = 0.5f;


    private List<Transform> nodes;
    private int correctNode = 0;

    // Use this for initialization
    private void Start ()
    {
        Transform[] pathATransforms =startPath.GetComponentsInChildren<Transform>();
        Transform[] mainNodes = path.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        //Path A
        for (int i = 0; i < pathATransforms.Length; i++)
        {
            if (pathATransforms[i] != startPath.transform)
            {
                nodes.Add(pathATransforms[i]);
            }
        }

        //Main Path
        for (int i = 0; i < mainNodes.Length; i++)
        {
            if (mainNodes[i] != path.transform)
            {
                nodes.Add(mainNodes[i]);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate ()
    {
        if (isBraking == true)
        {
            wheelBL.brakeTorque = maxBrakeTorque;
            wheelBR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            Sensors();
            ApplySteer();
            Drive();
            CheckWaypointDistance();
        }
	}

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos.z += frontSensorPos;

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && !isBraking)
        {
            isBraking = true;
            StartCoroutine(StopSignWait());
        }
        
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[correctNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[correctNode].position) < 0.5f)
        {
            if (correctNode == nodes.Count - 1)
            {
                correctNode = 0;
            }
            else
            {
                correctNode++;
            }

        }
    }

    IEnumerator StopSignWait()
    {
        yield return new WaitForSeconds(5);
        isBraking = false;
        wheelBL.brakeTorque = 0;
        wheelBR.brakeTorque = 0;
    }
}
