
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private RectTransform movementArea;
    [SerializeField] private float followSpeed = 12f;

    private RectTransform cloudRect;
    private bool canMove = true;

    private void Awake()
    {
        cloudRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!canMove || movementArea == null || Mouse.current == null)
            return;

        Camera uiCamera = null;
        Canvas canvas = movementArea.GetComponentInParent<Canvas>();

        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            uiCamera = canvas.worldCamera;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            movementArea, mousePosition, uiCamera, out Vector2 localPoint))
            return;

        float halfHeight = movementArea.rect.height * 0.5f;
        float cloudHalfHeight = cloudRect.rect.height *
                                cloudRect.localScale.y * 0.5f;

        float minY = -halfHeight + cloudHalfHeight;
        float maxY = halfHeight - cloudHalfHeight;

        Vector2 position = cloudRect.anchoredPosition;
        position.y = Mathf.Clamp(localPoint.y, minY, maxY);

        cloudRect.anchoredPosition = Vector2.Lerp(
            cloudRect.anchoredPosition,
            position,
            followSpeed * Time.deltaTime
        );
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void MoveToCenter(float duration)
    {
        canMove = false;
        StartCoroutine(MoveToCenterRoutine(duration));
    }


    private IEnumerator MoveToCenterRoutine(float duration)
    {
        Vector2 startPosition = cloudRect.anchoredPosition;
        Vector2 targetPosition = movementArea.rect.center;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(
                0f,
                1f,
                Mathf.Clamp01(elapsed / duration)
            );

            cloudRect.anchoredPosition = Vector2.Lerp(
                startPosition,
                targetPosition,
                t
            );

            yield return null;
        }

        cloudRect.anchoredPosition = targetPosition;
    }
}
