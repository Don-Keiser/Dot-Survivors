using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PassiveHotbarUI : MonoBehaviour
{
    [SerializeField] GameObject passiveSlotPrefab;
    [SerializeField] Transform hotbarContainer;
    private List<GameObject> slots = new List<GameObject>();

    public void InitializeHotbar(int maxPassives)
    {
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

        for (int i = 0; i < maxPassives; i++)
        {
            GameObject slot = Instantiate(passiveSlotPrefab, hotbarContainer);
            slots.Add(slot);

            Image passiveImage = slot.transform.GetChild(0).GetComponent<Image>();
            passiveImage.enabled = false;
        }
    }

    public void UpdateHotbar(List<PassiveUpgrade> passives)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image passiveImage = slots[i].transform.GetChild(0).GetComponent<Image>();

            if (i < passives.Count)
            {
                passiveImage.sprite = passives[i].passiveIcon;
                passiveImage.enabled = true;
            }
            else
            {
                passiveImage.enabled = false;
            }
        }
    }
}
