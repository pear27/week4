using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GooseTiedController : MonoBehaviour
{
    public Animator animator;            // Animator ���� (Idle �� ���� ��ȯ��)
    public SpriteRenderer spriteRenderer; // Sprite �ٲ� Renderer
    public Sprite tiedSprite;           // �̹��� 1 (���� ����)
    public Sprite defaultPeckSprite;    // �̹��� 2 (���� Ǯ�� �⺻)
    public Sprite peckSprite;           // �̹��� 3 (�ɴ� ����)

    public float mashDecayRate = 1f; // �ʴ� ������ ���ҷ�
    public float mashIncrease = 10f; // �� �� ���� �� ������
    public float requiredMash = 100f; // Ǯ�� ���� �ʿ��� ������ ��
    public float currentMash = 0f;

    public GooseWalkingController gooseWalkingController;

    private bool isTied = true;

    void Start()
    {
        gooseWalkingController.isTied = true; // GooseWalkingController���� ���� ���� ����

        if (animator != null)
            animator.enabled = false; // Animator ��Ȱ��ȭ

        if (spriteRenderer != null)
            spriteRenderer.sprite = tiedSprite; // ó�� ���� 1�� �̹���

        StartCoroutine(ChangeToPeckReady());
    }

    IEnumerator ChangeToPeckReady()
    {
        yield return new WaitForSeconds(2f); // 2�� ����
        spriteRenderer.sprite = defaultPeckSprite; // 2�� �̹����� ����
    }

    void Update()
    {
        if (!isTied)
            return; // �������� ������ �ƹ� ���۵� ���� ����

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMash += mashIncrease; // �����̽��� ���� ������ ������ ����
            StartCoroutine(PeckAnimation());
        }

        // ����Ű �Է� �� ��鸲
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(WiggleAnimation());
        }

        currentMash -= mashDecayRate * Time.deltaTime; // ������ ����
        currentMash = Mathf.Clamp(currentMash, 0, requiredMash); // ������ ���� ����

        if (currentMash >= requiredMash)
            ReleaseGoose();
    }

    IEnumerator PeckAnimation()
    {
        spriteRenderer.sprite = peckSprite;
        yield return new WaitForSeconds(0.1f); // �ɴ� ���� ����
        spriteRenderer.sprite = defaultPeckSprite;
    }

    IEnumerator WiggleAnimation()
    {
        Vector3 originalPos = transform.position;
        float wiggleAmount = 0.05f; // ��鸲 �Ÿ�
        float duration = 0.1f;     // �� �̵��� ���� �ð�

        // ����
        transform.position = originalPos + new Vector3(-wiggleAmount, 0, 0);
        yield return new WaitForSeconds(duration);

        // ������
        transform.position = originalPos + new Vector3(wiggleAmount, 0, 0);
        yield return new WaitForSeconds(duration);

        // ����ġ
        transform.position = originalPos;
    }

    void ReleaseGoose()
    {
        isTied = false;
        gooseWalkingController.isTied = false; // GooseWalkingController���� ���� ���� ����
        if (animator != null)
            animator.enabled = true; // Animator Ȱ��ȭ

        Debug.Log("Goose is free!");
    }
}
