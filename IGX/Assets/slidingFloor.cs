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
    }
    void FixedUpdate()
    {
        // speeeed = Mathf.Min(speeeed + acceleration * Time.fixedDeltaTime/100, maxSpeed);
        floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y, floor.transform.position.z + speeeed);
        // timer that determines when to spawn more foor:
        if (floor.transform.position.z > despawnDistance)
        {
            Destroy(floor);
            floor = GenerateMoreFloor();
        }
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
