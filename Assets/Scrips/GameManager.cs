using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    public void UpdateScore(int scoreCounter)
    {
        score += scoreCounter;
        scoreText.text = "Score: " + score;
    }
    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
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
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
