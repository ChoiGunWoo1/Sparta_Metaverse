using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject player; // player ������Ʈ
    [SerializeField] private float cameraSpeed = 0.3f;
    [SerializeField] private float maxx = 5.2f;
    [SerializeField] private float maxy = 6.2f;
    [SerializeField] private float minx = -7.4f;
    [SerializeField] private float miny = -4.0f;
    Vector3 cameradepth = new Vector3(0, 0, -10); // ī�޶� ����
    private void LateUpdate()
    {
        Vector3 targetposition = player.transform.position;
        targetposition.x = Mathf.Clamp(targetposition.x, minx, maxx); //�� x���� y���� �ּ� �ִ븦 �������ص�
        targetposition.y = Mathf.Clamp(targetposition.y, miny, maxy);
        transform.position = Vector3.Lerp(transform.position, targetposition + cameradepth, cameraSpeed);
        //Lerp�Լ��� �̿��� �ε巴�� ���� player�� ��ġ�� ī�޶� �̵�
    }
}
