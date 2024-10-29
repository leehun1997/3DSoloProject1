using System;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipable,
    Resource
}
public enum ConsumableType
{
    HP,
    Stamina
}

[Serializable]
public class ConsumableItem : ItemSO
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "ItemSO",menuName = "New Item") ]
public class ItemSO : ScriptableObject
{
    [Header("IDLE")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stackable")]
    public bool stackable;
    public int maxStackAmount;

    [Header("Consumable")]
    public ConsumableItem[] consumableItems;
}
