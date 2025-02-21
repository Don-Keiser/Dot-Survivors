using UnityEngine;

[CreateAssetMenu(fileName = "XPOrbConfig", menuName = "ScriptableObjects/XPOrbConfig", order = 6)]
public class XPOrbConfig : ScriptableObject
{
    public string xpOrbName;
    public GameObject xpOrbPrefab;
    public int xpAmount;
}
