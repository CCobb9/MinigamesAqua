
using System.Collections;
using UnityEngine;

public class RepairAnimationController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string repairStateName = "llave_fix";
    [SerializeField] private string repairTriggerName = "Repair";

    [Header("Game")]
    [SerializeField] private GameManager2 gameManager;

    private Coroutine repairCoroutine;
    private bool isPlaying;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void PlayAt(Vector3 position)
    {
        if (isPlaying)
            return;

        transform.position = position;
        gameObject.SetActive(true);

        animator.ResetTrigger(repairTriggerName);
        animator.SetTrigger(repairTriggerName);

        isPlaying = true;
        repairCoroutine = StartCoroutine(
            WaitForRepairAnimation()
        );
    }

    private IEnumerator WaitForRepairAnimation()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0)
                       .IsName(repairStateName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0)
                       .IsName(repairStateName) &&
               animator.GetCurrentAnimatorStateInfo(0)
                       .normalizedTime < 1f)
        {
            yield return null;
        }

        repairCoroutine = null;
        isPlaying = false;

        gameManager.CompleteRepairAnimation();
    }

    public void Hide()
    {
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }

        isPlaying = false;
        gameObject.SetActive(false);
    }
}
