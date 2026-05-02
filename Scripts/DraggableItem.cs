using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private bool isSafe = true;
    [SerializeField] private string itemName = "File";
    [SerializeField] private string explanation = "This file is safe to open.";
    [SerializeField] private Image itemImage;
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image image;
    private Vector3 originalPosition;
    private Transform originalParent;
    private DropZone currentHighlightedZone;
    private float dragAlpha = 0.7f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        originalPosition = rectTransform.localPosition;
        originalParent = transform.parent;

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public bool IsSafe() => isSafe;
    public string GetItemName() => itemName;
    public string GetExplanation() => explanation;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Make item semi-transparent while dragging
        if (canvasGroup != null)
        {
            canvasGroup.alpha = dragAlpha;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;

        // Move item accurate to any Canvas scaling
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos);
        rectTransform.position = globalMousePos;

        // Check if hovering over drop zones
        DropZone hoveredZone = GetHoveredDropZone(eventData.position);
        
        if (hoveredZone != null && hoveredZone != currentHighlightedZone)
        {
            if (currentHighlightedZone != null)
            {
                currentHighlightedZone.UnhighlightZone();
            }
            hoveredZone.HighlightZone();
            currentHighlightedZone = hoveredZone;
        }
        else if (hoveredZone == null && currentHighlightedZone != null)
        {
            currentHighlightedZone.UnhighlightZone();
            currentHighlightedZone = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore opacity
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        // Check if dropped on a valid zone
        DropZone dropZone = GetDropZoneAtPosition(eventData.position);

        if (dropZone != null)
        {
            dropZone.OnDrop(this);
            // Disable item after drop (will be handled by ItemQueueManager)
            gameObject.SetActive(false);
        }
        else
        {
            // Return to original position if not dropped on a zone
            rectTransform.localPosition = originalPosition;
        }

        // Unhighlight
        if (currentHighlightedZone != null)
        {
            currentHighlightedZone.UnhighlightZone();
            currentHighlightedZone = null;
        }
    }

    private DropZone GetDropZoneAtPosition(Vector3 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(position.x, position.y)
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            DropZone zone = result.gameObject.GetComponent<DropZone>();
            if (zone != null)
                return zone;
        }

        return null;
    }

    private DropZone GetHoveredDropZone(Vector3 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(position.x, position.y)
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            DropZone zone = result.gameObject.GetComponent<DropZone>();
            if (zone != null)
                return zone;
        }

        return null;
    }

    public void ResetPosition()
    {
        rectTransform.localPosition = originalPosition;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
