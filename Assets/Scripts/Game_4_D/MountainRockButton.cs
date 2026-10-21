
using UnityEngine;
using UnityEngine.UI;

public class MountainRockButton : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private Button button;
    [SerializeField] private int orderNumber;

    private MountainWaterGameManager gameManager;

    public int OrderNumber => orderNumber;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void Initialize(MountainWaterGameManager manager)
    {
        gameManager = manager;

        button.onClick.RemoveListener(OnClicked);
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (gameManager != null)
            gameManager.TryRemoveRock(this);
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("RockBreak");
    }

    public void Remove()
    {
        button.interactable = false;
        gameObject.SetActive(false);
    }

    public void SetInteractable(bool value)
    {
        if (button != null)
            button.interactable = value;
    }
}
