using UnityEngine;

[CreateAssetMenu(fileName = "PassiveUpgrade", menuName = "ScriptableObjects/PassiveUpgrade", order = 10)]
public class PassiveUpgrade : ScriptableObject
{
    public string passiveName;
    public PassiveType passiveType;
    public Sprite passiveIcon;
    public int level = 1;
    public int maxLevel = 5;
    public float baseValue;
    public float increasePerLevel;

    public virtual bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public void UpgradePassive() 
    {
        if (!CanUpgrade()) return;

        level++;
    }


    public PassiveUpgrade Clone()
    {
        return Instantiate(this);
    }
}

public enum PassiveType
{
    IronCore,
    RegenerativeShell,
    BerserkerRage,
    EvasiveInstinct,
    SharpenedReflexes,
    BattleHardened
}