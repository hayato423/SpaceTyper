using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int score;
    private Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "SCORE:" + score;
    }

    public void AddScore(int point)
    {
        score += point;
    }

    public int GetScore()
    {
        return score;
    }
}
