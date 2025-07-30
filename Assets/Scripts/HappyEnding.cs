using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HappyEnding : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;     // 대사 텍스트
    public float typingSpeed = 0.05f;

    [Header("Fade Settings")]
    public CanvasGroup fadeScreen;           // 검은 화면 페이드용
    public float fadeDuration = 1f;
    public float lineDelay = 0.5f;           // 대사 전환 대기 시간

    [Header("Background Images")]
    public Image beautifulBG;                // 예쁜 배경
    public Image kaistBG;                    // 카이스트 도트
    public Image geeseBG;                    // 거위 사진

    [Header("Buttons")]
    public GameObject restartButton;
    public GameObject exitButton;

    [Header("Audio")]
    public AudioSource audioSource;   // 사운드 재생용 AudioSource
    public AudioClip gooseSFX;        // 꽥꽥 효과음


    private int currentStep = 0;             // 엔딩 스텝 (3-1, 3-2, 3-3)
    private int currentLine = 0;             // 현재 대사 인덱스
    private Coroutine typingCoroutine;
    private bool isTransitioning = false;

    private string[][] lines;                // 대사 텍스트만 관리

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
        InitializeLines();

        // 모든 배경 이미지를 숨김 (투명화)
        SetImageAlpha(beautifulBG, 0f);
        SetImageAlpha(kaistBG, 0f);
        SetImageAlpha(geeseBG, 0f);

        dialogueText.text = "";
        if (fadeScreen != null) fadeScreen.alpha = 1f;

        restartButton.SetActive(false);
        exitButton.SetActive(false);

        StartCoroutine(StartSequence());
    }

    void Update()
    {
        if (!isTransitioning && Input.GetKeyDown(KeyCode.Space))
        {
            if (typingCoroutine != null)
            {
                // 타이핑 중이면 전부 표시
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[currentStep][currentLine];
                typingCoroutine = null;
            }
            else
            {
                NextLine();
            }
        }
    }

    private void InitializeLines()
    {
        lines = new string[3][];

        lines[0] = new string[]
        {
            "철컥 -",
            "거위들은 모두 함께 바깥 세상으로 나섰다.",
            "철분 냄새가 진동하던 비좁은 공간을 벗어난 거위들은,",
            "드넓은 초원을 마음껏 거닐며 자유를 온몸으로 맞이했다.",
            "꽥 - 꽥 -",
            "비로소 그들의 검은 눈동자에 처음으로 공포가 아닌 행복의 눈물이 차올랐다.",
            "그리고, 그들은 저 멀리로 날아올랐다."
        }; 

        lines[1] = new string[]
        {
            "-2025년, 대전-",
            "학생1: 아~ 오늘도 코딩 진짜 빡셌다.",
            "학생2: 하… 피곤하다. 파스쿠찌 가면서 산책 좀 할래?",
            "학생1: 그래, 잠깐 나가자."
        };

        lines[2] = new string[]
        {
            "학생1: 아, 젠장! 거위 똥 밟았다…",
            "학생2: 헐, 나 지금까지 저거 나뭇가지인 줄 알았는데 똥이었네.",
            "근데 저 거위들, 언제부터 여기서 산 거지?",
            "학생1: 그게 말이지, .........",
            "-END-"
        };
    }

    private IEnumerator StartSequence()
    {
        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 1f, 0f));
        currentStep = 0;
        currentLine = 0;
        StartCoroutine(ShowStepBackground(0));
        StartTyping(lines[0][0]);
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine < lines[currentStep].Length)
        {
            StartTyping(lines[currentStep][currentLine]);
        }
        else
        {
            NextStep();
        }
    }

    private void NextStep()
    {
        currentStep++;
        currentLine = 0;

        if (currentStep < lines.Length)
        {
            StartCoroutine(TransitionToNextStep());
        }
        else
        {
            StartCoroutine(ShowEndingButtons());
        }
    }

    private IEnumerator TransitionToNextStep()
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 0f, 1f)); // 화면 어두워짐
        yield return new WaitForSeconds(lineDelay);

        StartCoroutine(ShowStepBackground(currentStep));  // 새로운 배경
        StartTyping(lines[currentStep][currentLine]);

        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 1f, 0f)); // 화면 밝아짐
        isTransitioning = false;
    }

    private IEnumerator ShowEndingButtons()
    {
        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 0f, 1f)); // 완전 검정화면
        yield return new WaitForSeconds(0.5f);
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    private void StartTyping(string text)
    {
        Debug.Log($"[StartTyping] text = {text}");

        // 꽥꽥 대사일 때 효과음 재생
        if (text.Contains("꽥") && audioSource != null && gooseSFX != null)
        {
            Debug.Log("[StartTyping] Goose SFX 재생");
            audioSource.PlayOneShot(gooseSFX);
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(text));
    }

    private IEnumerator TypeSentence(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    // 배경 전환
    private IEnumerator ShowStepBackground(int step)
    {
        HideAllBackgrounds();

        Image target = null;
        if (step == 0) target = beautifulBG;
        if (step == 1) target = kaistBG;
        if (step == 2) target = geeseBG;

        if (target != null)
        {
            yield return StartCoroutine(FadeImage(target, 0f, 1f));
        }
    }

    private void HideAllBackgrounds()
    {
        SetImageAlpha(beautifulBG, 0f);
        SetImageAlpha(kaistBG, 0f);
        SetImageAlpha(geeseBG, 0f);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end)
    {
        float elapsed = 0f;
        group.alpha = start;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        group.alpha = end;
    }

    private IEnumerator FadeImage(Image img, float start, float end)
    {
        float elapsed = 0f;
        Color c = img.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, elapsed / fadeDuration);
            img.color = c;
            yield return null;
        }
        c.a = end;
        img.color = c;
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    public void OnRestartButton()
    {
        Debug.Log("처음부터");
        SceneManager.LoadScene("Opening");
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
