using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public Grid _grid;
    public float _speed = 5.0f;

    //Player Cell Position
    private Animator _animator;
    private Rigidbody2D _rigid;
    private Vector3Int _cellPos = Vector3Int.zero;
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

    private void Start()
    {
        // 나중에 정보 긁어서 내 위치 바꿔주기.
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;
    }

    private void Update()
    {
        GetDirInput();
        UpdatePosition();
        UpdateIsMoving();
        SearchAction();
        GetInputKey();
    }

    // 키보드 입력
    private void GetInputKey()
    {
        //Scan Object
        if (Input.GetButtonDown("Jump") && scanObject != null)
            Debug.Log($"{scanObject.name}");
    }

    private void GetDirInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }
    }

    // 스르륵 이동하는 것을 처리
    private void UpdatePosition()
    {
        if (_isMoving == false)
            return;

        Vector3 destPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveDir = destPos - transform.position;

        // 도착 여부 체크
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

    // 이동 가능한 상태일 때, 실제 좌표를 이동한다.
    private void UpdateIsMoving()
    {
        if (_isMoving == false)
        {
            switch (_dir)
            {
                case MoveDir.None:
                    break;
                case MoveDir.Up:
                    _cellPos += Vector3Int.up;
                    _isMoving = true;
                    break;
                case MoveDir.Down:
                    _cellPos += Vector3Int.down;
                    _isMoving = true;
                    break;
                case MoveDir.Left:
                    _cellPos += Vector3Int.left;
                    _isMoving = true;
                    break;
                case MoveDir.Right:
                    _cellPos += Vector3Int.right;
                    _isMoving = true;
                    break;
                default:
                    break;
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

}
