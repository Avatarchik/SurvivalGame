using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
	public string name;
	public int ID;
	public Texture2D icon;

    public enum ItemType
    {
        resource,
        building,
        tools,
        consumable,
        junk
    }

    public ItemType iType;

    public int thirstReplenish;
    public int hungerReplenish;
}
