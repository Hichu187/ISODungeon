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
    
    [Header("====== MOVEMENT ======")]
    [SerializeField] private float _speed = 5;
    private Vector3 _input;
    private float prePosX;
    [Header("====== DASH ======")]
    [SerializeField] bool canDash = true;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float _turnSpeed = 360;
    [Header("====== COMBAT ======")]
    public GameObject target;
    private float nextFireTime = 0f;
    private static int noOfClicks = 0;
    private float lastClickedTime =0;
    private float maxComboDelay = 1;


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
        if (Time.time > 0)
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
            }
        }

        ResetAttack();
    }

    private void FixedUpdate()
    {
        Move();
        ChangeAnimation();
    }
    #region Moving
    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));


    }

    private void Look()
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

    private void Move()
    {
        if (state == PlayerState.Running)
        {
            if (!target)
            {
                _rb.MovePosition(transform.position + _input.normalized * _input.normalized.magnitude * _speed * Time.deltaTime);
                _anim.CrossFade("Running", 0f);
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
    #endregion
    #region Dash
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
    #endregion

    #region COMBAT
    void Attack()
    {
        lastClickedTime = Time.time;
        noOfClicks++; 
        CheckClass();
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 5);
        if (data.LeftHandEquippedWeapon)
        {
            if(data.LeftHandEquippedWeapon.weapon == WeaponType.Shield || data.LeftHandEquippedWeapon.weapon == WeaponType.Catalists)
            {
                if (noOfClicks == 1)
                    _anim.CrossFade("One_HandAttack_1", 0, 1);

                if (noOfClicks >= 2 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("One_HandAttack_1"))
                    _anim.CrossFade("One_HandAttack_2", 0, 1);

                if (noOfClicks >= 3 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("One_HandAttack_2"))
                    _anim.CrossFade("One_HandAttack_3", 0, 1);

                if (noOfClicks >= 4 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("One_HandAttack_3"))
                    _anim.CrossFade("One_HandAttack_4", 0, 1);
                if (noOfClicks >= 5 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("One_HandAttack_4"))
                    noOfClicks = 0;
            }else if(data.LeftHandEquippedWeapon.weapon == WeaponType.Axes || data.LeftHandEquippedWeapon.weapon == WeaponType.Knife || data.LeftHandEquippedWeapon.weapon == WeaponType.Swords)
            {
                if (noOfClicks == 1)
                    _anim.CrossFade("DualAttack_1", 0, 1);

                if (noOfClicks >= 2 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("DualAttack_1"))
                    _anim.CrossFade("DualAttack_2", 0, 1);

                if (noOfClicks >= 3 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("DualAttack_2"))
                    _anim.CrossFade("DualAttack_3", 0, 1);

                if (noOfClicks >= 4 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("DualAttack_3"))
                    noOfClicks = 0;
            }


        }
        else
        {
            if (noOfClicks == 1)
                _anim.CrossFade("2_HandAttack_1", 0, 1);

            if (noOfClicks >= 2 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("2_HandAttack_1"))
                _anim.CrossFade("2_HandAttack_2", 0, 1);

            if (noOfClicks >= 3 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("2_HandAttack_2"))
                _anim.CrossFade("2_HandAttack_3", 0, 1);

            if (noOfClicks >= 4 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("2_HandAttack_3"))
                _anim.CrossFade("2_HandAttack_4", 0, 1);
            if (noOfClicks >= 5 && _anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(1).IsName("2_HandAttack_4"))
                noOfClicks = 0;
        }
        
    }

    void ResetAttack()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

    }
    #endregion
    void ChangeAnimation()
    {
        switch (state)
        {
            case PlayerState.Idle:
                _anim.CrossFade("Idle", 0.1f);
                _anim.SetFloat("y", 0);
                break;
        }
    }

    void CheckClass()
    {
        switch (data.Class)
        {
            case PlayerClass.Knight:
                break;
            case PlayerClass.Berserker:
                break;
            case PlayerClass.Assasin:
                _anim.SetFloat("1Hand", 0f);
                break;
            case PlayerClass.Rogue:
                _anim.SetFloat("1Hand", 0.5f);
                break;
            case PlayerClass.Mage:
                _anim.SetFloat("1Hand", 1f);
                break;
        }
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
        