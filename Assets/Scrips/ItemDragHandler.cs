using System.Drawing;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform origionalParent;
    CanvasGroup canvasGroup;
    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        origionalParent = transform.parent; //save parent
        transform.SetParent(transform.root); //above other canvass
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; //enables raycasts
        canvasGroup.alpha = 1f; // not opake

        ItemSlot dropslot = eventData.pointerEnter?.GetComponent<ItemSlot>(); // slot dropped in
        if (dropslot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropslot = dropItem.GetComponentInParent<ItemSlot>();
            }
        }
        ItemSlot originalSlot = origionalParent.GetComponent<ItemSlot>();


        if (dropslot != null)
        {
            // is a slot under drop point
            if (dropslot.currentItem != null)
            {
                //slot has item so swap items
                dropslot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropslot.currentItem;
                dropslot.currentItem.GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            //move iteminto dropslot
            transform.SetParent(dropslot.transform);
            dropslot.currentItem = gameObject;
        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                //drop 
                DropItem(originalSlot);
            }
            else
            {
                //no slop under drop point
                transform.SetParent(origionalParent);
            }

        }

        GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
    }

    bool IsWithinInventory(UnityEngine.Vector2 mousePosition)
    {
        RectTransform inventoryRect = origionalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(ItemSlot origionalSlot)
    {
        origionalSlot.currentItem = null;
        //find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' Tag");
            return;
        }

        //random drop pos
        UnityEngine.Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        UnityEngine.Vector2 dropPosition = (UnityEngine.Vector2)playerTransform.position + dropOffset;

        //instantiate gam object
        Instantiate(gameObject, dropPosition, UnityEngine.Quaternion.identity);

        //destroy the ui one
        Destroy(gameObject);
        
    }
}
