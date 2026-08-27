using System.Collections;
using UnityEngine;

public class TransparansyDirection : MonoBehaviour
{
    private const float FULL_NON_TRANSPARENT = 1f;


    [Range(0f, 1f)]
    [SerializeField] private float transparansyAmount= 0.8f;
    [SerializeField] private float fadeTime = 0.5f;

    SpriteRenderer _spriteRenderer;

    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
            if(collider is CapsuleCollider2D)
            StartCoroutine(FadeRoutine(_spriteRenderer,fadeTime, _spriteRenderer.color.a, transparansyAmount));
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
            if (collider is CapsuleCollider2D)
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, FULL_NON_TRANSPARENT));
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime,float startTransparansyAmount,float targetTransparansyAmount)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTransparansyAmount, targetTransparansyAmount, elapsedTime / fadeTime);
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            spriteRenderer.color = newColor;
            yield return null;
        }
    }
}


