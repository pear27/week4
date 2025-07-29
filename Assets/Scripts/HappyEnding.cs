using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 엔딩에서 각 문장과 이미지를 함께 관리하는 구조체
/// </summary>
[System.Serializable]
public class EndingLine
{
    [TextArea(2, 5)]
    public string sentence;    // 대사
    public Sprite image;       // 문장에 해당하는 이미지
}

/// <summary>
/// 엔딩 단계(3-1, 3-2, 3-3)를 관리하는 클래스
/// </summary>
[System.Serializable]
public class EndingStep
{
    public EndingLine[] lines;  // 이 단계의 대사들
}

/// <summary>
/// 엔딩 전체를 관리하는 매니저
/// </summary>
public class HappyEnding : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backgroundImageUI;       // 이미지 표시 UI
    public TextMeshProUGUI dialogueText;  // 대사 표시 TextMeshPro
    public float typingSpeed = 0.05f;     // 글자 타이핑 속도

    [Header("Ending Steps")]
    public EndingStep[] endingSteps;      // 엔딩 3-1, 3-2, 3-3 데이터

    [Header("Fade Settings")]
    public CanvasGroup fadeScreen;        // 화면 페이드용 CanvasGroup
    public float fadeDuration = 1f;       // 페이드 시간

    [Header("Buttons")]
    public GameObject restartButton;
    public GameObject exitButton;

    private int currentStep = 0;          // 현재 엔딩 단계 인덱스
    private int currentLine = 0;          // 현재 대사 인덱스
    private Coroutine typingCoroutine;
    private bool isTransitioning = false; // 페이드 전환 중 여부

    void Start()
    {
        dialogueText.text = "";
        if (fadeScreen != null) fadeScreen.alpha = 1f; // 시작은 화면 어두움
        restartButton.SetActive(false);
        exitButton.SetActive(false);
        StartCoroutine(FadeIn()); // 첫 장면 시작
    }

    void Update()
    {
        if (!isTransitioning && Input.GetKeyDown(KeyCode.Space))
        {
            if (typingCoroutine != null)
            {
                // 타이핑 중이면 문장을 바로 다 보여줌
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
        yield return StartCoroutine(FadeOut()); // 화면 어두워짐
        currentLine = 0;
        ShowLine(currentLine);
        yield return StartCoroutine(FadeIn());  // 화면 밝아짐
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

        ShowLine(currentLine); // 첫 문장 시작
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
