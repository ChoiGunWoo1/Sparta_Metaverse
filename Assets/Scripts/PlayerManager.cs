using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum imagetype // 각 image를 enum으로 번호화
{
    skeleton,
    knight,
    pumkin
}

public class PlayerManager : MonoBehaviour
{
    static PlayerManager instance;
    [SerializeField] private float speed = 5f; // 속도
    [SerializeField] private SpriteRenderer characterRenderer; // 메인 캐릭터 이미지
    private Vector2 movementdirection = Vector2.zero; // 움직이는 방향
    private Vector2 lookdirection = Vector2.zero; // 보는 방향
    protected Rigidbody2D _rigidbody; // 속도를 넣기 위한 해당 개체의 rigidbody
    private bool isleft = false; // 왼쪽을 보고있는지 값 여부
    private string gamezonetag = "Gamezone";
    [SerializeField] private GameObject gamestartUI;
    private Animator _animator;
    private Coroutine checkcor; // 각 zone에서 실행할 코루틴을 저장
    private imagetype it = imagetype.knight;
    private string moveparameter = "Ismove"; // 각 애니메이터 파라미터 이름을 저장
    private string imageparameter = "Player_image";
    private string skeleton = "Skeleton";
    private string knight = "Knight";
    private string pumkin = "Pumkin";
    private float lastx;
    private float lasty;
    string Lastpositionx = "LastpositionX";
    string Lastpositiony = "LastpositionY";
    string Lastimage = "LastImage";

    public static PlayerManager Instance
    {
        get { return instance; }
    }

    public float Lastx
    {
        get { return lastx; }
    }
    public float Lasty
    {
        get { return lasty; }
    }
    public imagetype It
    {
        get { return it; }
    }
    private void Start()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody2D>(); // player의 rigidbody를 가져옴
        _animator = GetComponentInChildren<Animator>(); // 하위 자식의 animator를 가져옴
        if (PlayerPrefs.HasKey(Lastpositionx) && PlayerPrefs.HasKey(Lastpositiony) && PlayerPrefs.HasKey(Lastimage))
        {
            gameObject.transform.position = new Vector2(PlayerPrefs.GetFloat(Lastpositionx), PlayerPrefs.GetFloat(Lastpositiony));
            _animator.SetInteger(imageparameter, PlayerPrefs.GetInt(Lastimage));
        }
    }

    private void Update() // 매 프레임마다 이동체크
    {
        HandleAction();
        HandleLooking();
    }

    private void LateUpdate()
    {
        lastx = gameObject.transform.position.x;
        lasty = gameObject.transform.position.y;
    }

    public void Changeit() // 해당하는 이미지의 animator 파라미터 변경 => 애니메이션이 변경되며 sprite 변경
    {
        _animator.SetInteger(imageparameter, (int)it);
    }
    public void NotChangeit()
    {
        it = (imagetype)_animator.GetFloat(imageparameter);
    }

    private void HandleAction() // 움직임
    {
        _rigidbody.velocity = movementdirection * speed;
        if (_rigidbody.velocity != Vector2.zero) // 움직이고 있을 때와 아닐때를 구분
        {
            _animator.SetBool(moveparameter, true);
        }
        else
        {
            _animator.SetBool(moveparameter, false);
        }
    }

    private void HandleLooking() // 보는 방향
    {
        if(lookdirection.x == 0) // 가다가 멈출때 가던 방향으로의 보는 방향유지하기 위한 함수
        {
            return;
        }

        isleft = lookdirection.x < 0; // 보는방향의 x축이 -면 왼쪽을 보는것임
        characterRenderer.flipX = isleft;
        
    }


    private void OnMove(InputValue inputvalue) // onmove함수를 통한 input간편하게 받기
    {
        movementdirection = inputvalue.Get<Vector2>();
        movementdirection = movementdirection.normalized; // 이동방향과 보는 방향을 변경
        lookdirection = movementdirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(gamezonetag)) // gamezone에 들어갔다면
        {
            checkcor = StartCoroutine(GameManager.Instance.Checkkey()); // 게임존 코루틴을 시작(스페이스 감지)
            Debug.Log("코루틴 시작");
        }
        else // 그렇지 않다면 트리거는 이미지 변환과 충돌한것임
        {
            if(collision.gameObject.CompareTag(skeleton)) // 각 이미지에 맞는 imagetype을 가져오고
            {
                it = imagetype.skeleton;
            }
            else if(collision.gameObject.CompareTag(knight))
            {
                it = imagetype.knight;
            }
            else
            {
                it = imagetype.pumkin;
            }
            checkcor = StartCoroutine(GameManager.Instance.CheckChangeImage()); // imagechange 코루틴을 시작
            Debug.Log("코루틴 시작");
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // 빠져나왔다면 코루틴을 종료하여 더이상 키감지를 하지않게한다.
    {
         StopCoroutine(checkcor);
         Debug.Log("코루틴 종료");
    }

}
