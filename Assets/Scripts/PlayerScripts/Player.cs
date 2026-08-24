using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerTakeDamage;


    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _damageRecoveryTime = 0.5f;


    Vector2 inputVector;
    private float _minMovingSpeed = 0.1f;
    private bool _isRuning = false;


    private Rigidbody2D rigidBody;
    private PunchScript punchScript;

    private int _currentHealth;
    private bool _canTakeDamage = true;
    private bool _isAlive = true;

    public bool IsAlive => _isAlive;
    public bool IsRunning => _isRuning;



    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        punchScript = GetComponent<PunchScript>();
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        _currentHealth = _maxHealth;

    }
    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
        DetectDeath();
    }
    private void FixedUpdate()
    {
        if (punchScript.IsGotPunch == true)
            return;
        HandleMovement();
    }
    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSource, int damageAmount)
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            punchScript.GetPunch(damageSource);
            _currentHealth = Mathf.Max(0, _currentHealth -= damageAmount);
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
            OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
        }

    }

    private IEnumerator DamageRecoveryRoutine()
    {
        if (_isAlive)
        {
            yield return new WaitForSeconds(_damageRecoveryTime);
            _canTakeDamage = true;
        }
    }
    private void HandleMovement()
    {
        rigidBody.MovePosition(rigidBody.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
            _isRuning = true;
        else
            _isRuning = false;
    }
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _canTakeDamage = false;
            _isAlive = false;
            punchScript.StopPunchMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            GameInput.Instance.DisableMovement();
        }
    }

}
