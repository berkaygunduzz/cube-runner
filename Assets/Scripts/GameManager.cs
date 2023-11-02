using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject booster;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject replayButton;
    public GameObject player;
    public int obstacleCount;
    public int boosterCount;
    public int platformLength;
    public int status = 0;

    public void ScoreUp()
    {
        if (status == 1)
        {
            score++;
            scoreText.text = score.ToString();
        }
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameStart()
    {
        player.SetActive(true);
        playButton.SetActive(false);
        if (obstacleCount > 0)
        {
            var obstacleSectionLength = platformLength / obstacleCount;
            for (int i = 1; i < obstacleCount - 1; i++)
            {
                Instantiate(obstacle,
                    new Vector3(Random.Range(-1.25f, 1.25f), 0.5f,
                        Random.Range(obstacleSectionLength * i, obstacleSectionLength * (i + 1))),
                    Quaternion.identity);
            }
        }
        if (boosterCount > 0)
        {
            var boosterSectionLength = platformLength / boosterCount;
            for (int i = 1; i < boosterCount - 1; i++)
            {
                Instantiate(booster,
                    new Vector3(Random.Range(-1.25f, 1.25f), 0.5f,
                        Random.Range(boosterSectionLength * i, boosterSectionLength * (i + 1))),
                    Quaternion.identity);
            }
        }

        // InvokeRepeating("ScoreUp", 2f, 1f);
        status = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (status == 2 || status == 3)
        {
            replayButton.SetActive(true);
        }
    }
}