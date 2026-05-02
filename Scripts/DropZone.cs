using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropTarget
{
    [SerializeField] private bool isSafeZone = true; // True = Safe to Open, False = Scan or Delete
    [SerializeField] private Image zoneHighlight;
    [SerializeField] private Color highlightColor = Color.yellow;
    private Color originalColor;
    private bool isHighlighted = false;

    // Event to notify when an item is dropped
    public delegate void OnItemDropped(DraggableItem item, bool isCorrect);
    public event OnItemDropped ItemDropped;

    private void Start()
    {
        if (zoneHighlight != null)
        {
            originalColor = zoneHighlight.color;
        }
    }

    public bool IsSafeZone() => isSafeZone;

    public void HighlightZone()
    {
        if (zoneHighlight != null && !isHighlighted)
        {
            isHighlighted = true;
            zoneHighlight.color = highlightColor;
        }
    }

    public void UnhighlightZone()
    {
        if (zoneHighlight != null && isHighlighted)
        {
            isHighlighted = false;
            zoneHighlight.color = originalColor;
        }
    }

    public void OnDrop(DraggableItem item)
    {
        if (item == null) return;

        // Determine if the drop is correct
        bool isCorrect = (isSafeZone && item.IsSafe()) || (!isSafeZone && !item.IsSafe());

        // Get explanation/feedback
        string explanation = item.GetExplanation();

        // Fire the event
        ItemDropped?.Invoke(item, isCorrect);

        // Reset zone highlight
        UnhighlightZone();
    }

    public Transform GetDropPosition()
    {
        return this.transform;
    }
}

public interface IDropTarget
{
    void OnDrop(DraggableItem item);
    Transform GetDropPosition();
    bool IsSafeZone();
}
