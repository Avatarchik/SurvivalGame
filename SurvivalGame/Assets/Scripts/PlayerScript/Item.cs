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
        junk
    }

    public ItemType iType;

    public GameObject toolObject;
}
