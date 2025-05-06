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
        if(PlayerPrefs.HasKey(maxscore)) // ���� playerprefs�� �ְ� ������ ���ŵǾ��ٸ� �� ���ŵ� ������ highscore�� �Է��Ѵ�
        {
            highestscore.text = highscorebasicstring + PlayerPrefs.GetInt(maxscore).ToString();
        }
        else
        {
            highestscore.text += highscorebasicstring + "---";
        }
        quitcoroutine = StartCoroutine(CheckQuit()); // ������ ���۵Ǹ� escŰ ���� ����
    }
    public void EnterGameZone() // �̴ϰ����� �����ϴ� �Լ�
    {
        PlayerPrefs.SetFloat(Lastpositionx, PlayerManager.Instance.Lastx);
        PlayerPrefs.SetFloat(Lastpositiony, PlayerManager.Instance.Lasty);
        PlayerPrefs.SetInt(Lastimage, (int)PlayerManager.Instance.It);
        SceneManager.LoadScene(minigamezone);
    }

    public void QuitGame() // ������ �������� �ʰ� �׸��δ� �Լ�
    {
        canvas.SetActive(false);
        quitcoroutine = StartCoroutine(CheckQuit());
    }

    public void ChangeImage() // trigger�� �̹����� �ٲٴ� �Լ�
    {
        PlayerManager.Instance.Changeit();
        imagechangecanvas.SetActive(false);
        quitcoroutine = StartCoroutine(CheckQuit());
    }

    public void CancelChangeImage() // �̹����� �ٲ����ʰ� ����ϴ� �Լ�
    {
        PlayerManager.Instance.NotChangeit();
        imagechangecanvas.SetActive(false);
        quitcoroutine = StartCoroutine(CheckQuit());
    }

    public void ExitGame() // ������ �����ϴ� �Լ�
    {
        PlayerPrefs.SetFloat(Lastpositionx, PlayerManager.Instance.Lastx); // ������ ��ġ�� �̹����� ����
        PlayerPrefs.SetFloat(Lastpositiony, PlayerManager.Instance.Lasty);
        PlayerPrefs.SetInt(Lastimage, (int)PlayerManager.Instance.It);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
    public void CancelExitGame() // ���� ���Ḧ ����ϴ� �Լ�
    {
        QuitCanvas.SetActive(false);
    }

    public IEnumerator Checkkey() // �̴ϰ����������� spaceüũ
    {
        while(true)
        {
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }
            canvas.SetActive(true);
            StopCoroutine(quitcoroutine);
            Debug.Log("�����̽� ����");
            yield return null;
        }

    }

    public IEnumerator CheckChangeImage() // imagechage������ spaceüũ
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
    public IEnumerator CheckQuit() // ���� ���� esc�� �������� üũ
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
