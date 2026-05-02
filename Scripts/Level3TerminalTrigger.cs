using UnityEngine;
using UnityEngine.UI;

public class Level3TerminalTrigger : MonoBehaviour
{
    [Header("Assignments")]
    public GameObject mainCanvas;
    public VirusShieldLevelManager levelManager;
    public PlayerController playerController;

    [Header("Interaction Notification")]
    public GameObject interactionPrompt;
    [TextArea] public string promptMessage = "Press SPACE or E to scan system";

    private bool playerInRange = false;
    private bool isTaskActive = false;

    void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        // Try dynamically grabbing the interaction prompt if none manually assigned
        if (interactionPrompt == null)
        {
            GameObject controlsUI = GameObject.Find("Controls UI Builder");
            if (controlsUI != null && controlsUI.transform.childCount > 0) 
            {
                interactionPrompt = controlsUI.transform.GetChild(0).gameObject;
            }
        }

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isTaskActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)))
        {
            StartSystemScan();
        }
    }

    void StartSystemScan()
    {
        isTaskActive = true;
        
        if (playerController != null)
        {
            playerController.enabled = false;
            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
        }

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
            
        if (levelManager != null)
            levelManager.StartLevelTask();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTaskActive)
        {
            playerInRange = true;
            if (interactionPrompt != null)
            {
                Text txt = interactionPrompt.GetComponentInChildren<Text>();
                if (txt != null) txt.text = promptMessage;
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
}
