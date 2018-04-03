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

        spawnWait = Random.Range(spawnMinWait, spawnMaxWait);
    }

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
                    break;
                case 1:
                    CarAI.transform.root.GetComponent<CarEngine>().startPath = PathB;
                    break;
                case 2:
                    CarAI.transform.root.GetComponent<CarEngine>().startPath = PathC;
                    break;
            }
            CarAI.transform.root.GetComponent<CarEngine>().path = MainPath;

            Instantiate(CarAI.gameObject, spawnNodes[randSpawnSpot].position, spawnNodes[randSpawnSpot].localRotation);
            print("Rand Spot: " + randSpawnSpot);

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
