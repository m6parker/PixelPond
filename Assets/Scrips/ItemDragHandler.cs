using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform origionalParent;
    CanvasGroup canvasGroup;

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
                dropslot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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
            //no slop under drop point
            transform.SetParent(origionalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;


    }

}
