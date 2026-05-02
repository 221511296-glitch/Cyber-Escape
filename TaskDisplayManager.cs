using UnityEngine;
using UnityEngine.UI;

public class TaskDisplayManager : MonoBehaviour
{
    [Header("UI")]
    public Text taskDisplayText;

    [Header("References")]
    public GameObject floatingObject;        // The floating trigger object
    public GameObject identifyPanel;         // The Identify Panel to enable

    GameObject FindSceneObjectByName(string objectName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == objectName && obj.hideFlags == HideFlags.None && obj.scene.isLoaded)
            {
                return obj;
            }
        }
        return null;
    }

    void Start()
    {
        // Initialize
        if (taskDisplayText != null)
            taskDisplayText.text = "Go to the mentor for guidance";

        // Auto-find the Identify Panel if it became null due to variable rename
        if (identifyPanel == null)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "Identify Panel" || obj.name == "IdentifyPanel")
                {
                    // Ensure it's part of the scene, not a prefab
                    if (obj.hideFlags == HideFlags.None && obj.scene.isLoaded)
                    {
                        identifyPanel = obj;
                        break;
                    }
                }
            }
        }

        // Prioritize finding the new "Card for starting the task" if it exists
        GameObject cardObj = FindSceneObjectByName("Card for starting the task");
        if (cardObj != null)
        {
            floatingObject = cardObj;
        }

        // Auto-find the floating training object if not assigned
        if (floatingObject == null)
        {
            FloatingTriggerObject trigger = FindObjectOfType<FloatingTriggerObject>(true);
            if (trigger != null)
                floatingObject = trigger.gameObject;
            else
                floatingObject = GameObject.Find("PASSWORD TRAINING");
        }

        if (floatingObject != null)
            floatingObject.SetActive(false);

        if (identifyPanel != null)
            identifyPanel.SetActive(false);
    }

    // Called by MentorInteractionManager when mentor interaction starts
    public void OnMentorInteractionStart()
    {
        if (taskDisplayText != null)
            taskDisplayText.text = "Listen to the mentor...";
    }

    // Called by PasswordTrainingIntroManager when all training screens are complete
    public void OnTrainingComplete()
    {
        if (taskDisplayText != null)
            taskDisplayText.text = "Find the floating object for the next task";

        GameObject cardObj = FindSceneObjectByName("Card for starting the task");
        if (cardObj != null)
        {
            floatingObject = cardObj;
        }

        if (floatingObject != null)
        {
            floatingObject.SetActive(true);
            Debug.Log("Floating object enabled - go interact with it!");
        }
        else
        {
            Debug.LogWarning("TaskDisplayManager: Could not find a floating object to enable after training completion.");
        }
    }

    // Called by FloatingTriggerObject when player interacts with it
    public void OnFloatingObjectInteraction()
    {
        if (taskDisplayText != null)
            taskDisplayText.text = "Complete the password security simulation...";

        if (identifyPanel != null)
        {
            identifyPanel.SetActive(true);
            Debug.Log("TaskDisplayManager: Identify Panel enabled!");
        }
        else
        {
            Debug.LogError("TaskDisplayManager: Identify panel is STILL null! Could not enable it.");
        }

        if (floatingObject != null)
            floatingObject.SetActive(false);
    }
}
