using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _PlayerState : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;

    public PlayerState state;
    public CharacterData data;
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
        data = this.GetComponent<CharacterData>();
        _speed = data.movementSpeed;
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
            if (!data.LeftHandEquippedWeapon)
            {
                _anim.CrossFade("1HandAttack", 0,1);
                CheckClass();
            }
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

        if (_input == Vector3.zero)
        {
            state = PlayerState.Idle;
            return;
        }
        else
        {
            state = PlayerState.Running;
        }
    }

    private void Look()
    {
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

    private void Move()
    {
        if (state == PlayerState.Running)
        {
            if (!target)
            {
                _rb.MovePosition(transform.position + _input.normalized * _input.normalized.magnitude * _speed * Time.deltaTime);
                _anim.CrossFade("Running", 0f);
                Debug.Log("b");
            }
            else
            {
                _rb.MovePosition(transform.position + _input.normalized * _input.normalized.magnitude * _speed * Time.deltaTime);
                float distance = Vector3.Distance(transform.position, target.transform.position);
                _anim.CrossFade("Target_Run", 0f);
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
        if(!target)
            _rb.velocity = transform.forward * dashSpeed;
        else
        {
            _rb.velocity = transform.position + _input.normalized * dashSpeed;
        }
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
                if (!target)
                {
                    _anim.CrossFade("Idle", 0.1f);
                } 
                else 
                    _anim.SetFloat("y", 0);
                break;
            case PlayerState.Running:    
                
                break;
            case PlayerState.Dashing:
                break;
            case PlayerState.Attacking:
                
                break;
        }
    }

    private void ResetIdle()
    {
        state = PlayerState.Idle;
    }

    void CheckClass()
    {
        switch (data.Class)
        {
            case PlayerClass.Knight:
                _anim.SetFloat("1Hand", 0);
                break;
            case PlayerClass.Berserker:
                _anim.SetFloat("1Hand", 0.25f);
                break;
        }
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
        