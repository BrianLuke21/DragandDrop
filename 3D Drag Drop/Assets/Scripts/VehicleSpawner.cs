using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public Transform PathA;
    public Transform PathB;
    public Transform PathC;
    public Transform MainPath;

    //public Transform[] spawnPos;
    private Transform[] spawnPos;
    private List<Transform> spawnNodes = new List<Transform>();

    //Tutorial code
    public GameObject CarAI;
    public float spawnWait = 1;
    public float spawnMaxWait = 10;
    public float spawnMinWait = 2;
    public int startWait = 1;
    private int randSpawnSpot;
    private int spawnYRot;


    // Use this for initialization
    void Start()
    {
        spawnPos = GetComponentsInChildren<Transform>();
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawnPos.Length; i++)
        {
            if (spawnPos[i] != transform)
            {
                spawnNodes.Add(spawnPos[i]);
            }
        }

        /*if (transform.position.y <= -10)
        {
            ResetPostion();
        }*/

        spawnWait = Random.Range(spawnMinWait, spawnMaxWait);
    }

    /*private void ResetPostion()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);

        correctNode = 0;

        if (startPath.name == "Path_A")
        {
            transform.position = spawnPos.position;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        if (startPath.name == "Path_B")
        {
            transform.position = spawnPos.position;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }

        if (startPath.name == "Path_C")
        {
            transform.position = spawnPos.position;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }*/

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            randSpawnSpot = Random.Range(0, 3);

            switch (randSpawnSpot)
            {
                case 0:
                    CarAI.transform.root.GetComponent<CarEngine>().startPath = PathA;
                    spawnYRot = 0;
                    break;
                case 1:
                    CarAI.transform.root.GetComponent<CarEngine>().startPath = PathB;
                    spawnYRot = 90;
                    break;
                case 2:
                    CarAI.transform.root.GetComponent<CarEngine>().startPath = PathC;
                    spawnYRot = 180;
                    break;
            }
            CarAI.transform.root.GetComponent<CarEngine>().path = MainPath;

            Instantiate(CarAI.gameObject, spawnNodes[randSpawnSpot].position, Quaternion.Euler(0, spawnYRot, 0));

            //print("Rand Spot: " + spawnNodes[randSpawnSpot]);
            //print("Local Rot: " + spawnNodes[randSpawnSpot].localRotation);
            //print("Rot: " + spawnNodes[randSpawnSpot].rotation);

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
