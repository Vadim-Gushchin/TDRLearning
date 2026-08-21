using UnityEngine;

[SelectionBase]
public class PlayerMoving : MonoBehaviour
{
    public static PlayerMoving Instance { get; private set; }


    [SerializeField] private float movingSpeed = 5f;

    Vector2 inputVector;
    private float minMovingSpeed = 0.1f;
    private bool isRuning = false;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
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
        HandleMovement();
    }

    private void HandleMovement()
    {
        rigidBody.MovePosition(rigidBody.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
            isRuning = true;
        else
            isRuning = false;
    }

    public bool IsRunning()
    {
        return isRuning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }


}
