using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInterface : MonoBehaviour {

    public GUISkin skin;

    public bool isOpen = false;

    public PlayerManager pManager;
    public Database database;

    public List<InventoryItem> itemList;
    public int slotY, slotX;

    public int curCrafting;

    void Start ()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i] = new InventoryItem();
        }

        database = GameObject.FindObjectOfType<Database>();
	}
	void Update ()
    {
		
	}

    private void OnGUI()
    {
        GUI.skin = skin;
        Event e = Event.current;

        if (isOpen)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 500, Screen.height / 2 - 250, 1000, 500));

            GUI.Box(new Rect(0, 0, 1000, 500), "Crafting");

            int j = 0;

            for(int y = 0; y < slotY; y++)
            {
                for(int x = 0; x < slotX; x++)
                {
                    Rect slotRect = new Rect(5 + (x * 51), 30 + (y* 51), 50, 50);

                    GUI.Box(slotRect, "");

                    if(itemList[j].stack >= 1)
                    {
                        GUI.DrawTexture(slotRect, itemList[j].item.icon);
                        GUI.Label(new Rect(slotRect), itemList[j].stack.ToString(), skin.GetStyle("CustomLabel"));

                        if (slotRect.Contains(e.mousePosition))
                        {
                            GUI.Box(new Rect(265, Screen.height / 2 - 142, 150, 25), "");
                            GUI.Label(new Rect(275, Screen.height / 2 - 147, 150, 25), itemList[j].item.name);


                            if (e.button == 0 && e.type == EventType.MouseDrag && !pManager.draggingItem)
                            {
                                pManager.draggingItem = true;
                                pManager.draggedItem = itemList[j];
                                pManager.previousIndex = j;
                                itemList[j] = new InventoryItem();
                                pManager.pickedItemFromInv = true;
                            }

                            if (e.type == EventType.MouseUp && pManager.draggingItem)
                            {

                                if (!pManager.pickedItemFromInv)
                                {
                                    pManager.hotbarItems[pManager.previousIndex] = itemList[j];
                                }
                                else
                                {
                                    pManager.items[pManager.previousIndex] = itemList[j];
                                }
                                itemList[j] = pManager.draggedItem;
                                pManager.draggedItem = new InventoryItem();
                                pManager.draggingItem = false;
                            }
                        }
                    }
                    else if (itemList[j].stack == 0)
                    {
                        if (slotRect.Contains(e.mousePosition))
                        {

                            if (e.type == EventType.MouseUp && pManager.draggingItem)
                            {
                                itemList[j] = pManager.draggedItem;
                                pManager.draggedItem = new InventoryItem();
                                pManager.draggingItem = false;
                            }
                        }
                    }

                    j++;
                }
            }

            int k;
            for (k = 0; k < database.craftingDatabase.Count; k++)
            {
                Rect craftingSlot = new Rect(300, 30 * (k + 51), 50, 50);
                GUI.Box(craftingSlot, "");

                GUI.DrawTexture(craftingSlot, database.itemDatabase[database.craftingDatabase[k].madeItemID].icon);

                if (craftingSlot.Contains(e.mousePosition))
                {

                    if (e.button == 0)
                    {
                        curCrafting = k;
                    }
                }
            }
            

            int i = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Rect slotRect = new Rect(740 + (x * 51), 30 + (y * 51), 50, 50);

                    GUI.Box(slotRect, "");

                    if (pManager.items[i].stack >= 1)
                    {
                        GUI.DrawTexture(slotRect, pManager.items[i].item.icon);
                        GUI.Label(new Rect(slotRect), pManager.items[i].stack.ToString(), skin.GetStyle("CustomLabel"));

                        if (slotRect.Contains(e.mousePosition))
                        {
                            GUI.Box(new Rect(265, Screen.height / 2 - 142, 150, 25), "");
                            GUI.Label(new Rect(275, Screen.height / 2 - 147, 150, 25), pManager.items[i].item.name);


                            if (e.button == 0 && e.type == EventType.MouseDrag && !pManager.draggingItem)
                            {
                                pManager.draggingItem = true;
                                pManager.draggedItem = pManager.items[i];
                                pManager.previousIndex = i;
                                pManager.items[i] = new InventoryItem();
                                pManager.pickedItemFromInv = true;
                            }

                            if (e.type == EventType.MouseUp && pManager.draggingItem)
                            {

                                if (!pManager.pickedItemFromInv)
                                {
                                    pManager.hotbarItems[pManager.previousIndex] = pManager.items[i];
                                }
                                else
                                {
                                    pManager.items[pManager.previousIndex] = pManager.items[i];
                                }
                                pManager.items[i] = pManager.draggedItem;
                                pManager.draggedItem = new InventoryItem();
                                pManager.draggingItem = false;
                            }
                        }
                    }
                    else if (pManager.items[i].stack == 0)
                    {
                        if (slotRect.Contains(e.mousePosition))
                        {

                            if (e.type == EventType.MouseUp && pManager.draggingItem)
                            {
                                pManager.items[i] = pManager.draggedItem;
                                pManager.draggedItem = new InventoryItem();
                                pManager.draggingItem = false;
                            }
                        }
                    }

                    i++;
                }
            }

            if (GUI.Button(new Rect(300, 465, 150, 30), "Craft!"))
            {
                CraftItem(k);
            }

            GUI.EndGroup();


            if (pManager.draggingItem)
            {
                GUI.DrawTexture(new Rect(e.mousePosition.x + 15, e.mousePosition.y, 35, 35), pManager.draggedItem.item.icon);
                GUI.Label(new Rect(e.mousePosition.x + 15, e.mousePosition.y, 35, 35), pManager.draggedItem.stack.ToString(), skin.GetStyle("CustomLabel"));
            }
        }
    }

    void CraftItem(int craftID)
    {
        bool canCraft = false;

        for(int i = 0; i < database.craftingDatabase.Count; i++)
        {
            if(database.craftingDatabase[i] != null)
            {
                for(int j = 0; j < database.craftingDatabase[i].requiredItems.Count; j++)
                {
                    
                    if (InventoryContains(database.craftingDatabase[i].requiredItems[j].ID))
                    {
                        canCraft = true;
                        pManager.AddItem(database.craftingDatabase[i].madeItemID, database.craftingDatabase[i].amount);

                        RemoveItem(database.craftingDatabase[i].requiredItems[j].ID, database.craftingDatabase[i].requiredItems[j].amount);
                    }
                }
            }
        }
    }

    public void RemoveItem(int ID, int amount)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].item.ID == ID)
            {
                if (itemList[i].stack <= amount)
                {
                    itemList[i] = new InventoryItem();
                }
                else if (itemList[i].stack > amount)
                {
                    itemList[i].stack -= amount;
                }
            }
        }
    }

    public bool InventoryContains(int ID)
    {
        bool result = false;
        for (int i = 0; i < itemList.Count; i++)
        {
            result = itemList[i].item.ID == ID;
            if (result)
            {
                break;
            }
        }
        return result;
    }
}
