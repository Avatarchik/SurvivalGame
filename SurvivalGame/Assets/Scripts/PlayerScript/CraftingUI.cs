using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class CraftingUI : MonoBehaviour {

	public GUISkin skin;

	bool isOpen = false;

	public FirstPersonController controller;
	public PlayerManager manager;
	public Database data;
	public Inventory inv;

	public int curCrafting;

	void Start ()
	{
        data = GameObject.FindGameObjectWithTag("GameController").GetComponent<Database>();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C))
		{
			isOpen = !isOpen;
		}
	}

	void OnGUI()
	{
		if (isOpen) {
			controller.enabled = false;
			/*Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;*/

			DrawInterface ();
		} else {
			//controller.enabled = true;
		}
	}

	void CheckCraft()
	{

	}

	void DrawInterface()
	{
		GUI.skin = skin;

		GUI.BeginGroup (new Rect (Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500), "");

		GUI.Box(new Rect(0, 0, 500, 500), "Crafting");

		for (int i = 0; i < data.craftingDatabase.Count; i++)
		{
			if(GUI.Button(new Rect(5, 15 + (51 * i), 50, 50), data.itemDatabase[data.craftingDatabase[i].madeItemID].icon))
			{
				curCrafting = i;
			}

			GUI.Label(new Rect(40, 40 + (51 * i), 50, 50), data.craftingDatabase[i].amount.ToString(), skin.GetStyle("CustomLabel"));
		}

		GUI.Box (new Rect (250, 25, 240, 470), data.itemDatabase[data.craftingDatabase[curCrafting].madeItemID].name);
		GUI.Label(new Rect(255, 40, 230, 40), "Required Items for Recipe");

		for (int x = 0; x < data.craftingDatabase[curCrafting].requiredItems.Count; x++) 
		{
			GUI.Box (new Rect (255, 60 + (x * 42), 230, 40), data.craftingDatabase[curCrafting].requiredItems[x].amount.ToString() + " " + data.itemDatabase [data.craftingDatabase [curCrafting].requiredItems [x].ID].name);

			if (GUI.Button (new Rect (255, 450, 230, 40), "Craft!"))
			{
				bool canCraft = false;

				foreach(CraftingRecipeItem items in data.craftingDatabase[curCrafting].requiredItems)
				{
					if(inv.InventoryContains(data.craftingDatabase[curCrafting].requiredItems[x].ID))
					{
						canCraft = true;
					}
				}

					if(canCraft)
					{
						inv.AddItem(data.craftingDatabase[curCrafting].madeItemID, data.craftingDatabase[curCrafting].amount);
						foreach(CraftingRecipeItem items in data.craftingDatabase[curCrafting].requiredItems)
						{
							inv.RemoveItem(data.craftingDatabase[curCrafting].requiredItems[x].ID, data.craftingDatabase[curCrafting].requiredItems[x].amount);
						}
					}
				}
			}

		GUI.EndGroup ();
	}
}
