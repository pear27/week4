using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScaryEffect : MonoBehaviour
{
    [Header("Scary Image Settings")]
    public RectTransform scaryImage;
    public float shakeAmount = 10f;
    public float shakeSpeed = 0.05f;
    public float shakeDuration = 1f;

    [Header("YOU DEAD Text Settings")]
    public TextMeshProUGUI youDeadText;
    public float typingDelay = 0.1f;
    public string deadMessage = "YOU DEAD";

    [Header("Fade Settings")]
    public CanvasGroup blackScreen;
    public float fadeDuration = 2f;

    [Header("UI Buttons")]
    public CanvasGroup restartButtonCanvas;
    public CanvasGroup exitButtonCanvas;
    public float buttonFadeDuration = 1f; // 버튼 페이드인 시간

    private Vector3 originalPos;

    void Start()
    {
        if (scaryImage != null)
            originalPos = scaryImage.anchoredPosition;
        if (youDeadText != null)
            youDeadText.text = "";
        if (blackScreen != null)
            blackScreen.alpha = 0f;

        if (restartButtonCanvas != null) restartButtonCanvas.alpha = 0f;
        if (exitButtonCanvas != null) exitButtonCanvas.alpha = 0f;

        StartScarySequence();
    }

    public void StartScarySequence()
    {
        StartCoroutine(ScarySequence());
    }

    private IEnumerator ScarySequence()
    {
        yield return StartCoroutine(ShakeImage());
        yield return StartCoroutine(TypeText(deadMessage));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeToBlack());

        // 버튼 페이드인
        if (restartButtonCanvas != null) StartCoroutine(FadeInCanvas(restartButtonCanvas));
        if (exitButtonCanvas != null) StartCoroutine(FadeInCanvas(exitButtonCanvas));
    }

    private IEnumerator ShakeImage()
    {
        float elapsed = 0f;
        Vector3 originalScale = scaryImage.localScale;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            scaryImage.anchoredPosition = originalPos + new Vector3(x, 0, 0);

            float scaleFactor = Mathf.Lerp(1f, 1.2f, elapsed / shakeDuration);
            scaryImage.localScale = originalScale * scaleFactor;

            elapsed += shakeSpeed;
            yield return new WaitForSeconds(shakeSpeed);
        }
    }

    private IEnumerator TypeText(string message)
    {
        youDeadText.text = "";
        foreach (char c in message)
        {
            youDeadText.text += c;
            yield return new WaitForSeconds(typingDelay);
        }
    }

    private IEnumerator FadeToBlack()
    {
        if (blackScreen == null) yield break;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        blackScreen.alpha = 1f;
    }

    private IEnumerator FadeInCanvas(CanvasGroup canvasGroup)
    {
        float elapsed = 0f;
        while (elapsed < buttonFadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / buttonFadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    // 버튼 클릭 이벤트
    public void OnRestartButton()
    {
        SceneManager.LoadScene("Farm"); // stage2 씬으로 이동
    }

    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
