
using UnityEngine;

public class Airplane : MonoBehaviour
{
    [SerializeField] private float speed = 500f;

    private RectTransform airplaneRect;
    private RectTransform movementArea;

    private void Awake()
    {
        airplaneRect = GetComponent<RectTransform>();
    }

    public void Initialize(RectTransform area, float movementSpeed)
    {
        movementArea = area;
        speed = movementSpeed;
    }

    private void Update()
    {
        if (movementArea == null)
            return;

        airplaneRect.anchoredPosition +=
            Vector2.left * speed * Time.deltaTime;

        Vector3[] corners = new Vector3[4];
        airplaneRect.GetWorldCorners(corners);

        Vector3 rightmostPoint = corners[2];

        if (rightmostPoint.x < movementArea.position.x -
            movementArea.rect.width * movementArea.lossyScale.x / 2f)
        {
            Destroy(gameObject);
        }
    }
}
