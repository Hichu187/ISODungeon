using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    private CharacterController _controller;
    [SerializeField] private float _speed = 5;

    [Header("Dash")]
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = true;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float _turnSpeed = 360;
    float turnSmoothVelocity;
    public Vector3 moveDir;
    private Vector3 _input;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _anim = this.GetComponent<Animator>();
        _controller = this.GetComponent<CharacterController>();
    }
    private void Update()
    {
        GatherInput();
        Look();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }
    }

    private void FixedUpdate()
    {
        Move();

        
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
    }

    private void Look()
    {
        if (_input == Vector3.zero)
        {
            _anim.CrossFade("Idle", 0.1f);
            return;
        }else
        {
            if(!isDashing) _anim.CrossFade("Running_A", 0f);
        }

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);  
    }
    void Dash()
    {
        _anim.CrossFade("Dash", 0f);
        canDash = false;
        isDashing = true;
        _rb.velocity = transform.forward * dashSpeed;
        Invoke("resetDash", dashCooldown);
    }
    void resetDash()
    {
        _rb.velocity = Vector3.zero;
        canDash = true;
        _anim.CrossFade("Idle", 0f);
        isDashing = false;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
        