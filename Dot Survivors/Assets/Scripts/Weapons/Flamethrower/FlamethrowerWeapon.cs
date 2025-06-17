using UnityEngine;

[CreateAssetMenu(fileName = "FlamethrowerWeapon", menuName = "ScriptableObjects/FlamethrowerWeapon", order = 6)]
public class FlamethrowerWeapon : WeaponBase
{
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private float flameRange = 3f;
    [SerializeField] private float coneAngle = 30f;
    [SerializeField] private float damagePerSecond = 10f;

    private float cooldownTimer = 0f;
    private GameObject leftFlameInstance;
    private GameObject rightFlameInstance;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = cooldown;
            FireFlames(player);
        }
    }

    private void FireFlames(Transform player)
    {
        Vector2 leftPosition = (Vector2)player.position + Vector2.left * 0.5f; // Left of player
        Vector2 rightPosition = (Vector2)player.position + Vector2.right * 0.5f; // Right of player

        // Create right flame normally
        rightFlameInstance = Instantiate(flamePrefab, rightPosition, Quaternion.identity, player);
        rightFlameInstance.GetComponent<Flame>().Initialize(flameRange, coneAngle, damagePerSecond, 1); // Right side

        // Create left flame and FLIP it
        leftFlameInstance = Instantiate(flamePrefab, leftPosition, Quaternion.identity, player);
        leftFlameInstance.transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        leftFlameInstance.GetComponent<Flame>().Initialize(flameRange, coneAngle, damagePerSecond, -1); // Left side
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "rangeIncrease", "coneAngleIncrease", "cooldownReduction" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        damagePerSecond += upgrade.GetUpgradeValue("damageIncrease");
        flameRange += upgrade.GetUpgradeValue("rangeIncrease");
        coneAngle += upgrade.GetUpgradeValue("coneAngleIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
    }

    public override WeaponBase Clone()
    {
        FlamethrowerWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.damagePerSecond = this.damagePerSecond;
        copy.coneAngle = this.coneAngle;
        copy.cooldown = this.cooldown;
        return copy;
    }
}
