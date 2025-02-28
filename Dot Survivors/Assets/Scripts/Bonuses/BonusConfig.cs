using UnityEngine;

[CreateAssetMenu(fileName = "BonusConfig", menuName = "ScriptableObjects/BonusConfig", order = 6)]
public class BonusConfig : ScriptableObject
{
    public string bonusName;
    public GameObject bonusPrefab;
    public int bonusAmount;
}
