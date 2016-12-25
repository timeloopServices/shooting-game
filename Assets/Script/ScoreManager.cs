using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    public Text scoreText;
    int score = 0;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }

    void Awake()
    {
        instance = this;
    }
}
