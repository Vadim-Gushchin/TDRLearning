using UnityEngine;

public class PunchScript : MonoBehaviour
{
    [SerializeField] private float punchForce = 2f;
    [SerializeField] private float punchDurationMax = 0.2f;

    private float _punchDurationTimer;
    private Rigidbody2D rb;

    public bool IsGotPunch { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        _punchDurationTimer -= Time.deltaTime;
        if (_punchDurationTimer <= 0)
            StopPunchMovement();

    }

    public void GetPunch(Transform damageSource)
    {
        IsGotPunch= true;
        _punchDurationTimer = punchDurationMax;
        Vector2 difference = (transform.position - damageSource.position).normalized *punchForce/rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopPunchMovement()
    {
        rb.linearVelocity = Vector3.zero;
        IsGotPunch = false;
    }

}
