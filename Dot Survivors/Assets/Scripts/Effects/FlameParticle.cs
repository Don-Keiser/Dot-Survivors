using UnityEngine;

public class FlameParticle : MonoBehaviour
{
    private float floatSpeed = 1.3f;
    private float lifetime = 0.5f;
    private float scaleDecay = 2.2f;
    private float alphaDecaySpeed = 3.5f;

    private SpriteRenderer spriteRenderer;
    private Color startColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        float scale = Mathf.Lerp(transform.localScale.x, 0f, Time.deltaTime * scaleDecay);
        transform.localScale = new Vector3(scale, scale, 1f);

        startColor.a = Mathf.Lerp(startColor.a, 0f, Time.deltaTime * alphaDecaySpeed);
        spriteRenderer.color = startColor;
    }
}