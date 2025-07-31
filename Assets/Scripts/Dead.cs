using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  // �� ��ȯ��

public class Dead : MonoBehaviour
{
    [Header("Fade Settings")]
    public CanvasGroup blackScreen;         // ���� ���
    public float fadeDuration = 2f;         // ȭ���� ��ο����� �ð�

    [Header("UI Buttons")]
    public CanvasGroup restartButtonCanvas; // �ٽ��ϱ� ��ư CanvasGroup
    public CanvasGroup exitButtonCanvas;    // ������ ��ư CanvasGroup
    public float buttonFadeDuration = 1f;   // ��ư ���̵��� �ð�

    private bool isFading = false;          // ���̵� ������ ����

    void Start()
    {
        // ���� �� ȭ��� ��ư ����
        if (blackScreen != null) blackScreen.alpha = 0f;
        if (restartButtonCanvas != null) restartButtonCanvas.alpha = 0f;
        if (exitButtonCanvas != null) exitButtonCanvas.alpha = 0f;
    }

    void Update()
    {
        // �ƹ� Ű�� ������ �� �� ���� ���̵� ����
        if (!isFading && Input.anyKeyDown)
        {
            StartCoroutine(FadeSequence());
        }
    }

    private IEnumerator FadeSequence()
    {
        isFading = true;
        yield return StartCoroutine(FadeToBlack());       // ȭ�� ��ο���
        yield return StartCoroutine(FadeInBothButtons()); // ��ư 2�� ���ÿ� ���̵���
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

    private IEnumerator FadeInBothButtons()
    {
        float elapsed = 0f;
        while (elapsed < buttonFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / buttonFadeDuration);

            if (restartButtonCanvas != null) restartButtonCanvas.alpha = alpha;
            if (exitButtonCanvas != null) exitButtonCanvas.alpha = alpha;

            yield return null;
        }
        if (restartButtonCanvas != null) restartButtonCanvas.alpha = 1f;
        if (exitButtonCanvas != null) exitButtonCanvas.alpha = 1f;
    }

    // �ٽ��ϱ� ��ư
    public void OnRestartButton()
    {
        SceneManager.LoadScene("scene1_slaughter"); // stage2 ��� Farm �� �ε�
    }

    // ������ ��ư
    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
