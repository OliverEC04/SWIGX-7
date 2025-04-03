using Unity.VisualScripting;
using UnityEngine;

public class moveWithCam : MonoBehaviour
{
    public GameObject readeren;
    public StandaloneEasyReaderSample scanner;
    public float distance = 0.0f;
    void Start()
    {
        scanner = readeren.GetComponent<StandaloneEasyReaderSample>();
    }

    void Update()
    {
        // transform.position = new Vector3(scanner.qrTrans.position.x, scanner.qrTrans.position.y, (scanner.distanceFromCam / 10));
    }
}
