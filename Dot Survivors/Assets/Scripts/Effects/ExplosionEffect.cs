using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private float maxSize;
    private float expandSpeed = 10f;
    private SpriteRenderer spriteRenderer;

    public void Initialize(float explosionRadius)
    {
        maxSize = explosionRadius * 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, 1), expandSpeed * Time.deltaTime);

        if (transform.localScale.x >= maxSize * 0.9f)
        {
            Destroy(gameObject);
        }
    }
}
