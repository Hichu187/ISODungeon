using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Component
    private NavMeshAgent _agent;
    private FieldOfView _fieldOfView;
    private Animator _anim;
    private CapsuleCollider _col;
    private EnemyData _data;

    [SerializeField] private EnemyState state;
    [Header("PATROLLING & MOVING")]
    [SerializeField] private float randomRange;
    private Vector3 desPoint;
    bool walkPointSet;
    [Header("ATTACK")]
    [SerializeField] float atkCd;
    [SerializeField] bool canAtk;
    [SerializeField] bool isAttacking;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _fieldOfView = GetComponent<FieldOfView>();
        _anim = GetComponent<Animator>();
        _col = GetComponent<CapsuleCollider>();
        _data = GetComponent<EnemyData>();

        _agent.speed = _data.data.movementSpeed;
        _fieldOfView.radius = _data.data.senseRange;
        atkCd = _data.data.cooldownAtk;
        canAtk = true;
        isAttacking = false;
    }
    void Start()
    {
        
    }
    void Update()
    {
        Patrolling();
        Chasing();
    }

    void FixedUpdate()
    {
        ChangeAnimation();
    }

    #region AI PATROLLING
    void Patrolling()
    {
        if (!_fieldOfView.canSeePlayer)
        {
            if (!walkPointSet) SearchDestination();
            else
            {
                _agent.SetDestination(desPoint);
            }

            if (Vector3.Distance(transform.position, desPoint) < 0.5f) Invoke("resetDestination", 1f);
        }
    }

    void SearchDestination()
    {
        float z = Random.Range(-randomRange, randomRange);
        float x = Random.Range(-randomRange, randomRange);

        desPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        walkPointSet = true;
    }

    void resetDestination()
    {
        walkPointSet = false;

    }
    #endregion

    #region CHASING PLAYER
    void Chasing()
    {
        if (_fieldOfView.canSeePlayer && !isAttacking)
        {
            Vector3 playerPos = _fieldOfView.playerRef.transform.position;
            if (Vector3.Distance(transform.position, playerPos) > _data.data.atkRange)
            {
                _agent.SetDestination(playerPos);
                state = EnemyState.Chasing;
            } 
            else
            {
                _agent.ResetPath();
                Invoke("Attack", 1f);
            }
        }
    }
    #endregion

    #region ATTACK
    void Attack()
    {
        if (canAtk)
        {                     
            StartCoroutine(AttackCooldown());
        }
        else
        {
            state = EnemyState.Idle;
        }
    }

    private IEnumerator AttackCooldown()
    {
        isAttacking = true;
        state = EnemyState.Attacking;
        yield return new WaitForSeconds(0.75f);
        canAtk = false;
        isAttacking = false;
        yield return new WaitForSeconds(atkCd);
        canAtk = true;
        
    }
    #endregion
    #region ANIMATION
    void ChangeAnimation()
    {
        switch (state)
        {
            case EnemyState.Idle:
                _anim.CrossFade("Idle", 0f);
                break;
            case EnemyState.Patrolling:
                _anim.CrossFade("move", 0f);
                break;
            case EnemyState.Chasing:
                _anim.CrossFade("chase", 0f);
                break;
            case EnemyState.Attacking:
                
                switch (_data.data.type)
                {
                    case EnemyType.MeleeEnemy:
                        _anim.CrossFade("melee_atk", 0f);
                        break;
                    case EnemyType.RangerEnemy:
                        _anim.CrossFade("range_atk", 0f);
                        break;
                    case EnemyType.CasterEnemy:
                        _anim.CrossFade("caster_atk", 0f);
                        break;
                }
                break;
            case EnemyState.Takedame:
                _anim.CrossFade("Take Damage", 0f);
                break;
            case EnemyState.Die:
                _anim.CrossFade("Die", 0f);
                break;
        }
    }
    #endregion
}
