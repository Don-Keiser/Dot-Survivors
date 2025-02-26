using UnityEngine;
using System.Collections;

public class AfterImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer afterImageSpriteRenderer;
    [SerializeField] SpriteRenderer ironCoreSpriteRenderer;
    private float fadeDuration = 0.5f;

    public void Initialize(Sprite playerSprite, Sprite ironCoreSprite, bool ironCoreActive)
    {
        // Get the main afterimage renderer
        if (afterImageSpriteRenderer != null)
        {
            afterImageSpriteRenderer.sprite = playerSprite;
            afterImageSpriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }

        if (ironCoreSpriteRenderer != null)
        {
            if (ironCoreSpriteRenderer != null)
            {
                ironCoreSpriteRenderer.sprite = ironCoreSprite; // Copy iron core sprite
                ironCoreSpriteRenderer.enabled = ironCoreActive;
                ironCoreSpriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, elapsedTime / fadeDuration);

            if (afterImageSpriteRenderer != null)
            {
                Color c = afterImageSpriteRenderer.color;
                c.a = alpha;
                afterImageSpriteRenderer.color = c;
            }

            if (ironCoreSpriteRenderer != null)
            {
                Color c = ironCoreSpriteRenderer.color;
                c.a = alpha;
                ironCoreSpriteRenderer.color = c;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
