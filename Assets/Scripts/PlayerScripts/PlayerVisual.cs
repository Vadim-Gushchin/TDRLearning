using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        m_animator.SetBool(IS_RUNNING, PlayerMoving.Instance.IsRunning());
        AdjustPlyaerFacingDirection();
    }

    private void AdjustPlyaerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPos = PlayerMoving.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPos.x)
            m_spriteRenderer.flipX = true;
        else
            m_spriteRenderer.flipX = false;
    }

}
