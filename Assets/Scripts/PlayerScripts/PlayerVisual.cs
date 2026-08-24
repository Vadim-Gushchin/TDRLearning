using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Instance_OnPlayerDeath;
    }

    private void Update()
    {
        m_animator.SetBool(IS_RUNNING, Player.Instance.IsRunning);
        
        if(Player.Instance.IsAlive)
        AdjustPlyaerFacingDirection();
    }

    private void Instance_OnPlayerDeath(object sender, System.EventArgs e)
    {
        m_animator.SetBool(IS_DEAD, true);
    }

    private void AdjustPlyaerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPos.x)
            m_spriteRenderer.flipX = true;
        else
            m_spriteRenderer.flipX = false;
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Instance_OnPlayerDeath;
    }

}
