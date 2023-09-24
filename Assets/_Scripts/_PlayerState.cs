using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _PlayerState : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;

    public PlayerState state;

    public GameObject target;
    [SerializeField] private float _speed = 5;

    [Header("====== DASH ======")]
    [SerializeField] bool canDash = true;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float _turnSpeed = 360;
    private Vector3 _input;
    private int animationStateHash;
    private float prePosX;
    private Vector3 prevFw;
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _anim = this.GetComponent<Animator>();
        _speed = this.GetComponent<CharacterData>().movementSpeed;
    }
    private void Update()
    {
        GatherInput();
        Look();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = PlayerState.Attacking;
        }
    }

    private void FixedUpdate()
    {
        Move();
        ChangeAnimation();
    }
    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
    }

    private void Look()
    {
        
        if (state != PlayerState.Attacking)
        {
            if (_input == Vector3.zero)
            {
                state = PlayerState.Idle; 
                return;
            }
            else
            {
                state = PlayerState.Running;
                
            }

            if (!target)
            {
                float targetAngle = Mathf.Atan2(_input.x, _input.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
            }
            else
            {
                transform.LookAt(target.transform);

            }

        }
        else
        {
            _anim.CrossFade("Attack", 0);
            Invoke("ResetIdle", 1);
            
        }    
    }

    private void Move()
    {
        if (state == PlayerState.Running)
        {
            if (!target)
            {
                _rb.MovePosition(transform.position + _input.normalized * _input.normalized.magnitude * _speed * Time.deltaTime);
                _anim.CrossFade("Running", 0.1f);
            }
            else
            {
                _rb.MovePosition(transform.position + _input.normalized * _input.normalized.magnitude * _speed * Time.deltaTime);
                float distance = Vector3.Distance(transform.position, target.transform.position);
                _anim.CrossFade("Target_Run", 0.1f);
                if (distance >= prePosX)
                {

                    _anim.SetFloat("y", -1);
                    _speed = this.GetComponent<CharacterData>().movementSpeed * 0.75f;
                }
                else
                {
                    _anim.SetFloat("y", 1);
                    _speed = this.GetComponent<CharacterData>().movementSpeed;
                }
                prePosX = distance;

            }
        }
            
          
    }
    void Dash()
    {
        canDash = false;
        state = PlayerState.Dashing;
        _rb.velocity = transform.forward * dashSpeed;
        Invoke("resetDash", dashCooldown);
    }
    void resetDash()
    {
        _rb.velocity = Vector3.zero;
        canDash = true;
        state = PlayerState.Idle;
    }

    void ChangeAnimation()
    {
        switch (state)
        {
            case PlayerState.Idle:
                _anim.CrossFade("Idle", 0.1f);
                //_anim.CrossFade("Running_A", 0f);
                break;
            case PlayerState.Running:
                
                //.CrossFade("Running_A", 0f);
                break;
            case PlayerState.Dashing:
                //_anim.CrossFade("Dash", 0);
                break;
            case PlayerState.Attacking:
                //_anim.CrossFade("Attack", 0);
                break;
        }
    }

    private void ResetIdle()
    {
        state = PlayerState.Idle;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
        