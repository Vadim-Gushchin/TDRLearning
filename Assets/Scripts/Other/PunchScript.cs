using UnityEngine;

public class PunchScript : MonoBehaviour
{
    [SerializeField] private float _punchForce = 2f;
    [SerializeField] private float _punchDurationMax = 0.2f;

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
        _punchDurationTimer = _punchDurationMax;
        Vector2 difference = (transform.position - damageSource.position).normalized *_punchForce/rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopPunchMovement()
    {
        rb.linearVelocity = Vector3.zero;
        IsGotPunch = false;
    }

}
