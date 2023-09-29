using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class _PlayerState : MonoBehaviour
{
    public static _PlayerState instance;
    private void Awake()
    {
        instance = this;
    }
    private Rigidbody _rb;
    private Animator _anim;
    private NavMeshAgent _agent;

    public PlayerState state;
    public CharacterData data;

    [Header("====== MOVEMENT ======")]
    [SerializeField] Camera movementCamera;
    [SerializeField] private float _speed = 5;

    [Header("====== DASH ======")]
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = false;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float dashSpeed = 20f;
    [Header("====== COMBAT ======")]
    public GameObject target;


    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _anim = this.GetComponent<Animator>();
        _agent = this.GetComponent<NavMeshAgent>();
        data = this.GetComponent<CharacterData>();
        //
        isDashing = false;
        _agent.speed = data.movementSpeed;
        CheckClass();
    }
    private void Update()
    {
        CheckMouseClickPoint();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }
    }


    private void FixedUpdate()
    {
        ChangeAnimation();
        MouseClickAttack();
    }

    #region MOVING MOUSE POSITION
    private void CheckMouseClickPoint()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                transform.LookAt(hit.transform);
                Movement(hit);
                if (hit.transform.tag == "Enemy") target = hit.transform.gameObject;
                else if(hit.transform.tag == "Ground") target = null;
            }
        }

    }

    private void Movement(RaycastHit hit)
    {
        Vector3 prePos = this.transform.position;
        _agent.ResetPath();
        _agent.speed = data.movementSpeed;
        _agent.SetDestination(hit.transform.position);

    }
    #endregion

    #region CLICK ATTACK
    private void MouseClickAttack()
    {
        if (target && Vector3.Distance(transform.position, target.transform.position) <= data.RightHandEquippedWeapon.AttackRange && !isDashing)
        {
            _agent.ResetPath();

            state = PlayerState.Attacking;
        }
        else
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance) state = PlayerState.Idle;
            else state = PlayerState.Running;
        }
    }
    #endregion

    #region DASH

    void Dash()
    {
        canDash = false;
        _agent.ResetPath();
        state = PlayerState.Dashing;
        isDashing = true;
        Vector3 newPosition = transform.position + transform.forward * dashSpeed;
        transform.DOMove(newPosition, 0.25f).OnComplete(() => { isDashing = false; });

        Invoke("resetDash", dashCooldown);

    }
    void resetDash()
    {
        canDash = true;        
        state = PlayerState.Idle;
    }
    #endregion

    #region ANIMATION
    void ChangeAnimation()
    {
        switch (state)
        {
            case PlayerState.Idle:
                _anim.CrossFade("Idle", 0.1f);
                _anim.SetFloat("y", 0);
                break;
            case PlayerState.Running:
                _anim.CrossFade("Running", 0f);
                break;
            case PlayerState.Attacking:
                switch (data.RightHandEquippedWeapon.weapon)
                {
                    case WeaponType.Claymores:
                    case WeaponType.Staff:
                    case WeaponType.BigAxes:
                    case WeaponType.BigCrossbows:
                        _anim.CrossFade("2_HandAttack_1", 0);
                        break;
                    case WeaponType.Axes:
                    case WeaponType.Knife:
                    case WeaponType.Swords:
                    case WeaponType.Wands:
                    case WeaponType.Crossbows:
                        if (data.LeftHandEquippedWeapon == null) _anim.CrossFade("One_HandAttack_1", 0);
                        else
                        {
                            switch (data.LeftHandEquippedWeapon.weapon)
                            {
                                case WeaponType.Shield:
                                case WeaponType.Catalists:
                                    _anim.CrossFade("One_HandAttack_1", 0);
                                    break;
                                case WeaponType.Axes:
                                case WeaponType.Knife:
                                case WeaponType.Swords:
                                    _anim.CrossFade("DualAttack_1", 0);
                                    break;
                            }
                        }
                        break;
                }
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
    #endregion
}

