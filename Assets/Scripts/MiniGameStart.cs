using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class MiniGameStart : MonoBehaviour
{

    public TextMeshProUGUI countdown;
    // Start is called before the first frame update
    

    IEnumerator Deactivate()
    {
        int ct = 3;
        countdown.text = ct.ToString();
        while(true)
        {
            yield return new WaitForSecondsRealtime(1f);
            ct -= 1;
            if(ct == 0)
            {
                Time.timeScale = 1f;
                gameObject.SetActive(false);
                yield break;
            }
            countdown.text = ct.ToString();

        }
    }
}
