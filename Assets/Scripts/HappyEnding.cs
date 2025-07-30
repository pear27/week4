using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HappyEnding : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;     // ��� �ؽ�Ʈ
    public float typingSpeed = 0.05f;

    [Header("Fade Settings")]
    public CanvasGroup fadeScreen;           // ���� ȭ�� ���̵��
    public float fadeDuration = 1f;
    public float lineDelay = 0.5f;           // ��� ��ȯ ��� �ð�

    [Header("Background Images")]
    public Image beautifulBG;                // ���� ���
    public Image kaistBG;                    // ī�̽�Ʈ ��Ʈ
    public Image geeseBG;                    // ���� ����

    [Header("Buttons")]
    public GameObject restartButton;
    public GameObject exitButton;

    [Header("Audio")]
    public AudioSource audioSource;   // ���� ����� AudioSource
    public AudioClip gooseSFX;        // �в� ȿ����


    private int currentStep = 0;             // ���� ���� (3-1, 3-2, 3-3)
    private int currentLine = 0;             // ���� ��� �ε���
    private Coroutine typingCoroutine;
    private bool isTransitioning = false;

    private string[][] lines;                // ��� �ؽ�Ʈ�� ����

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
        InitializeLines();

        // ��� ��� �̹����� ���� (����ȭ)
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
                // Ÿ���� ���̸� ���� ǥ��
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
            "ö�� -",
            "�������� ��� �Բ� �ٱ� �������� ������.",
            "ö�� ������ �����ϴ� ������ ������ ��� ��������,",
            "����� �ʿ��� ������ �ŴҸ� ������ �¸����� �����ߴ�.",
            "�� - �� -",
            "��μ� �׵��� ���� �����ڿ� ó������ ������ �ƴ� �ູ�� ������ ���ö���.",
            "�׸���, �׵��� �� �ָ��� ���ƿö���."
        }; 

        lines[1] = new string[]
        {
            "-2025��, ����-",
            "�л�1: ��~ ���õ� �ڵ� ��¥ ���ô�.",
            "�л�2: �ϡ� �ǰ��ϴ�. �Ľ����� ���鼭 ��å �� �ҷ�?",
            "�л�1: �׷�, ��� ������."
        };

        lines[2] = new string[]
        {
            "�л�1: ��, ����! ���� �� ��Ҵ١�",
            "�л�2: ��, �� ���ݱ��� ���� ���������� �� �˾Ҵµ� ���̾���.",
            "�ٵ� �� ������, �������� ���⼭ �� ����?",
            "�л�1: �װ� ������, .........",
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
        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 0f, 1f)); // ȭ�� ��ο���
        yield return new WaitForSeconds(lineDelay);

        StartCoroutine(ShowStepBackground(currentStep));  // ���ο� ���
        StartTyping(lines[currentStep][currentLine]);

        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 1f, 0f)); // ȭ�� �����
        isTransitioning = false;
    }

    private IEnumerator ShowEndingButtons()
    {
        yield return StartCoroutine(FadeCanvasGroup(fadeScreen, 0f, 1f)); // ���� ����ȭ��
        yield return new WaitForSeconds(0.5f);
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    private void StartTyping(string text)
    {
        Debug.Log($"[StartTyping] text = {text}");

        // �в� ����� �� ȿ���� ���
        if (text.Contains("��") && audioSource != null && gooseSFX != null)
        {
            Debug.Log("[StartTyping] Goose SFX ���");
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

    // ��� ��ȯ
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
        Debug.Log("ó������");
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
