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

    public float maxMotorTorque = 85;
    public float maxBrakeTorque = 200f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    //public Collider stopSign;
    public bool isBraking = false;

    [Header("Spawn Points")]
    public Transform spawnPos;

    private float sensorLength = 8f;
    //private Vector3 frontSensorPos = new Vector3(-0.5f, 0.5f, 0f);
    //private float frontSideSensorPos = 0.5f;


    private List<Transform> nodes;
    private int correctNode = 0;


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


    private void FixedUpdate ()
    {
        Sensors();

        if (isBraking == true)
        {
            Braking();

            if (Mathf.Round(wheelFR.rpm) == 0)
            {
                //print("stopped");
                StartCoroutine(StopSignWait());
                isBraking = false;
                //print("start driving");
            }
        }
        else
        {
            //print(wheelBL.brakeTorque);
            ApplySteer();
            Drive();
            CheckWaypointDistance();
        }
	}

    private void Braking()
    {
        if (isBraking == true)
        {
            wheelBL.brakeTorque = maxBrakeTorque;
            wheelBR.brakeTorque = maxBrakeTorque;
            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelBL.brakeTorque = 0;
            wheelBR.brakeTorque = 0;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
        }
    }

    private void Sensors()
    {
        RaycastHit hit;
        Transform sensorLeftPos = GetComponentInChildren<Transform>().transform.Find("FrontLeftSensor");
        Transform sensorMidPos = GetComponentInChildren<Transform>().transform.Find("FrontMidSensor");
        Transform sensorRightPos = GetComponentInChildren<Transform>().transform.Find("FrontRightSensor");

        //Front Center Sensor
        if (Physics.Raycast(sensorMidPos.position, transform.forward, out hit, sensorLength) && !isBraking)
            {
                Debug.DrawLine(sensorMidPos.position, hit.point, Color.yellow);
                if (hit.transform.tag == "BrakingZone")
                {
                    //print("hit");
                }
                else
                {
                    isBraking = true;
                }
            }

            //Front Right Sensor
            //sensorLeftPos.x += frontSideSensorPos;
            if (Physics.Raycast(sensorRightPos.position, transform.forward, out hit, sensorLength) && !isBraking)
            {
                Debug.DrawLine(sensorRightPos.position, hit.point, Color.red);
                //isBraking = true;
            }

            //Front Left Sensor
            //sensorStartPos.x -= 2 * frontSideSensorPos;
            if (Physics.Raycast(sensorLeftPos.position, transform.forward, out hit, sensorLength) && !isBraking)
            {
                Debug.DrawLine(sensorLeftPos.position, hit.point, Color.green);
                //isBraking = true;
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
        currentSpeed = Mathf.Round( 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000);

        if (currentSpeed < maxSpeed && !isBraking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
    }

    private void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[correctNode].position) < 0.5f)
        {
            if (correctNode == nodes.Count - 1)
            {
                isBraking = true;
            }
            else
            {
                correctNode++;
            }

        }
    }

    IEnumerator StopSignWait()
    {
        yield return new WaitForSeconds(3);
        Braking();
    }
}
