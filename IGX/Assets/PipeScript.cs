using UnityEngine;

public class PipeScript : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Kasse"))
        if (other.tag == "Kasse")
        {
            other.GetComponent<BirdScript>().Lose();
        }
        
    }
}
