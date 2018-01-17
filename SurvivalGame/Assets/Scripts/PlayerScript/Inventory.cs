using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
	public GUISkin skin;
	public bool isOpen = false;

	public FirstPersonController controller;
	public Database database;
	public PlayerManager manager;
	public List<InventoryItem> items;
	public List<InventoryItem> hotbarItems;

	bool draggingItem;
	InventoryItem draggedItem;
	int previousIndex;

    int selectedItemHotbar;

	void Start ()
	{
        database = GameObject.FindGameObjectWithTag("GameController").GetComponent<Database>();

        for (int x = 0; x < items.Count; x++) 
		{
			items[x] = new InventoryItem();
		}

        for(int i = 0; i < hotbarItems.Count; i++)
        {
            hotbarItems[i] = new InventoryItem();
        }

		AddItem (1, 5);
        AddItem(2, 5);
        AddItem(3, 5);
        AddItem(4, 5);
        AddItem(5, 5);
        AddItem(6, 7);
        AddItem(7, 1);
        AddItem(8, 1);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.E)) 
		{
			isOpen = !isOpen;
		}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedItemHotbar = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedItemHotbar = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedItemHotbar = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedItemHotbar = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedItemHotbar = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedItemHotbar = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedItemHotbar = 7;
        }
    }

	void OnGUI()
	{
		DrawHotBar();

		if (isOpen)
        {
			controller.enabled = false;

            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			Event e = Event.current;
			DrawInventory ();

			if (draggingItem) {
				GUI.DrawTexture (new Rect (e.mousePosition.x + 15, e.mousePosition.y, 35, 35), draggedItem.item.icon);
				GUI.Label (new Rect (e.mousePosition.x + 15, e.mousePosition.y, 35, 35), draggedItem.stack.ToString (), skin.GetStyle("CustomLabel"));
			}
		} 
		else {
			controller.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}

	void DrawInventory()
	{
		GUI.skin = skin;

		Event e = Event.current;
		int i = 0;

		GUI.BeginGroup (new Rect (Screen.width / 2 - 132, Screen.height / 2 - 142, 400, 285), "");

		GUI.Box(new Rect(0, 0, 265, 285), "Inventory");

		for (int y = 0; y < 5; y++)
		{
			for(int x = 0; x < 5; x++)
			{
				Rect slotRect = new Rect(5 + (x * 51), 25 + (y * 51), 50, 50);

				GUI.Box(slotRect, "");

				if(items[i].stack >= 1)
				{
					GUI.DrawTexture(slotRect, items[i].item.icon);
					GUI.Label(new Rect(slotRect), items[i].stack.ToString(), skin.GetStyle("CustomLabel"));

					if(slotRect.Contains(e.mousePosition))
					{
						GUI.Box(new Rect(265, Screen.height / 2 - 142, 150, 25), "");
						GUI.Label(new Rect(275, Screen.height / 2 - 147, 150, 25), items[i].item.name);

                        Debug.Log("Mouse hovering slot with item in it.");

						if(e.button == 0 && e.type == EventType.MouseDrag && !draggingItem)
						{
							draggingItem = true;
							draggedItem = items[i];
							previousIndex = i;
							items[i] = new InventoryItem();
						}

						if(e.type == EventType.MouseUp && draggingItem)
						{
                            Debug.Log("Item " + i + " should be put in item slot " + previousIndex);
							items[previousIndex] = items[i];
							items[i] = draggedItem;
							draggedItem = new InventoryItem();
							draggingItem = false;
						}
					}
                }
                else if (items[i].stack == 0)
                {
                    if (slotRect.Contains(e.mousePosition))
                    {
                        Debug.Log("Mouse hovering slot with no item in");

                        if (e.type == EventType.MouseUp && draggingItem)
                        {
                            Debug.Log("should put in empty slot " + i);
                            items[i] = draggedItem;
                            draggedItem = new InventoryItem();
                            draggingItem = false;
                        }
                    }
                }

                i++;
			}
		}

		GUI.EndGroup ();
	}


	void DrawHotBar()
	{
        Event e = Event.current;

        GUI.BeginGroup (new Rect (Screen.width / 2 - 200, Screen.height - 65, 400, 70), "");
		GUI.Box (new Rect (0, 0, 400, 65), "");

        int i = 0;

		for (int x = 0; x < 7; x++)
		{
            Rect hotbarSlot = new Rect(5 + (x * 56), 5, 55, 55);
            Rect hotbarSlotSel = new Rect(5 + ((selectedItemHotbar - 1) * 56), 5, 55, 5);

            GUI.Box(hotbarSlot, "");
            GUI.Box(hotbarSlotSel, "");

            

            if (hotbarItems[i].stack > 0)
            {
                GUI.DrawTexture(hotbarSlot, hotbarItems[i].item.icon);
                GUI.Label(new Rect(hotbarSlot), hotbarItems[i].stack.ToString(), skin.GetStyle("CustomLabel"));

                if (hotbarSlot.Contains(e.mousePosition))
                {
                    if (e.button == 0 && e.type == EventType.MouseDrag && !draggingItem)
                    {
                        draggingItem = true;
                        draggedItem = hotbarItems[i];
                        previousIndex = i;
                        hotbarItems[i] = new InventoryItem();
                    }

                    if (e.type == EventType.MouseUp && draggingItem)
                    {
                        Debug.Log("Item " + i + " should be put in item slot " + previousIndex);
                        hotbarItems[previousIndex] = hotbarItems[i];
                        hotbarItems[i] = draggedItem;
                        draggedItem = new InventoryItem();
                        draggingItem = false;
                    }
                }
            }
            else if (items[i].stack == 0)
            {
                if (hotbarSlot.Contains(e.mousePosition))
                {
                    Debug.Log("Mouse hovering hot bar slot with no item in");

                    if (e.type == EventType.MouseUp && draggingItem)
                    {
                        Debug.Log("should put in empty slot " + i);
                        hotbarItems[i] = draggedItem;
                        draggedItem = new InventoryItem();
                        draggingItem = false;
                    }
                }
            }

            i++;
        }

		GUI.EndGroup();
	}

	public void AddItem(int ID, int amount)
	{
		for (int x = 0; x < database.itemDatabase.Count; x++)
		{
			if(database.itemDatabase[x].ID == ID)
			{
				for(int y = 0; y < items.Count; y++)
				{
					if(items[y].item != null)
					{
						if(items[y].item.ID == ID)
						{
							items[y].stack += amount;
							break;
						}
					}
					else if(items[y].item == null)
					{
						items[y].item = database.itemDatabase[x];
						items[y].stack = amount;
						break;
					}
				}
			}
		}
	}

	public void RemoveItem(int ID, int amount)
	{
		for (int i = 0; i < items.Count; i++) {
			if(items[i].item.ID == ID)
			{
				if(items[i].stack <= amount){
					items[i] = new InventoryItem();
				}
				else if(items[i].stack > amount){
					items[i].stack -= amount;
				}
			}
		}
	}

	public bool InventoryContains(int ID)
	{
		bool result = false;
		for (int i = 0; i < items.Count; i++)
		{
			result = items[i].item.ID == ID;
			if (result)
			{
				break;
			}
		}
		return result;
	}
}
