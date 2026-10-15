
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar2 : MonoBehaviour
{
    [SerializeField] private Image waterBar;
    [SerializeField] private float fillDuration = 10f;

    private Coroutine progressCoroutine;
    private GameManager2 gameManager;
    private int activeLeaks;

    public void Initialize(GameManager2 manager)
    {
        gameManager = manager;
    }

    private void Start()
    {
        waterBar.fillAmount = 0f;
    }

    public void SetActiveLeaks(int count)
    {
        activeLeaks = Mathf.Max(0, count);

        if (activeLeaks > 0 && progressCoroutine == null)
        {
            progressCoroutine = StartCoroutine(ProgressRoutine());
        }
        else if (activeLeaks == 0 && progressCoroutine != null)
        {
            StopCoroutine(progressCoroutine);
            progressCoroutine = null;
        }
    }

    private IEnumerator ProgressRoutine()
    {
        while (waterBar.fillAmount < 1f && activeLeaks > 0)
        {
            waterBar.fillAmount +=
                Time.deltaTime * activeLeaks / fillDuration;

            yield return null;
        }

        if (waterBar.fillAmount >= 1f)
        {
            waterBar.fillAmount = 1f;
            progressCoroutine = null;

            gameManager.LoseGame();
            yield break;
        }

        progressCoroutine = null;
    }

    public void ResetProgress()
    {
        SetActiveLeaks(0);
        waterBar.fillAmount = 0f;
    }
}
