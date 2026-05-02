using UnityEngine;
using UnityEngine.UI;

public class EmailTerminalInteractor : MonoBehaviour
{
    [Header("Level Manager")]
    public EmailPhishingLevelManager phishingLevelManager;

    [Header("Player Tracking")]
    public string playerTag = "Player";
    
    [Header("Interaction UI")]
    public GameObject interactionPrompt; // e.g. A Canvas or 3D text showing "Press E"
    
    [Header("Animation (Optional)")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.3f;

    private bool isPlayerNear = false;
    private bool levelStarted = false;
    private Vector3 startPos;
    private PlayerController playerMovement;

    void Start()
    {
        startPos = transform.position;
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // Auto-find Manager if not set
        if (phishingLevelManager == null)
        {
            phishingLevelManager = FindObjectOfType<EmailPhishingLevelManager>();
        }
    }

    void Update()
    {
        // Floating animation for the prompt/icon if needed
        if (!levelStarted)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        if (isPlayerNear && !levelStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartEmailLevel();
        }
    }

    private void StartEmailLevel()
    {
        if (phishingLevelManager == null)
        {
            Debug.LogError("EmailTerminalInteractor: Phishing Level Manager is not assigned!");
            return;
        }

        levelStarted = true;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // If you have a player controller, freeze it here. 
        if (playerMovement != null)
        {
            playerMovement.StopMovement();
        }

        // Forcefully ensure EmailCanvas is enabled if we had one here (alternatively we rely on phishingLevelManager)
        phishingLevelManager.gameObject.SetActive(true);

        // Trigger the Level Manager
        phishingLevelManager.StartEmailSession();

        // Optionally disable this script so it can't be triggered again
        this.enabled = false; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelStarted) return;

        if (collision.CompareTag(playerTag))
        {
            isPlayerNear = true;
            playerMovement = collision.GetComponent<PlayerController>();
            
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isPlayerNear = false;
            playerMovement = null;
            
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
}
