using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame_UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI restartText;
    public GameObject startimage;
    public TextMeshProUGUI countdown;
    private string sstring;

    public string Sstring
    {
        get { return sstring; }
    }

    public void Start()
    {
        restartText.gameObject.SetActive(false);
        sstring = restartText.text;
    }

    public void SetRestart() // 재시작 화면 나올 시 화면에 현재 game score를 띄워줌
    {
        string curscore = "Score : "+ MiniGameManager.Instance.CurrentScore.ToString() + "\n";
        restartText.text = curscore + restartText.text;
        restartText.gameObject.SetActive(true);
    }

    /*public void SetRestartUI()
    {
        restartText.text = sstring;
    }*/

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public IEnumerator Deactivate() // 3초 뒤에 게임시 시작되면서 카운트다운 canvas 비활성화
    {
        int ct = 3;
        countdown.text = ct.ToString();
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            ct -= 1;
            if (ct == 0)
            {
                Time.timeScale = 1f;
                startimage.SetActive(false);
                yield break;
            }
            countdown.text = ct.ToString();

        }
    }

}
