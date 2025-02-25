using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveManager : MonoBehaviour
{
    public List<PassiveUpgrade> acquiredPassives = new List<PassiveUpgrade>();
    public int maxPassives = 6;
    private HashSet<string> acquiredPassiveNames = new HashSet<string>();

    public void AddPassive(PassiveUpgrade newPassive)
    {
        if (acquiredPassiveNames.Contains(newPassive.passiveName))
        {
            Debug.Log($"Passive {newPassive.passiveName} is already acquired!");
            return;
        }

        if (acquiredPassives.Count >= maxPassives)
        {
            Debug.Log("Passive limit reached!");
            return;
        }

        PassiveUpgrade passiveInstance = newPassive.Clone();
        acquiredPassives.Add(passiveInstance);
        acquiredPassiveNames.Add(passiveInstance.passiveName);

        PlayerPassives.Instance.ApplyPassiveUpgrade(passiveInstance);
    }

    public void UpgradePassive(int passiveIndex)
    {
        if (passiveIndex >= 0 && passiveIndex < acquiredPassives.Count)
        {
            acquiredPassives[passiveIndex].UpgradePassive();
            PlayerPassives.Instance.ApplyPassiveUpgrade(acquiredPassives[passiveIndex]);
        }
    }
}