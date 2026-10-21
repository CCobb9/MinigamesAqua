
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CloudTarget : MonoBehaviour
{
    private const string CloudClickSound = "CloudClick";

    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private Animator animator;

    [Header("Animation")]
    [SerializeField] private float destroyDelay = 0.5f;

    private GameManager4 gameManager;
    private bool clicked;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Initialize(GameManager4 manager)
    {
        gameManager = manager;

        if (button != null)
        {
            button.onClick.RemoveListener(OnClicked);
            button.onClick.AddListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        if (clicked || gameManager == null)
            return;

        clicked = true;

        if (button != null)
            button.interactable = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(CloudClickSound);

        if (animator != null)
            animator.enabled = true;

        gameManager.RemoveCloud(this);

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
