
using UnityEngine;
using UnityEngine.UI;

public class SequenceButton : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button button;
    [SerializeField] private int orderNumber;

    [Header("Indicators")]
    [SerializeField] private GameObject correctIndicator;
    [SerializeField] private GameObject wrongIndicator;

    private GameManager3 gameManager;

    public int OrderNumber => orderNumber;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        ClearIndicators();
    }

    public void Initialize(GameManager3 manager)
    {
        gameManager = manager;

        button.onClick.RemoveListener(OnButtonClicked);
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (gameManager != null)
            gameManager.SelectButton(this);
    }

    public void ShowCorrect()
    {
        correctIndicator.SetActive(true);
        wrongIndicator.SetActive(false);
    }

    public void ShowWrong()
    {
        correctIndicator.SetActive(false);
        wrongIndicator.SetActive(true);
    }

    public void ClearWrongIndicator()
    {
        wrongIndicator.SetActive(false);
    }

    public void ClearIndicators()
    {
        if (correctIndicator != null)
            correctIndicator.SetActive(false);

        if (wrongIndicator != null)
            wrongIndicator.SetActive(false);
    }

    public void SetInteractable(bool interactable)
    {
        if (button != null)
            button.interactable = interactable;
    }
}
