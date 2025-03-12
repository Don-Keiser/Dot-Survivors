using UnityEngine;
using System.Collections;

public class AfterImage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer afterImageSpriteRenderer;
    [SerializeField] private SpriteRenderer ironCoreSpriteRenderer;
    [SerializeField] private SpriteRenderer outlineSpriteRenderer;
    private const float FadeDuration = 0.25f;
    private const float InitialAlpha = 0.4f;
    private const float IronCoreAlpha = 0.5f;

    public void Initialize(Sprite playerSprite, Sprite ironCoreSprite, bool ironCoreActive)
    {
        SetSpriteRenderer(afterImageSpriteRenderer, playerSprite, InitialAlpha);
        SetSpriteRenderer(outlineSpriteRenderer, outlineSpriteRenderer.sprite, InitialAlpha);
        SetSpriteRenderer(ironCoreSpriteRenderer, ironCoreSprite, IronCoreAlpha, ironCoreActive);

        StartCoroutine(FadeOut());
    }

    private void SetSpriteRenderer(SpriteRenderer spriteRenderer, Sprite sprite, float alpha, bool enabled = true)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            spriteRenderer.enabled = enabled;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(InitialAlpha, 0f, elapsedTime / FadeDuration);

            SetAlpha(afterImageSpriteRenderer, alpha);
            SetAlpha(outlineSpriteRenderer, alpha);
            SetAlpha(ironCoreSpriteRenderer, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }
    }
}