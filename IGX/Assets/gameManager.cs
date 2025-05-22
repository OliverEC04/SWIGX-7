using UnityEngine;

public class gameManager : MonoBehaviour
{
    GameObject restartButton;
    GameObject menuButton;
    void Start()
    {
        restartButton = GameObject.Find("RestartButton");
        menuButton = GameObject.Find("MenuButton");
        Debug.Log("RestartButton position " + restartButton.transform.position); // 377, 131
        Debug.Log("MenuButton position " + menuButton.transform.position); // 377, 91
        restartButton.transform.position = new Vector3(3000, 3000, 300);
        menuButton.transform.position = new Vector3(3000, 3000, 300);

    }

    public void ShowButtons()
    {
        restartButton.transform.position = new Vector3(377, 131, 0);
        menuButton.transform.position = new Vector3(377, 91, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
