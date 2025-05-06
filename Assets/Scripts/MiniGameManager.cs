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
        StartCoroutine(miniGame_UIManager.Deactivate()); // ó���� timescale�� 0���θ���� 3�ʵ� �����ϴ� �Լ� ����

    }

    public void GameOver() // ������ ����ɶ� �ְ�������� ���Ž�Ű�� �ƴ϶�� ���Ž�Ű�� ����
    {
        if (PlayerPrefs.HasKey(maxscore))
        {
            if(PlayerPrefs.GetInt(maxscore) < currentScore)
            {
                PlayerPrefs.SetInt(maxscore, currentScore);
                Debug.Log("�ְ����� ����!");
            }
            else
            {
                Debug.Log("���� ����..");
            }
        }
        else
        {
            PlayerPrefs.SetInt(maxscore, currentScore);
            Debug.Log("ùƮ");
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
