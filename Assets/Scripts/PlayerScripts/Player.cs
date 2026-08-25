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

    [Header("Character Base")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float damageRecoveryTime = 0.7f;
    [Header("Character Dash")]
    [SerializeField] private float dashSpeedMultiplayer = 10f;
    [SerializeField] private float dashCooldown = 2f; 
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;

    Vector2 inputVector;
    private float _minMovingSpeed = 0.1f;
    private float _deltaSpeedSaver;
    private bool _isRuning = false;

    private Rigidbody2D rigidBody;
    private PunchScript punchScript;

    private int _currentHealth;
    private bool _canTakeDamage = true;
    private bool _isAlive = true;
    private bool _canDash = true;

    public bool IsAlive => _isAlive;
    public bool IsRunning => _isRuning;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        punchScript = GetComponent<PunchScript>();
        Instance = this;
        _deltaSpeedSaver= movingSpeed;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPLayerDash += GameInput_OnPlayerDash;
        _currentHealth = maxHealth;

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
            OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
        }

    }


    private IEnumerator DamageRecoveryRoutine()
    {
        if (_isAlive)
        {
            yield return new WaitForSeconds(damageRecoveryTime);
            _canTakeDamage = true;
        }
    }
    private IEnumerator DashRoutine()
    {
        trailRenderer.emitting = true;
        _canDash = false;
        movingSpeed *= dashSpeedMultiplayer;

        yield return new WaitForSeconds(dashDuration);
        movingSpeed = _deltaSpeedSaver;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown + dashDuration);
        _canDash = true;
    }
 

    private void HandleMovement()
    {
        rigidBody.MovePosition(rigidBody.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
            _isRuning = true;
        else
            _isRuning = false;
    }
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    private void GameInput_OnPlayerDash(object sender, System.EventArgs e)
    {
        if (_canDash)
            StartCoroutine(DashRoutine()); 
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
    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        GameInput.Instance.OnPLayerDash -= GameInput_OnPlayerDash;
    }
}
