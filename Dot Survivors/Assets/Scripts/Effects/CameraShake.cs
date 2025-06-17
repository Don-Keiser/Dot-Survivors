using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Shake(float intensity, float duration)
    {
        originalPosition = transform.position;
        StartCoroutine(ShakeRoutine(intensity, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-intensity, intensity) * 0.5f;
            float offsetY = Random.Range(-intensity, intensity);
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }

        transform.position = originalPosition;
    }
}
