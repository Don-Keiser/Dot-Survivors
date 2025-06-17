using UnityEngine;
using System.Collections;

public class ExpandingAura : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float maxSize;
    private float duration;

    public void Initialize(float size, float fadeDuration, int level)
    {
        maxSize = size;
        duration = fadeDuration;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Adjust color intensity based on level
        float intensity = 0.5f + (level * 0.1f); // Increases brightness slightly per level
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, intensity);

        StartCoroutine(ExpandAndFade());
    }

    private IEnumerator ExpandAndFade()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.zero;
        Color initialColor = spriteRenderer.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            transform.localScale = Vector3.Lerp(initialScale, Vector3.one * maxSize, progress);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(1f, 0f, progress));

            yield return null;
        }

        Destroy(gameObject);
    }
}