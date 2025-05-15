using UnityEngine;

public class zoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kasse")
        {
            Debug.Log("Kassen ramte");
        }
    }

    void OnTriggerExit(Collider other) {
        Debug.Log("SES");
    }
}
