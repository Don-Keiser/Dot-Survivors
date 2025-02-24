using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponHotbarUI : MonoBehaviour
{
    [SerializeField] GameObject weaponSlotPrefab;
    [SerializeField] Transform hotbarContainer;
    private List<GameObject> slots = new List<GameObject>();

    public void InitializeHotbar(int maxWeapons)
    {
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

        for (int i = 0; i < maxWeapons; i++)
        {
            GameObject slot = Instantiate(weaponSlotPrefab, hotbarContainer);
            slots.Add(slot);

            Image weaponImage = slot.transform.GetChild(0).GetComponent<Image>();
            weaponImage.enabled = false;
        }
    }

    public void UpdateHotbar(List<WeaponBase> weapons)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image weaponImage = slots[i].transform.GetChild(0).GetComponent<Image>();

            if (i < weapons.Count)
            {
                weaponImage.sprite = weapons[i].weaponIcon;
                weaponImage.enabled = true;
            }
            else
            {
                weaponImage.enabled = false;
            }
        }
    }
}
