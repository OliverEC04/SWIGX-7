using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject loseText;

    void Start()
    {
        InvokeRepeating("IncreaseScore", 1f, 1f);
        loseText.SetActive(false);
        scoreText.text = "Score: 0";
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void Lose()
    {
        Debug.Log("YOU LOST");
        loseText.SetActive(true);
        CancelInvoke("IncreaseScore");
        GameObject.Find("Manager").GetComponent<gameManager>().ShowButtons();
    }
}
