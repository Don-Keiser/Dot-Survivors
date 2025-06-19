using UnityEngine;

public class ApplyBurnOnHit : MonoBehaviour
{
    public float burnDamage = 2f;
    public float burnDuration = 3f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            var existing = col.GetComponent<BurningEffect>();
            if (existing == null)
            {
                var burn = col.gameObject.AddComponent<BurningEffect>();
                burn.Initialize(burnDamage, burnDuration);
            }
        }
    }
}