using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirusAnimationController : MonoBehaviour
{
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float spreaderSpeed = 1f;
    [SerializeField] private RectTransform animationContainer;
    [SerializeField] private Image virusImage;
    [SerializeField] private bool destroyAfterAnimation = true;

    private RectTransform rectTransform;
    private Vector3 startPosition;
    private float elapsedTime = 0f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            startPosition = rectTransform.localPosition;
        }

        StartCoroutine(PlayVirusAnimation());
    }

    private IEnumerator PlayVirusAnimation()
    {
        // Virus spreads across screen
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime * spreaderSpeed;
            
            if (rectTransform != null)
            {
                // Move from left to right
                float moveDistance = Mathf.Lerp(-500f, 500f, elapsedTime / animationDuration);
                rectTransform.localPosition = startPosition + new Vector3(moveDistance, 0, 0);
            }

            if (virusImage != null)
            {
                // Change opacity
                Color color = virusImage.color;
                color.a = Mathf.Lerp(1f, 0.3f, elapsedTime / animationDuration);
                virusImage.color = color;
            }

            yield return null;
        }

        if (destroyAfterAnimation)
        {
            Destroy(gameObject);
        }
    }

    public void SetAnimationSpeed(float speed)
    {
        spreaderSpeed = speed;
    }

    public void SetAnimationDuration(float duration)
    {
        animationDuration = duration;
    }
}
