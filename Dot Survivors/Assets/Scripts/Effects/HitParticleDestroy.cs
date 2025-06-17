using UnityEngine;
using System.Collections;

public class HitParticleDestroy : MonoBehaviour
{
    private static readonly float FadeDuration = 0.3f;

    void Start()
    {
        StartCoroutine(FadeOutAndDestroy(gameObject));
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;

        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, elapsedTime / FadeDuration);
                sr.color = c;
            }
            yield return null;
        }

        Destroy(obj);
    }
}
