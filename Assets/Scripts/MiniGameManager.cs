using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    static MiniGameManager miniGameManager;
    MiniGame_UIManager miniGame_UIManager;
    string maxscore = "MaxScore";
    public static MiniGameManager Instance
    {
        get { return miniGameManager; }
    }

    public MiniGame_UIManager MiniGame_UIManager
    {
        get { return MiniGame_UIManager; }
    }

    private int currentScore = 0;

    public int CurrentScore
    {
        get { return currentScore; }
    }
    private string mainscene = "MainScene";

    private void Awake()
    {
        miniGameManager = this;
        miniGame_UIManager = FindObjectOfType<MiniGame_UIManager>();  
    }

    private void Start()
    {
        miniGame_UIManager.UpdateScore(0);
        Time.timeScale = 0;
        StartCoroutine(miniGame_UIManager.Deactivate()); // 처음에 timescale을 0으로만들고 3초뒤 시작하는 함수 실행

    }

    public void GameOver() // 게임이 종료될때 최고점수라면 갱신시키고 아니라면 갱신시키지 않음
    {
        if (PlayerPrefs.HasKey(maxscore))
        {
            if(PlayerPrefs.GetInt(maxscore) < currentScore)
            {
                PlayerPrefs.SetInt(maxscore, currentScore);
                Debug.Log("최고점수 갱신!");
            }
            else
            {
                Debug.Log("갱신 실패..");
            }
        }
        else
        {
            PlayerPrefs.SetInt(maxscore, currentScore);
            Debug.Log("첫트");
        }
        miniGame_UIManager.SetRestart();
    }

    public void RestartGame()
    {
        //miniGame_UIManager.SetRestartUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        miniGame_UIManager.UpdateScore(currentScore);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainscene);
    }
}
