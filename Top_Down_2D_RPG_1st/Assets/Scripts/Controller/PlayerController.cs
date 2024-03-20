using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public Grid _grid;
    public float _speed = 5.0f;

    //Player Cell Position
    private Animator _animator;
    private Rigidbody2D _rigid;
    private Vector3Int _cellPos = Vector3Int.zero;
    public Vector3Int CellPos
    {
        get { return _cellPos; }
        set { _cellPos = value; }
    }
    private bool _isMoving = false;
    private Vector3 dirVec;
    private GameObject scanObject;

    private MoveDir _dir = MoveDir.None;
    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;

            switch (value)
            {
                case MoveDir.None:
                    if (_dir == MoveDir.Up)
                        _animator.Play("Player_Up_Idle");
                    else if (_dir == MoveDir.Down)
                        _animator.Play("Player_Down_Idle");
                    else if (_dir == MoveDir.Left)
                        _animator.Play("Player_Left_Idle");
                    else
                        _animator.Play("Player_Right_Idle");
                    break;
                case MoveDir.Up:
                    _animator.Play("Player_Up_Walk");
                    dirVec = Vector3.up;
                    break;
                case MoveDir.Down:
                    _animator.Play("Player_Down_Walk");
                    dirVec = Vector3.down;
                    break;
                case MoveDir.Left:
                    _animator.Play("Player_Left_Walk");
                    dirVec = Vector3.left;
                    break;
                case MoveDir.Right:
                    _animator.Play("Player_Right_Walk");
                    dirVec = Vector3.right;
                    break;
                default:
                    break;
            }

            _dir = value;
        }
    }

    int right_Value;
    int left_Value;
    int down_Value;
    int up_Value;
    
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
            transform.position = pos;
        }
    }

    private void Start()
    {
        // ���߿� ���� �ܾ �� ��ġ �ٲ��ֱ�.
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GetDirInput();
        UpdatePosition();
        UpdateIsMoving();
        SearchAction();
        GetInputKey();
    }

    // Ű���� �Է�
    private void GetInputKey()
    {
        //Scan Object
        if (Input.GetButtonDown("Jump") && scanObject != null)
            gameManager.Action(scanObject);
    }

    private void GetDirInput()
    {
        if (gameManager.isAction == true)
            return;

        if (Input.GetKey(KeyCode.UpArrow) || (up_Value + down_Value == 1))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || (up_Value + down_Value == -1))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || (left_Value + right_Value == -1))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || (left_Value + right_Value == 1))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }
    }

    // ������ �̵��ϴ� ���� ó��
    private void UpdatePosition()
    {
        if (_isMoving == false)
            return;

        Vector3 destPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveDir = destPos - transform.position;

        // ���� ���� üũ
        float dist = moveDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            _isMoving = false;
        }
        else
        {
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            _isMoving = true;
        }
    }

    // �̵� ������ ������ ��, ���� ��ǥ�� �̵��Ѵ�.
    private void UpdateIsMoving()
    {
        if (_isMoving == false && _dir != MoveDir.None)
        {
            Vector3Int destPos = _cellPos;

            switch (_dir)
            {
                case MoveDir.None:
                    break;
                case MoveDir.Up:
                    destPos += Vector3Int.up;
                    break;
                case MoveDir.Down:
                    destPos += Vector3Int.down;
                    break;
                case MoveDir.Left:
                    destPos += Vector3Int.left;
                    break;
                case MoveDir.Right:
                    destPos += Vector3Int.right;
                    break;
                default:
                    break;
            }

            if (Managers.Map.CanGo(destPos))
            {
                _cellPos = destPos;
                _isMoving = true;
            }
        }
    }

    private void SearchAction()
    {
        Debug.DrawRay(_rigid.position, dirVec * 0.7f, new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(_rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

    public void ButtonDown(string type)
    {
        switch(type)
        {
            case "U":
                up_Value = 1;
                break;
            case "D":
                down_Value = -1;
                break;
            case "L":
                left_Value = -1;
                break;
            case "R":
                right_Value = 1;
                break;
            case "A":
                if (scanObject != null)
                    gameManager.Action(scanObject);
                break;
            case "C":
                Managers.Game.SubMenuActive();
                break;
        }
    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "U":
                up_Value = 0;
                break;
            case "D":
                down_Value = 0;
                break;
            case "L":
                left_Value = 0;
                break;
            case "R":
                right_Value = 0;
                break;
        }
    }

}
