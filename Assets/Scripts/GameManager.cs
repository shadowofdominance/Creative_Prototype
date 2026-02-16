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
    public GameObject heartPrefab;
    public Transform heartsContainer;

    private float xRange = 10;
    private float yPos = 4;
    private float startDelay = 1f;
    private int score;
    private float spawnRate;
    private int lives = 3;
    private List<GameObject> heartsList = new List<GameObject>();
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

    public void UpdateLives(int livesToRemove)
    {
        lives -= livesToRemove;

        if (heartsList.Count > 0)
        {
            Destroy(heartsList[heartsList.Count - 1]);
            heartsList.RemoveAt(heartsList.Count - 1);
        }

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void InitializeHearts()
    {
        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            heartsList.Add(heart);
        }
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
        lives = 3;
        UpdateScore(0);

        InitializeHearts();

        titleScreen.gameObject.SetActive(false);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}