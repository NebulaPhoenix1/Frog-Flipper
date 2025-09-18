using UnityEngine;
using TMPro;

public class HighScoreChecker : MonoBehaviour
{

    TMP_Text highscoreText;
    GameObject player;

    void Start()
    {
        highscoreText = GetComponent<TMP_Text>();
        float highscore = Mathf.Round(getHighScore());
        highscoreText.text = "Highscore: " + highscore.ToString() + "m";
        player = GameObject.Find("Player");
    }

    public float getHighScore()
    {
        return PlayerPrefs.GetFloat("Highscore", 0.0f);
    }

    public void updateHighScore()
    {
        float highscore = Mathf.Round(getHighScore());
        //Debug.Log(highscore);
        float currentScore = Mathf.Round(player.transform.position.y);
        if (currentScore > highscore)
        {
            PlayerPrefs.SetFloat("Highscore", currentScore);
            highscoreText.text = "Highscore: " + currentScore.ToString() + "m";
            
        }
      
    }
}
