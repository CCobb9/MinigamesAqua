
using UnityEngine;
using UnityEngine.UI;

public class CloudTarget : MonoBehaviour
{
    [SerializeField] private Button button;

    private GameManager4 gameManager;
    private bool clicked;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void Initialize(GameManager4 manager)
    {
        gameManager = manager;

        button.onClick.RemoveListener(OnClicked);
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (clicked || gameManager == null)
            return;

        clicked = true;
        gameManager.RemoveCloud(this);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
