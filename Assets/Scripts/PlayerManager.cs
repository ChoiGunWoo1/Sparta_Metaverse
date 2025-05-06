using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum imagetype // �� image�� enum���� ��ȣȭ
{
    skeleton,
    knight,
    pumkin
}

public class PlayerManager : MonoBehaviour
{
    static PlayerManager instance;
    [SerializeField] private float speed = 5f; // �ӵ�
    [SerializeField] private SpriteRenderer characterRenderer; // ���� ĳ���� �̹���
    private Vector2 movementdirection = Vector2.zero; // �����̴� ����
    private Vector2 lookdirection = Vector2.zero; // ���� ����
    protected Rigidbody2D _rigidbody; // �ӵ��� �ֱ� ���� �ش� ��ü�� rigidbody
    private bool isleft = false; // ������ �����ִ��� �� ����
    private string gamezonetag = "Gamezone";
    [SerializeField] private GameObject gamestartUI;
    private Animator _animator;
    private Coroutine checkcor; // �� zone���� ������ �ڷ�ƾ�� ����
    private imagetype it = imagetype.knight;
    private string moveparameter = "Ismove"; // �� �ִϸ����� �Ķ���� �̸��� ����
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
        _rigidbody = GetComponent<Rigidbody2D>(); // player�� rigidbody�� ������
        _animator = GetComponentInChildren<Animator>(); // ���� �ڽ��� animator�� ������
        if (PlayerPrefs.HasKey(Lastpositionx) && PlayerPrefs.HasKey(Lastpositiony) && PlayerPrefs.HasKey(Lastimage))
        {
            gameObject.transform.position = new Vector2(PlayerPrefs.GetFloat(Lastpositionx), PlayerPrefs.GetFloat(Lastpositiony));
            _animator.SetInteger(imageparameter, PlayerPrefs.GetInt(Lastimage));
        }
    }

    private void Update() // �� �����Ӹ��� �̵�üũ
    {
        HandleAction();
        HandleLooking();
    }

    private void LateUpdate()
    {
        lastx = gameObject.transform.position.x;
        lasty = gameObject.transform.position.y;
    }

    public void Changeit() // �ش��ϴ� �̹����� animator �Ķ���� ���� => �ִϸ��̼��� ����Ǹ� sprite ����
    {
        _animator.SetInteger(imageparameter, (int)it);
    }
    public void NotChangeit()
    {
        it = (imagetype)_animator.GetFloat(imageparameter);
    }

    private void HandleAction() // ������
    {
        _rigidbody.velocity = movementdirection * speed;
        if (_rigidbody.velocity != Vector2.zero) // �����̰� ���� ���� �ƴҶ��� ����
        {
            _animator.SetBool(moveparameter, true);
        }
        else
        {
            _animator.SetBool(moveparameter, false);
        }
    }

    private void HandleLooking() // ���� ����
    {
        if(lookdirection.x == 0) // ���ٰ� ���⶧ ���� ���������� ���� ���������ϱ� ���� �Լ�
        {
            return;
        }

        isleft = lookdirection.x < 0; // ���¹����� x���� -�� ������ ���°���
        characterRenderer.flipX = isleft;
        
    }


    private void OnMove(InputValue inputvalue) // onmove�Լ��� ���� input�����ϰ� �ޱ�
    {
        movementdirection = inputvalue.Get<Vector2>();
        movementdirection = movementdirection.normalized; // �̵������ ���� ������ ����
        lookdirection = movementdirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(gamezonetag)) // gamezone�� ���ٸ�
        {
            checkcor = StartCoroutine(GameManager.Instance.Checkkey()); // ������ �ڷ�ƾ�� ����(�����̽� ����)
            Debug.Log("�ڷ�ƾ ����");
        }
        else // �׷��� �ʴٸ� Ʈ���Ŵ� �̹��� ��ȯ�� �浹�Ѱ���
        {
            if(collision.gameObject.CompareTag(skeleton)) // �� �̹����� �´� imagetype�� ��������
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
            checkcor = StartCoroutine(GameManager.Instance.CheckChangeImage()); // imagechange �ڷ�ƾ�� ����
            Debug.Log("�ڷ�ƾ ����");
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // �������Դٸ� �ڷ�ƾ�� �����Ͽ� ���̻� Ű������ �����ʰ��Ѵ�.
    {
         StopCoroutine(checkcor);
         Debug.Log("�ڷ�ƾ ����");
    }

}
