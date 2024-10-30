using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemConsumableName;
    public TextMeshProUGUI selectedItemConsumableValue;
    public GameObject consumeButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerInputSystem playerInputSystem;
    private Gagecontroller gagecontroller;
    public Transform dropItemTransform;

    ItemSO selectedItem;
    int selectedItemIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerInputSystem = GameManger.Instance.Player.GetComponent<PlayerInputSystem>();
        gagecontroller = GameManger.Instance.Player.GetComponent<PlayerGage>().controller;
        dropItemTransform = GameManger.Instance.Player.dropItemTransform;

        playerInputSystem.inventory += Toggle;
        GameManger.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemConsumableName.text = string.Empty;
        selectedItemConsumableValue.text = string.Empty;

        consumeButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddItem()
    {
        ItemSO data = GameManger.Instance.Player.itemData;

        if (data.stackable)
        {
            ItemSlot slot = GetitemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                GameManger.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            GameManger.Instance.Player.itemData = null;
            return;
        }

        DropItem(data);
        GameManger.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for(int i =0; i< slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].Clear();
            }
            else
            {
                slots[i].Set();
            }
        }
    }

    ItemSlot GetitemStack(ItemSO data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    void DropItem(ItemSO data)
    {
        Instantiate(data.dropPrefab, dropItemTransform.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.itemDescription;

        selectedItemConsumableName.text = string.Empty;
        selectedItemConsumableValue.text = string.Empty ;

        for (int i = 0; i < selectedItem.consumableItems.Length; i++)
        {
            selectedItemConsumableName.text += selectedItem.consumableItems[i].type.ToString()+"\n";
            selectedItemConsumableValue.text += selectedItem.consumableItems[i].value.ToString() + "\n";
        }

        consumeButton.SetActive(selectedItem.itemType == ItemType.Consumable);
        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equiped);
        unequipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equiped);
        dropButton.SetActive(true);
    }

    public void OnConsumeButton()
    {
        if(selectedItem.itemType == ItemType.Consumable)
        {
            for(int i =0; i< selectedItem.consumableItems.Length; i++)
            {
                switch (selectedItem.consumableItems[i].type)
                {
                    case ConsumableType.HP:
                        gagecontroller.HPGage.ChangeGage(selectedItem.consumableItems[i].value);
                        break;
                    case ConsumableType.Stamina:
                        gagecontroller.StaminaGage.ChangeGage(selectedItem.consumableItems[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }

    }

    public void OnDropButton()
    {
        DropItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity -= 1;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
}
