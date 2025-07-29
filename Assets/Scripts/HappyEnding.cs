using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// �������� �� ����� �̹����� �Բ� �����ϴ� ����ü
/// </summary>
[System.Serializable]
public class EndingLine
{
    [TextArea(2, 5)]
    public string sentence;    // ���
    public Sprite image;       // ���忡 �ش��ϴ� �̹���
}

/// <summary>
/// ���� �ܰ�(3-1, 3-2, 3-3)�� �����ϴ� Ŭ����
/// </summary>
[System.Serializable]
public class EndingStep
{
    public EndingLine[] lines;  // �� �ܰ��� ����
}

/// <summary>
/// ���� ��ü�� �����ϴ� �Ŵ���
/// </summary>
public class HappyEnding : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backgroundImageUI;       // �̹��� ǥ�� UI
    public TextMeshProUGUI dialogueText;  // ��� ǥ�� TextMeshPro
    public float typingSpeed = 0.05f;     // ���� Ÿ���� �ӵ�

    [Header("Ending Steps")]
    public EndingStep[] endingSteps;      // ���� 3-1, 3-2, 3-3 ������

    [Header("Fade Settings")]
    public CanvasGroup fadeScreen;        // ȭ�� ���̵�� CanvasGroup
    public float fadeDuration = 1f;       // ���̵� �ð�

    [Header("Buttons")]
    public GameObject restartButton;
    public GameObject exitButton;

    private int currentStep = 0;          // ���� ���� �ܰ� �ε���
    private int currentLine = 0;          // ���� ��� �ε���
    private Coroutine typingCoroutine;
    private bool isTransitioning = false; // ���̵� ��ȯ �� ����

    void Start()
    {
        dialogueText.text = "";
        if (fadeScreen != null) fadeScreen.alpha = 1f; // ������ ȭ�� ��ο�
        restartButton.SetActive(false);
        exitButton.SetActive(false);
        StartCoroutine(FadeIn()); // ù ��� ����
    }

    void Update()
    {
        if (!isTransitioning && Input.GetKeyDown(KeyCode.Space))
        {
            if (typingCoroutine != null)
            {
                // Ÿ���� ���̸� ������ �ٷ� �� ������
                StopCoroutine(typingCoroutine);
                dialogueText.text = endingSteps[currentStep].lines[currentLine].sentence;
                typingCoroutine = null;
            }
            else
            {
                NextLine();
            }
        }
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine < endingSteps[currentStep].lines.Length)
        {
            ShowLine(currentLine);
        }
        else
        {
            NextStep();
        }
    }

    private void ShowLine(int lineIndex)
    {
        var line = endingSteps[currentStep].lines[lineIndex];
        backgroundImageUI.sprite = line.image;
        StartTyping(line.sentence);
    }

    private void NextStep()
    {
        currentStep++;
        if (currentStep < endingSteps.Length)
        {
            StartCoroutine(TransitionToNextStep());
        }
        else
        {
            ShowButtons();
        }
    }

    private void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    private IEnumerator TransitionToNextStep()
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeOut()); // ȭ�� ��ο���
        currentLine = 0;
        ShowLine(currentLine);
        yield return StartCoroutine(FadeIn());  // ȭ�� �����
        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        if (fadeScreen == null) yield break;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        fadeScreen.alpha = 0f;

        ShowLine(currentLine); // ù ���� ����
    }

    private IEnumerator FadeOut()
    {
        if (fadeScreen == null) yield break;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        fadeScreen.alpha = 1f;
    }

    private void ShowButtons()
    {
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("stage2");
    }

    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
