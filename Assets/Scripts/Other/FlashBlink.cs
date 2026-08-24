using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour damagebleObject;
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.15f;


    private float _blinkTimer;
    private Material _originalMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
    }

    private void Start()
    {
        if (damagebleObject is Player player)
        {
            player.OnPlayerTakeDamage += FlashBlink_OnPlayerTakeDamage;
            player.OnPlayerDeath += FlashBlink_OnPlayerDeath;
        }
    }

    private void FlashBlink_OnPlayerDeath(object sender, System.EventArgs e)
    {
        StopBlinking();
    }

    private void Update()
    {
        if (_isBlinking)
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
        _blinkTimer = blinkDuration;
        _spriteRenderer.material = blinkMaterial;
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
    private void OnDestroy()
    {
        if (damagebleObject is Player player)
        {
            player.OnPlayerDeath -= FlashBlink_OnPlayerDeath;
            player.OnPlayerTakeDamage -= FlashBlink_OnPlayerTakeDamage;
        }
    }
}

