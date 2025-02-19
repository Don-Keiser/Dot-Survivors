using UnityEngine;

[CreateAssetMenu(fileName = "EMPFieldWeapon", menuName = "ScriptableObjects/EMPFieldWeapon", order = 4)]
public class EMPFieldWeapon : AreaWeapon
{
    public override void UseWeapon(Transform firePoint, Transform player)
    {
        if (Time.time >= nextUseTime)
        {
            Instantiate(areaEffectPrefab, player.position, Quaternion.identity);
            nextUseTime = Time.time + cooldown;
        }
    }
}