using UnityEngine;

[CreateAssetMenu(fileName = "GatlingWeapon", menuName = "ScriptableObjects/GatlingWeapon", order = 2)]
public class GatlingWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public int volleyCount = 5;

    private float nextFireTime;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        if (Time.time >= nextFireTime)
        {
            for (int i = 0; i < volleyCount; i++)
            {
                Vector2 randomPoint = Random.insideUnitCircle.normalized * 10f;
                Vector2 direction = (randomPoint - (Vector2)firePoint.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
                projectile.GetComponent<Projectile>().damage = damage;
            }
            nextFireTime = Time.time + 1f / cooldown;
        }
    }

    public override void UpgradeWeapon()
    {
        level++;
        damage += 2f;
        volleyCount += 1;
        cooldown += 0.2f;
    }
}