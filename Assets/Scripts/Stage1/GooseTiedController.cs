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

    private bool isTied = true;

    void Start()
    {
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

    void ReleaseGoose()
    {
        isTied = false;
        if (animator != null)
            animator.enabled = true; // Animator Ȱ��ȭ
       
        Debug.Log("Goose is free!");
    }
}
