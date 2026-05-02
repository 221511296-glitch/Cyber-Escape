using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemQueueManager : MonoBehaviour
{
    [System.Serializable]
    public class VirusItem
    {
        public string itemName = "file.exe";
        public bool isSafe = false;
        public string explanation = "This file appears to be malware.";
        public Sprite icon;
    }

    [SerializeField] private List<VirusItem> itemDatabase = new List<VirusItem>();
    [SerializeField] private GameObject draggableItemPrefab;
    [SerializeField] private Transform spawnContainer;
    [SerializeField] private Vector2 spawnPosition = Vector2.zero;
    [SerializeField] private float spawnDelay = 0.5f;

    private Queue<VirusItem> itemQueue = new Queue<VirusItem>();
    private DropZone safeZone;
    private DropZone dangerZone;
    private System.Action<bool, string, string> onItemDroppedCallback;
    private int itemsPerLevel = 10;
    private int currentItemIndex = 0;
    private bool isSpawning = false;
    private DraggableItem currentDraggableItem;

    public void Initialize(int itemCount, DropZone safe, DropZone danger, System.Action<bool, string, string> callback)
    {
        itemsPerLevel = itemCount;
        safeZone = safe;
        dangerZone = danger;
        onItemDroppedCallback = callback;

        // Subscribe to drop zone events
        if (safeZone != null)
        {
            safeZone.ItemDropped += OnItemDropped;
        }
        if (dangerZone != null)
        {
            dangerZone.ItemDropped += OnItemDropped;
        }

        // Populate queue with random items
        PopulateItemQueue();
    }

    private void PopulateItemQueue()
    {
        itemQueue.Clear();
        List<VirusItem> selectedItems = new List<VirusItem>();

        // Randomly select items from database
        for (int i = 0; i < itemsPerLevel && i < itemDatabase.Count; i++)
        {
            int randomIndex = Random.Range(0, itemDatabase.Count);
            selectedItems.Add(itemDatabase[randomIndex]);
        }

        // If not enough items, shuffle and repeat
        while (selectedItems.Count < itemsPerLevel && itemDatabase.Count > 0)
        {
            int randomIndex = Random.Range(0, itemDatabase.Count);
            selectedItems.Add(itemDatabase[randomIndex]);
        }

        foreach (var item in selectedItems)
        {
            itemQueue.Enqueue(item);
        }
    }

    public void StartSpawningItems(float delay = 0.5f)
    {
        spawnDelay = delay;
        StartCoroutine(SpawnInitialItem());
    }

    private IEnumerator SpawnInitialItem()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnDelay);
        SpawnNextItem();
        isSpawning = false;
    }

    public void TriggerNextSpawn()
    {
        if (itemQueue.Count > 0)
        {
            StartCoroutine(SpawnInitialItem());
        }
        else
        {
            VirusShieldLevelManager mgr = FindObjectOfType<VirusShieldLevelManager>();
            if (mgr != null && mgr.IsGameActive()) 
            {
                mgr.LevelComplete();
            }
        }
    }

    private void SpawnNextItem()
    {
        if (itemQueue.Count == 0) return;

        VirusItem item = itemQueue.Dequeue();
        currentItemIndex++;

        // Destroy previous item if exists
        if (currentDraggableItem != null)
        {
            Destroy(currentDraggableItem.gameObject);
        }

        // Create new draggable item
        if (draggableItemPrefab != null)
        {
            GameObject newItemObj = Instantiate(draggableItemPrefab, spawnContainer);
            RectTransform rect = newItemObj.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localPosition = spawnPosition;
            }

            DraggableItem draggableItem = newItemObj.GetComponent<DraggableItem>();
            if (draggableItem != null)
            {
                currentDraggableItem = draggableItem;
                // Configure the item (you'll need to add a public method for this in DraggableItem)
                ConfigureDraggableItem(draggableItem, item);
            }

            newItemObj.SetActive(true);
        }
    }

    private void ConfigureDraggableItem(DraggableItem draggableItem, VirusItem virusItem)
    {
        // Use reflection or add a public method to configure
        // For now, we'll access the private fields through a helper method
        var type = draggableItem.GetType();
        
        var isSafeField = type.GetField("isSafe", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var itemNameField = type.GetField("itemName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var explanationField = type.GetField("explanation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (isSafeField != null) isSafeField.SetValue(draggableItem, virusItem.isSafe);
        if (itemNameField != null) itemNameField.SetValue(draggableItem, virusItem.itemName);
        if (explanationField != null) explanationField.SetValue(draggableItem, virusItem.explanation);

        // Set icon if image component exists
        Image image = draggableItem.GetComponent<Image>();
        if (image != null && virusItem.icon != null)
        {
            image.sprite = virusItem.icon;
        }

        // Dynamically visually update the UI label so the player can verify the file name!
        Text labelText = draggableItem.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.text = virusItem.itemName;
        }
    }

    private void OnItemDropped(DraggableItem item, bool isCorrect)
    {
        if (item == null) return;

        bool itemIsSafe = item.IsSafe();
        string itemName = item.GetItemName();
        string explanation = item.GetExplanation();

        onItemDroppedCallback?.Invoke(isCorrect, itemName, explanation);

        TriggerNextSpawn();
    }

    public int GetCurrentItemIndex()
    {
        return currentItemIndex;
    }

    public int GetTotalItems()
    {
        return itemsPerLevel;
    }

    public bool IsSpawning()
    {
        return isSpawning;
    }

    public void AddItemToDatabase(VirusItem item)
    {
        itemDatabase.Add(item);
    }

    public void ClearDatabase()
    {
        itemDatabase.Clear();
    }

    private void OnDestroy()
    {
        if (safeZone != null)
        {
            safeZone.ItemDropped -= OnItemDropped;
        }
        if (dangerZone != null)
        {
            dangerZone.ItemDropped -= OnItemDropped;
        }
    }
}
