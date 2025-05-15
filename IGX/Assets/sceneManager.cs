using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{
    public void LoadScene(int sceneIndex = 1) // vi loader game scene by default
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Study()
    {
        Application.Quit();
    }
}
