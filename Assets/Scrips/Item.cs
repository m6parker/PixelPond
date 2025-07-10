using UnityEngine;
// using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    internal int quantity = 1;
    //int tmp text = quantityText

    private void Awake()
    {
        //get, set quantity text
        //updateQuantityDisplay()
    }
    // public void UpdateQuantityDisplay()
    // {
    //     quantityText
    // }
    public virtual void useItem()
    {
        Debug.Log("using " + Name);
    }

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<UnityEngine.UI.Image>().sprite;
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        // UpdateQuantityDisplay
        return removed;
    }
}
