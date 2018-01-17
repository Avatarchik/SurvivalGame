using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CraftingRecipe
{
	public List<CraftingRecipeItem> requiredItems;
	public int madeItemID;
	public int amount;
}
