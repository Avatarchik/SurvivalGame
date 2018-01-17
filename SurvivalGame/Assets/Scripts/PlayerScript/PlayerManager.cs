﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerManager : MonoBehaviour
{
    public SingleplayerWorldCreator worldCreator;

    public FirstPersonController controller;
    public PlayerManager manager;
    public GameObject cam;

    bool pauseMenu = false;

    public GUISkin skin;
    public bool invOpen = false;
    private bool pickedItemFromInv = false;
    public bool craftOpen = false;

    public Database database;
    public List<InventoryItem> items;
    public List<InventoryItem> hotbarItems;

    bool draggingItem;
    InventoryItem draggedItem;
    int previousIndex;

    public int selectedItemHotbar;
    public int curCrafting;

    public int health, maxHealth;

    public List<GameObject> tools;

    void Start()
    {
        worldCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<SingleplayerWorldCreator>();

        database = GameObject.FindGameObjectWithTag("GameController").GetComponent<Database>();

        for (int x = 0; x < items.Count; x++)
        {
            items[x] = new InventoryItem();
        }

        for (int i = 0; i < hotbarItems.Count; i++)
        {
            hotbarItems[i] = new InventoryItem();
        }

        AddItem(4, 1);
        AddItem(5, 1);
        AddItem(6, 1);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click!");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3.3f))
            {
                Debug.Log("Ray hitting something");

                if (hit.transform.gameObject.tag == "Terrain")
                {
                    Debug.Log("Player Hit Terrain");
                }
                else if (hit.transform.gameObject.tag == "SmallRock")
                {
                    Debug.Log("Hit Small Rock");
                    int amount = Random.Range(1, 10);
                    AddItem(1, amount);
                    Destroy(hit.transform.gameObject);
                }
                else if(hit.transform.gameObject.tag == "LargeRock")
                {
                    if (tools[0].active)
                    {
                        int amount = Random.Range(1, 3);
                        AddItem(1, amount);
                    }
                    if (tools[1].active)
                    {
                        int amount = Random.Range(5, 10);
                        AddItem(1, amount);
                    }
                }
                else if (hit.transform.gameObject.tag == "Tree")
                {
                    if (tools[0].active)
                    {
                        int amount = Random.Range(1, 5);
                        AddItem(2, amount);
                    }
                    else if (tools[2].active)
                    {
                        int amount = Random.Range(5, 10);
                        AddItem(2, amount);
                    }
                }
                else
                {
                    Debug.Log("hit something that wasnt important, i hit: " + hit.transform.gameObject.tag);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            craftOpen = !craftOpen;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu = !pauseMenu;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            invOpen = !invOpen;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedItemHotbar = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedItemHotbar = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedItemHotbar = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedItemHotbar = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedItemHotbar = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedItemHotbar = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedItemHotbar = 6;
        }


        if(!pauseMenu && !craftOpen && !invOpen)
        {
            controller.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (hotbarItems[selectedItemHotbar].stack > 0)
        {
            if (hotbarItems[selectedItemHotbar].item.ID == 4)
            {
                tools[0].active = true;
                tools[1].active = false;
                tools[2].active = false;
            }
            else if (hotbarItems[selectedItemHotbar].item.ID == 5)
            {
                tools[0].active = false;
                tools[1].active = true;
                tools[2].active = false;
            }
            else if (hotbarItems[selectedItemHotbar].item.ID == 6)
            {
                tools[0].active = false;
                tools[1].active = false;
                tools[2].active = true;
            }
            if(hotbarItems[selectedItemHotbar].item.iType != Item.ItemType.tools)
            {
                tools[0].active = false;
                tools[1].active = false;
                tools[2].active = false;
            }
        }
        else
        {
            tools[0].active = false;
            tools[1].active = false;
            tools[2].active = false;
        }
    }

    public void UpdateHealth(int change)
    {
        health += change;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), "");
        GUI.Box (new Rect (0, 0, 120, 30), "Health: " + health + " / " + maxHealth);

        if (pauseMenu)
        {
            controller.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GUI.BeginGroup(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 250, 250, 500), "");

            GUI.Box(new Rect(0, 0, 250, 500), "Paused");

            if (GUI.Button(new Rect(5, 30, 240, 30), "Quit"))
            {
                worldCreator.inGame = false;
                Destroy(gameObject);
            }

            GUI.EndGroup();
        }


        DrawHotBar();

        if (invOpen)
        {
            controller.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Event e = Event.current;
            DrawInventory();

            if (draggingItem)
            {
                GUI.DrawTexture(new Rect(e.mousePosition.x + 15, e.mousePosition.y, 35, 35), draggedItem.item.icon);
                GUI.Label(new Rect(e.mousePosition.x + 15, e.mousePosition.y, 35, 35), draggedItem.stack.ToString(), skin.GetStyle("CustomLabel"));
            }
        }

        if (craftOpen)
        {
            controller.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            DrawInterface();
        }

    }

    void DrawInventory()
    {
        GUI.skin = skin;

        Event e = Event.current;
        int i = 0;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 132, Screen.height / 2 - 142, 400, 285), "");

        GUI.Box(new Rect(0, 0, 265, 285), "Inventory");

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                Rect slotRect = new Rect(5 + (x * 51), 25 + (y * 51), 50, 50);

                GUI.Box(slotRect, "");

                if (items[i].stack >= 1)
                {
                    GUI.DrawTexture(slotRect, items[i].item.icon);
                    GUI.Label(new Rect(slotRect), items[i].stack.ToString(), skin.GetStyle("CustomLabel"));

                    if (slotRect.Contains(e.mousePosition))
                    {
                        GUI.Box(new Rect(265, Screen.height / 2 - 142, 150, 25), "");
                        GUI.Label(new Rect(275, Screen.height / 2 - 147, 150, 25), items[i].item.name);


                        if (e.button == 0 && e.type == EventType.MouseDrag && !draggingItem)
                        {
                            draggingItem = true;
                            draggedItem = items[i];
                            previousIndex = i;
                            items[i] = new InventoryItem();
                            pickedItemFromInv = true;
                        }

                        if (e.type == EventType.MouseUp && draggingItem)
                        {
                            
                            if (!pickedItemFromInv)
                            {
                                hotbarItems[previousIndex] = items[i];
                            }
                            else
                            {
                                items[previousIndex] = items[i];
                            }
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

                        if (e.type == EventType.MouseUp && draggingItem)
                        {
                            items[i] = draggedItem;
                            draggedItem = new InventoryItem();
                            draggingItem = false;
                        }
                    }
                }

                i++;
            }
        }

        GUI.EndGroup();
    }


    void DrawHotBar()
    {
        Event e = Event.current;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height - 65, 400, 70), "");
        GUI.Box(new Rect(0, 0, 400, 65), "");

        int i = 0;

        for (int x = 0; x < 7; x++)
        {
            Rect hotbarSlot = new Rect(5 + (x * 56), 5, 55, 55);
            Rect hotbarSlotSel = new Rect(5 + ((selectedItemHotbar) * 56), 5, 55, 5);

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
                        pickedItemFromInv = false;
                    }

                    if (e.type == EventType.MouseUp && draggingItem)
                    {
                        if (pickedItemFromInv)
                        {
                            items[previousIndex] = hotbarItems[i];
                        }
                        else
                        {
                            hotbarItems[previousIndex] = hotbarItems[i];
                        }
                        hotbarItems[i] = draggedItem;
                        draggedItem = new InventoryItem();
                        draggingItem = false;
                    }
                }
            }
            else if (hotbarItems[i].stack == 0)
            {
                if (hotbarSlot.Contains(e.mousePosition))
                {

                    if (e.type == EventType.MouseUp && draggingItem)
                    {
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
            if (database.itemDatabase[x].ID == ID)
            {
                for (int y = 0; y < items.Count; y++)
                {
                    if (items[y].item != null)
                    {
                        if (items[y].item.ID == ID)
                        {
                            items[y].stack += amount;
                            break;
                        }
                    }
                    else if (items[y].item == null)
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
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.ID == ID)
            {
                if (items[i].stack <= amount)
                {
                    items[i] = new InventoryItem();
                }
                else if (items[i].stack > amount)
                {
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

    void CheckCraft()
    {

    }

    void DrawInterface()
    {
        GUI.skin = skin;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500), "");

        GUI.Box(new Rect(0, 0, 500, 500), "Crafting");

        for (int i = 0; i < database.craftingDatabase.Count; i++)
        {
            if (GUI.Button(new Rect(5, 15 + (51 * i), 50, 50), database.itemDatabase[database.craftingDatabase[i].madeItemID].icon))
            {
                curCrafting = i;
            }

            GUI.Label(new Rect(40, 40 + (51 * i), 50, 50), database.craftingDatabase[i].amount.ToString(), skin.GetStyle("CustomLabel"));
        }

        GUI.Box(new Rect(250, 25, 240, 470), database.itemDatabase[database.craftingDatabase[curCrafting].madeItemID].name);
        GUI.Label(new Rect(255, 40, 230, 40), "Required Items for Recipe");

        for (int x = 0; x < database.craftingDatabase[curCrafting].requiredItems.Count; x++)
        {
            GUI.Box(new Rect(255, 60 + (x * 42), 230, 40), database.craftingDatabase[curCrafting].requiredItems[x].amount.ToString() + " " + database.itemDatabase[database.craftingDatabase[curCrafting].requiredItems[x].ID].name);

            if (GUI.Button(new Rect(255, 450, 230, 40), "Craft!"))
            {
                bool canCraft = false;

                foreach (CraftingRecipeItem items in database.craftingDatabase[curCrafting].requiredItems)
                {
                    if (InventoryContains(database.craftingDatabase[curCrafting].requiredItems[x].ID))
                    {
                        canCraft = true;
                    }
                }

                if (canCraft)
                {
                    AddItem(database.craftingDatabase[curCrafting].madeItemID, database.craftingDatabase[curCrafting].amount);
                    foreach (CraftingRecipeItem items in database.craftingDatabase[curCrafting].requiredItems)
                    {
                        RemoveItem(database.craftingDatabase[curCrafting].requiredItems[x].ID, database.craftingDatabase[curCrafting].requiredItems[x].amount);
                    }
                }
            }
        }

        GUI.EndGroup();
    }
}
