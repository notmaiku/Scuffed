using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Equipable,
    Resource,
    Consumable
}

public enum ConsumableType{
    Hunger,
    Thirst,
    Health,
    MoveSpeed
}
[CreateAssetMenu(fileName = "Item", menuName ="NewItem")]
public class ItemData : ScriptableObject {
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStacks;

    [Header("Consumable")]
    public ItemDataConsumable[] consumable;
}

[System.Serializable]
public class ItemDataConsumable{
    public ConsumableType type;
    public float value;
}
