using System.ComponentModel;
using UnityEngine;

public class SlidingFloor : MonoBehaviour
{

    public float speeeed = 0.5f;
    public float acceleration = 0.25f;
    public float maxSpeed = 4.0f;
    public float despawnDistance = 50f;
    public GameObject floor;
    void Start()
    {
        floor = GenerateMoreFloor();
        SpawnPipes();
    }
    void FixedUpdate()
    {
        // speeeed = Mathf.Min(speeeed + acceleration * Time.fixedDeltaTime/100, maxSpeed);
        floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y, floor.transform.position.z + speeeed);
        if (floor.transform.position.z > despawnDistance)
        {
            Destroy(floor);
            floor = GenerateMoreFloor();
            SpawnPipes();

        }
    }
    public GameObject pipePrefab;
    public GameObject[] topPipeSpawnpoints;
    public GameObject[] bottomPipeSpawnpoints;
    public Vector3 bottomPipeOffset = new Vector3(0, 0, 10f);
    public void SpawnPipes() 
    {
        topPipeSpawnpoints = GameObject.FindGameObjectsWithTag("TopPipe");
        bottomPipeSpawnpoints = GameObject.FindGameObjectsWithTag("BottomPipe");
        for (int i = 0; i < topPipeSpawnpoints.Length; i++)
        {
            int spawnChance = Random.Range(0, 2);
            if (spawnChance == 0)
            {
                Instantiate(pipePrefab, topPipeSpawnpoints[i].transform.position, Quaternion.Euler(180, 0, 0)).transform.parent = floor.transform;
            }
        }
        for (int i = 0; i < bottomPipeSpawnpoints.Length; i++)
        {
            int spawnChance = Random.Range(0, 2);
            if (spawnChance == 0)
            {
                Instantiate(pipePrefab, bottomPipeSpawnpoints[i].transform.position + bottomPipeOffset, Quaternion.identity).transform.parent = floor.transform;
            }
        }

        // int randomTopIndex = Random.Range(0, topPipeSpawnpoints.Length);
        // int randomBottomIndex = Random.Range(0, bottomPipeSpawnpoints.Length);
        // Instantiate(pipePrefab, topPipeSpawnpoints[randomTopIndex].transform.position, Quaternion.Euler(180, 0, 0)).transform.parent = floor.transform;
        // Instantiate(pipePrefab, bottomPipeSpawnpoints[randomBottomIndex].transform.position, Quaternion.identity).transform.parent = floor.transform;
    }

    public GameObject floorPrefab;
    public GameObject GenerateMoreFloor(){
        // GameObject newFloor = Instantiate(gameObject);
        GameObject newFloor = Instantiate(floorPrefab);
        newFloor.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        newFloor.transform.localScale = transform.localScale;
        // newFloor.GetComponent<slidingFloor>().speeeed = speeeed;
        GetComponent<SlidingFloor>().speeeed = speeeed + acceleration; // add acceleration her
        return newFloor;
        // Destroy(floor, 10);
    }
}
