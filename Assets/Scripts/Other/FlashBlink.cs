using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagebleObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration= 0.15f;


    private float _blinkTimer;
    private Material _originalMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;


        if(_damagebleObject is Player)
        {
            ((Player)_damagebleObject).OnPlayerTakeDamage += FlashBlink_OnPlayerTakeDamage;
            ((Player)_damagebleObject).OnPlayerDeath += FlashBlink_OnPlayerDeath;
        }

    }

    private void FlashBlink_OnPlayerDeath(object sender, System.EventArgs e)
    {
        StopBlinking();
    }

    private void Update()
    {
        if(_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer <= 0)
                SetDefoultMaterial();
        }
    }

    private void FlashBlink_OnPlayerTakeDamage(object sender, System.EventArgs e)
    {
        SetBlinkltMaterial();
    }

    private void SetBlinkltMaterial()
    {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    private void SetDefoultMaterial()
    {
        _spriteRenderer.material = _originalMaterial;
    }
    private void StopBlinking()
    {
        SetDefoultMaterial();
        _isBlinking = false;
    }
}

