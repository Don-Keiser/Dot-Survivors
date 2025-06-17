using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeEntry
{
    public string statName;
    public float value;

    public UpgradeEntry(string name, float defaultValue = 0f)
    {
        statName = name;
        value = defaultValue;
    }
}

[System.Serializable]
public class WeaponUpgradeStep
{
    public List<UpgradeEntry> upgrades = new List<UpgradeEntry>();

    public WeaponUpgradeStep(string[] possibleStats)
    {
        upgrades.Clear();
        foreach (string stat in possibleStats)
        {
            upgrades.Add(new UpgradeEntry(stat)); // Auto-fill stats with 0 by default
        }
    }

    public float GetUpgradeValue(string key, float defaultValue = 0f)
    {
        foreach (var entry in upgrades)
        {
            if (entry.statName == key)
                return entry.value;
        }
        return defaultValue;
    }
}
