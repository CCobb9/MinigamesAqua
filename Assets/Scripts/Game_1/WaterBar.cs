using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    [SerializeField] private Image waterBar;
    [SerializeField] private float fillDuration = 10f;

    private Coroutine progressCoroutine;
    private GameManager gameManager;

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    private void Start()
    {
        waterBar.fillAmount = 0f;
    }

    public void StartProgress()
    {
        if (progressCoroutine != null)
            return;

        progressCoroutine = StartCoroutine(ProgressRoutine());
    }

    public void StopProgress()
    {
        if (progressCoroutine == null)
            return;

        StopCoroutine(progressCoroutine);
        progressCoroutine = null;
    }

    private IEnumerator ProgressRoutine()
    {
        while (waterBar.fillAmount < 1f)
        {
            waterBar.fillAmount += Time.deltaTime / fillDuration;

            yield return null;
        }

        waterBar.fillAmount = 1f;
        progressCoroutine = null;

        gameManager.LoseGame();
    }

    public void ResetProgress()
    {
        StopProgress();
        waterBar.fillAmount = 0f;
    }
}