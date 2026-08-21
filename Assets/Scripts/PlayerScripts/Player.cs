using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }


    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 100;


    Vector2 inputVector;
    private float _minMovingSpeed = 0.1f;
    private bool _isRuning = false;
    

    private Rigidbody2D rigidBody;
    private PunchScript punchScript;

    private int _currentHealth;

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

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        if (punchScript.IsGotPunch == true)
            return;
        HandleMovement();
    }

    public bool IsRunning()
    {
        return _isRuning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSource, int damageAmount)
    {
        punchScript.GetPunch(damageSource);
        _currentHealth -= Mathf.Max(0,_currentHealth-=damageAmount);

    }

    private void HandleMovement()
    {
        rigidBody.MovePosition(rigidBody.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
            _isRuning = true;
        else
            _isRuning = false;
    }

}
