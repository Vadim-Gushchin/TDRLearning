using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using TDRL.Utils;
using UnityEngine.InputSystem.XR.Haptics;
using TMPro;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimeMax = 2f;


    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplayer = 2;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackRate = 2f;

    private float _nextAttackTime = 0f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimeActual;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.02f;
    private Vector3 _lastPostition;


    public event EventHandler OnEnemyAttack;

    public bool IsRunning => _navMeshAgent.velocity != Vector3.zero;
    public float RoamingAnimationSpeed => _navMeshAgent.speed / _roamingSpeed;
   

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplayer;
    }


    private void Update()
    {
        StateHandle();
        MovementDirectionHandler();
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void StateHandle()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimeActual -= Time.deltaTime;
                if (_roamingTimeActual < 0)
                {
                    Roaming();
                    _roamingTimeActual = _roamingTimeMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttakingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;

            default:
            case State.Idle:
                break;

        }
    }

    private void CheckCurrentState()
    {
        float distanseToPLayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;



        if (_isChasingEnemy)
        {
            if (distanseToPLayer <= _chasingDistance)
                newState = State.Chasing;
        }

        if (_isAttackingEnemy)
        {
            if (distanseToPLayer <= _attackDistance)
                newState = State.Attacking;
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _roamingTimeActual = 1f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();

            }
            _currentState = newState;
        }
    }


    private void AttakingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
                ChangeFacingDirection(_lastPostition, transform.position);
            else if (_currentState == State.Attacking)
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);

            _lastPostition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);

    }
}
