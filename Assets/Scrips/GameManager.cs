using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;

    private float xRange = 10;
    private float yPos = 4;
    private float startDelay = 1f;
    private int score;
    private float spawnRate;
    bool isGameActive;

    IEnumerator RandomObjectSpawner()
    {
        yield return new WaitForSeconds(startDelay);

        while (isGameActive)
        {
            int randomIndex = Random.Range(0, objectPrefabs.Length);

            Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), yPos, 0);

            Instantiate(objectPrefabs[randomIndex], spawnPos, objectPrefabs[randomIndex].transform.rotation);

            yield return new WaitForSeconds(spawnRate);
        }
    }

    void UpdateScore(int scoreCounter)
    {
        score += scoreCounter;
        scoreText.text = "Score: " + score;
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }
    public void StartGame(int difficulty)
    {
        if (isGameActive)
        {
            return;
        }

        isGameActive = true;
        spawnRate = 1.5f / difficulty;
        StartCoroutine(RandomObjectSpawner());

        score = 0;
        UpdateScore(0);

        titleScreen.gameObject.SetActive(false);
    }
}
