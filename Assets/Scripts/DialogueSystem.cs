using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI txtSentence;
    public float typingSpeed = 0.01f;
    public Dialogue defaultDialogue;

    [Header("Blink UI")]
    public RectTransform topEyelid;
    public RectTransform bottomEyelid;
    public float blinkSpeed = 3000f;
    public float blinkPause = 0.3f;
    public int blinkCount = 3;

    [Header("Background Fade")]
    public CanvasGroup backgroundCanvasGroup;
    public float backgroundFadeTime = 4f;

    [Header("Dialogue Image Blink")]
    public GameObject dialogueImage;
    public CanvasGroup dialogueImageCanvas;
    public float imageFadeSpeed = 2f;

    [Header("Special Images")]
    public GameObject mrImage;        // 3번째 대사에서 등장할 이미지
    public GameObject gooselegsImage; // 끝날 때 크게 등장할 이미지
    public CanvasGroup blackScreen;   // 전체 화면 검은색 페이드

    private Queue<string> sentences = new Queue<string>();
    private Coroutine typingCoroutine;
    private Coroutine imageBlinkCoroutine;
    private bool dialogueEnded = false;
    private bool gooselegsShown = false;
    private string currentSentence = "";
    private int dialogueIndex = 0;

    void Start()
    {
        if (defaultDialogue != null)
            Begin(defaultDialogue);

        if (backgroundCanvasGroup != null)
            backgroundCanvasGroup.alpha = 0;

        if (dialogueImageCanvas != null)
            dialogueImageCanvas.alpha = 0;

        if (mrImage != null)
            mrImage.SetActive(false);

        if (gooselegsImage != null)
            gooselegsImage.SetActive(false);

        if (blackScreen != null)
            blackScreen.alpha = 0;
    }

    void Update()
    {
        if (!dialogueEnded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    txtSentence.text = currentSentence;
                    typingCoroutine = null;
                }
                else
                {
                    Next();
                }
            }
        }
        else
        {
            if (!gooselegsShown && Input.GetKeyDown(KeyCode.Space))
            {
                ShowGooselegs();
            }
            else if (gooselegsShown && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(FadeOutAndChangeScene());
            }
        }
    }

    public void Begin(Dialogue info)
    {
        sentences.Clear();
        foreach (var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }
        dialogueIndex = 0;
        Next();
    }

    public void Next()
    {
        if (sentences.Count == 0)
        {
            End();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueIndex++;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(sentence));

        Debug.Log($"대사 진행: {dialogueIndex}");
        if (dialogueIndex == 3 && mrImage != null)
        {
            Debug.Log("MR 이미지 켜짐!");
            mrImage.SetActive(true);
        }
        // 3번째 대사에서 mr 이미지 등장
        if (dialogueIndex == 3 && mrImage != null)
        {
            mrImage.SetActive(true);
        }

        // 6번째 대사에서 mr 이미지 사라짐
        if (dialogueIndex == 6 && mrImage != null)
        {
            mrImage.SetActive(false);
        }

        // 3번째 대사에서 깜빡이 이미지 시작 (dialogueImage)
        if (dialogueIndex == 3 && dialogueImage != null)
        {
            dialogueImage.SetActive(true);
            if (imageBlinkCoroutine != null) StopCoroutine(imageBlinkCoroutine);
            imageBlinkCoroutine = StartCoroutine(ImageBlinkFade());
        }

        // 6번째 대사에서 깜빡이 이미지 사라짐
        if (dialogueIndex == 6 && dialogueImage != null)
        {
            if (imageBlinkCoroutine != null) StopCoroutine(imageBlinkCoroutine);
            StartCoroutine(FadeOutImage());
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        currentSentence = sentence;
        txtSentence.text = "";
        foreach (char letter in sentence)
        {
            txtSentence.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    private void End()
    {
        dialogueEnded = true;
        StartCoroutine(BlinkAndFade());
    }

    private IEnumerator BlinkAndFade()
    {
        float totalBlinkTime = backgroundFadeTime / blinkCount;

        for (int i = 0; i < blinkCount; i++)
        {
            Coroutine blink = StartCoroutine(BlinkOnce(topEyelid, bottomEyelid));
            Coroutine fade = StartCoroutine(FadeBackgroundSegment(totalBlinkTime));
            yield return blink;
        }

        if (backgroundCanvasGroup != null)
            backgroundCanvasGroup.alpha = 1f;

        Vector2 topStart = new Vector2(0, Screen.height / 2f + topEyelid.rect.height);
        Vector2 bottomStart = new Vector2(0, -Screen.height / 2f - bottomEyelid.rect.height);
        topEyelid.anchoredPosition = topStart;
        bottomEyelid.anchoredPosition = bottomStart;
    }

    private IEnumerator FadeBackgroundSegment(float segmentTime)
    {
        if (backgroundCanvasGroup == null) yield break;

        float startAlpha = backgroundCanvasGroup.alpha;
        float endAlpha = Mathf.Min(1f, startAlpha + (1f / blinkCount));

        float t = 0f;
        while (t < segmentTime)
        {
            t += Time.deltaTime;
            backgroundCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / segmentTime);
            yield return null;
        }
    }

    private IEnumerator BlinkOnce(RectTransform top, RectTransform bottom)
    {
        Vector2 topStart = new Vector2(0, Screen.height / 2f + top.rect.height);
        Vector2 topClosed = new Vector2(0, 0);
        Vector2 bottomStart = new Vector2(0, -Screen.height / 2f - bottom.rect.height);
        Vector2 bottomClosed = new Vector2(0, 0);

        // 닫힘
        while (Vector2.Distance(top.anchoredPosition, topClosed) > 1f ||
               Vector2.Distance(bottom.anchoredPosition, bottomClosed) > 1f)
        {
            top.anchoredPosition = Vector2.MoveTowards(top.anchoredPosition, topClosed, blinkSpeed * Time.deltaTime);
            bottom.anchoredPosition = Vector2.MoveTowards(bottom.anchoredPosition, bottomClosed, blinkSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(blinkPause);

        // 열림
        while (Vector2.Distance(top.anchoredPosition, topStart) > 1f ||
               Vector2.Distance(bottom.anchoredPosition, bottomStart) > 1f)
        {
            top.anchoredPosition = Vector2.MoveTowards(top.anchoredPosition, topStart, blinkSpeed * Time.deltaTime);
            bottom.anchoredPosition = Vector2.MoveTowards(bottom.anchoredPosition, bottomStart, blinkSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // 이미지 투명도 깜빡 코루틴
    private IEnumerator ImageBlinkFade()
    {
        while (true)
        {
            float alpha = (Mathf.Sin(Time.time * imageFadeSpeed) + 1f) / 2f; // 0~1 반복
            dialogueImageCanvas.alpha = alpha;
            yield return null;
        }
    }

    // 이미지 서서히 사라짐
    private IEnumerator FadeOutImage()
    {
        float t = 0f;
        float startAlpha = dialogueImageCanvas.alpha;
        while (t < 1f)
        {
            t += Time.deltaTime;
            dialogueImageCanvas.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }
        dialogueImage.SetActive(false);
    }

    private void ShowGooselegs()
    {
        if (gooselegsImage == null) return;
        gooselegsImage.SetActive(true);
        gooselegsShown = true;  
    }

    private IEnumerator FadeToBlack()
    {
        if (blackScreen == null) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 2f;
            blackScreen.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }
    private IEnumerator FadeOutAndChangeScene()
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene("scene1_slaughter");
    }
}