using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using StarterAssets;
public class Inventory : MonoBehaviour {
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;
    private int curEquipIndex;

    //components
    private PlayerController controller;
    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;
    private StarterAssetsInputs starterAssetsInputs;

    //singleton
    public static Inventory instance;

    void Awake() {
        instance = this;
        controller = GetComponent<PlayerController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }
    void Start() {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];
        // init slots
        for (int x = 0; x < slots.Length; x++) {
            slots[x] = new ItemSlot();
            uiSlots[x].index = x;
            uiSlots[x].Clear();
        }
        ClearSelectedItemWindow();
    }
    private void Update() {
        if (starterAssetsInputs.inventory) {
            Toggle();
            starterAssetsInputs.inventory = false;
        }
    }
    public void Toggle() {
        if (inventoryWindow.activeInHierarchy) {
            inventoryWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            starterAssetsInputs.cursorInputForLook = true;
            onCloseInventory.Invoke();
        } else {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            Cursor.lockState = CursorLockMode.None;
            starterAssetsInputs.cursorInputForLook = false;
        }
    }
    public bool IsOpen() {
        return inventoryWindow.activeInHierarchy;
    }
    // adds the requested item to the player's inventory
    public void AddItem(ItemData item) {
        // does this item have a stack it can be added to?
        if (item.canStack) {
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null) {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();
        // do we have an empty slot for the item?
        if (emptySlot != null) {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }
        // if the item can't stack and there are no empty slots - throw it away
        ThrowItem(item);
    }

    // spawn in front of player
    void ThrowItem(ItemData item) {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }
    // if no empty slots return null
    ItemSlot GetEmptySlot() {
        for (int x = 0; x < slots.Length; x++) {
            if (slots[x].item == null) {
                return slots[x];
            }
        }
        return null;
    }
    // update ui slots
    void UpdateUI() {
        for (int x = 0; x < slots.Length; x++) {
            if (slots[x].item != null)
                uiSlots[x].Set(slots[x]);
            else
                uiSlots[x].Clear();
        }
    }
    ItemSlot GetItemStack(ItemData item) {
        for (int x = 0; x < slots.Length; x++) {
            if (slots[x].item == item && slots[x].quantity < item.maxStacks)
                return slots[x];
        }
        return null;
    }
    public void SelectItem(int index) {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        // getting each field in the inventory view
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // adding in each consumable type into the fields
        foreach (ItemDataConsumable itemDataConsumable in selectedItem.item.consumable) {
            selectedItemStatNames.text += itemDataConsumable.type.ToString() + "\n";
            selectedItemStatValues.text += itemDataConsumable.value.ToString() + "\n";
        }
        // set stat values and stat names
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }
    void ClearSelectedItemWindow() {
        // clear the text elements
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // disable buttons
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    public void OnUseButton() {
        if (selectedItem.item.type.Equals(ItemType.Consumable)) {
            foreach (var stat in selectedItem.item.consumable) {
                StarterAssets.ThirdPersonController.instance.statsData.stats[stat.type.ToString()] += stat.value;
            }
            RemoveSelectedItem();
        }
    }
    public void OnEquipButton() {

    }
    void UnEquip(int index) {

    }
    public void OnUnEquipButton() {

    }
    public void OnDropButton() {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }
    void RemoveSelectedItem() {
        selectedItem.quantity--;
        if (selectedItem.quantity >= 0) {
            if (uiSlots[selectedItemIndex].equipped == true) {
                UnEquip(selectedItemIndex);
            }
            selectedItem.item = null;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }
    void RemoveItem(ItemData item) {

    }
    public bool HasItems(ItemData item, int quantity) {
        return false;
    }
}


public class ItemSlot {
    public ItemData item;
    public int quantity;
}
