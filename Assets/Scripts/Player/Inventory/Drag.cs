using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class Drag : MonoBehaviour
{
    private Canvas canvas;
    
    private Vector3 originalPosition;

    private Image gunImage;

    private InventoryController inventory;

    private bool isMouseOver;

    private bool isDragging;
    
    private void Start()
    {
        originalPosition = transform.position;
        gunImage = GetComponent<Image>();
        
        UIInventoryData temp = GetComponentInParent<UIInventoryData>();
        canvas = temp.playerUI;
        inventory = temp.inventory;
    }

    public void DragHandler(BaseEventData data)
    {
        gunImage.raycastTarget = false;
        
        PointerEventData pointerData = (PointerEventData)data;
    
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform) canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position
        );

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void DropHandler(BaseEventData data)
    {
        inventory.SwitchGunSlotWeapon(gunImage);

        transform.position = originalPosition;
        
        gunImage.raycastTarget = true;
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        isMouseOver = true;
    }
    
    public void OnPointerExit(BaseEventData eventData)
    {
        isMouseOver = false;
    }

    public bool IsMouseOver()
    {
        return isMouseOver;
    }
}
