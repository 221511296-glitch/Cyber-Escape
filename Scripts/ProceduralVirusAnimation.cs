using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProceduralVirusAnimation : MonoBehaviour
{
    [Header("Settings")]
    public float scaleSpeed = 5f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    public float rotationSpeed = 50f;
    
    [Header("Color Flashing (Requires Image/Text)")]
    public float flashSpeed = 10f;
    public Color flashColor1 = Color.red;
    public Color flashColor2 = Color.yellow;

    private Graphic graphicComponent;

    void Start()
    {
        // Try to get an Image or Text component to flash its colors
        graphicComponent = GetComponent<Graphic>();
    }

    void Update()
    {
        // 1. Pulsing Scale effect
        float pingPong = Mathf.PingPong(Time.time * scaleSpeed, 1f);
        float scaleValue = Mathf.Lerp(minScale, maxScale, pingPong);
        transform.localScale = new Vector3(scaleValue, scaleValue, 1f);

        // 2. Continuous Rotation effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 3. Color Flashing effect
        if (graphicComponent != null)
        {
            float colorPingPong = Mathf.PingPong(Time.time * flashSpeed, 1f);
            graphicComponent.color = Color.Lerp(flashColor1, flashColor2, colorPingPong);
        }
    }
}
