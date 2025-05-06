using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject player; // player 오브젝트
    [SerializeField] private float cameraSpeed = 0.3f;
    [SerializeField] private float maxx = 5.2f;
    [SerializeField] private float maxy = 6.2f;
    [SerializeField] private float minx = -7.4f;
    [SerializeField] private float miny = -4.0f;
    Vector3 cameradepth = new Vector3(0, 0, -10); // 카메라 깊이
    private void LateUpdate()
    {
        Vector3 targetposition = player.transform.position;
        targetposition.x = Mathf.Clamp(targetposition.x, minx, maxx); //각 x값과 y값의 최소 최대를 지정해준뒤
        targetposition.y = Mathf.Clamp(targetposition.y, miny, maxy);
        transform.position = Vector3.Lerp(transform.position, targetposition + cameradepth, cameraSpeed);
        //Lerp함수를 이용해 부드럽게 현재 player의 위치로 카메라 이동
    }
}
