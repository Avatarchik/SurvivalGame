using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInterface : MonoBehaviour {

    public bool isOpen = false;

    public List<InventoryItem> itemList;
    public int slotY, slotX;

    void Start ()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i] = new InventoryItem();
        }
	}
	void Update ()
    {
		
	}

    private void OnGUI()
    {
        if (isOpen)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 500, Screen.height / 2 - 250, 1000, 500));

            GUI.Box(new Rect(0, 0, 1000, 500), "Crafting");

            for(int y = 0; y < slotY; y++)
            {
                for(int x = 0; x < slotX; x++)
                {
                    Rect slotRect = new Rect(5 + (x * 51), 30 + (y* 51), 50, 50);

                    GUI.Box(slotRect, "");
                }
            }

            GUI.EndGroup();
        }
    }
}
