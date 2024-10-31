using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO ItemData;
    private void Start()
    {
        
    }
    public string GetInteractPrompt()
    {
        string str = $"{ItemData.itemName}\n{ItemData.itemDescription}";
        return str;
    }

    public void OnInteract()
    {
        GameManger.Instance.Player.itemData = ItemData;
        GameManger.Instance.Player.addItem?.Invoke();

        Destroy(gameObject);
    }
}
