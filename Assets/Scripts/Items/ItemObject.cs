using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ItemObject : MonoBehaviour, IInteractable {
    public ItemData item;
    private ConsumableType statType;
    private ItemType itemType;


    public void Start() {
        // System.Reflection.PropertyInfo propertyInfo = StarterAssets.ThirdPersonController.instance.GetType().GetProperty("Gravity");
        // propertyInfo.GetValue(StarterAssets.ThirdPersonController.instance, null);
    }
    public void OnInteract() {
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
    public string GetInteractPrompt() {
        return string.Format("Pickup {0}", item.displayName);
    }
}

