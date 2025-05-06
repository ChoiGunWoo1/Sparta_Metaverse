using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string minigamezone = "MiniGameScene";
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject imagechangecanvas;
    [SerializeField] private GameObject QuitCanvas;
    [SerializeField] TextMeshPro highestscore;
    private static GameManager instance;
    string highscorebasicstring = "Highest Score : ";
    string maxscore = "MaxScore";
    string Lastpositionx = "LastpositionX";
    string Lastpositiony = "LastpositionY";
    string Lastimage = "LastImage";
    private Coroutine quitcoroutine;

    public static GameManager Instance {
        get {return instance;}
     }
    private void Start()
    {
        instance = this;
        if(PlayerPrefs.HasKey(maxscore)) // 만약 playerprefs에 최고 점수가 갱신되었다면 그 갱신된 점수를 highscore에 입력한다
        {
            highestscore.text = highscorebasicstring + PlayerPrefs.GetInt(maxscore).ToString();
        }
        else
        {
            highestscore.text += highscorebasicstring + "---";
        }
        quitcoroutine = StartCoroutine(CheckQuit()); // 게임이 시작되면 esc키 감지 시작
    }
    public void EnterGameZone() // 미니게임을 시작하는 함수
    {
        PlayerPrefs.SetFloat(Lastpositionx, PlayerManager.Instance.Lastx);
        PlayerPrefs.SetFloat(Lastpositiony, PlayerManager.Instance.Lasty);
        PlayerPrefs.SetInt(Lastimage, (int)PlayerManager.Instance.It);
        SceneManager.LoadScene(minigamezone);
    }

    public void QuitGame() // 게임을 시작하지 않고 그만두는 함수
    {
        canvas.SetActive(false);
        quitcoroutine = StartCoroutine(CheckQuit());
    }

    public void ChangeImage() // trigger한 이미지로 바꾸는 함수
    {
        PlayerManager.Instance.Changeit();
        imagechangecanvas.SetActive(false);
        quitcoroutine = StartCoroutine(CheckQuit());
    }

    public void CancelChangeImage() // 이미지를 바꾸지않고 취소하는 함수
    {
        PlayerManager.Instance.NotChangeit();
        imagechangecanvas.SetActive(false);
        quitcoroutine = StartCoroutine(CheckQuit());
    }

    public void ExitGame() // 게임을 종료하는 함수
    {
        PlayerPrefs.SetFloat(Lastpositionx, PlayerManager.Instance.Lastx); // 마지막 위치와 이미지를 저장
        PlayerPrefs.SetFloat(Lastpositiony, PlayerManager.Instance.Lasty);
        PlayerPrefs.SetInt(Lastimage, (int)PlayerManager.Instance.It);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void CancelExitGame() // 게임 종료를 취소하는 함수
    {
        QuitCanvas.SetActive(false);
    }

    public IEnumerator Checkkey() // 미니게임존에서의 space체크
    {
        while(true)
        {
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }
            canvas.SetActive(true);
            StopCoroutine(quitcoroutine);
            Debug.Log("스페이스 감지");
            yield return null;
        }

    }

    public IEnumerator CheckChangeImage() // imagechage존에서 space체크
    {
        while(true)
        {
            while(!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }
            imagechangecanvas.SetActive(true);
            StopCoroutine(quitcoroutine);
            yield return null;

        }
    }
    public IEnumerator CheckQuit() // 게임 종료 esc를 누르는지 체크
    {
        while(true)
        {
            while(!Input.GetKeyDown(KeyCode.Escape))
            {
                yield return null;
            }
            QuitCanvas.SetActive(true);
            yield return null;
        }

    }
}
