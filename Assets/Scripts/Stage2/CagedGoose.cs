using UnityEngine;
using UnityEngine.UI;

public class CagedGoose : MonoBehaviour
{
    public Sprite emptyCageSprite;        // �������� ���� �̹���
    public GameObject rescuedGoosePrefab; // �ع�� ���� ������
    public Transform rescuedSpawnPoint;   // �ع�� ������ ���� ��ġ
    public int requiredPresses = 7;       // ������ �ʿ��� �����̽��� Ƚ��

    private int currentPresses = 0;
    private bool playerInRange = false;
    private bool isRescued = false;
    private SpriteRenderer spriteRenderer;

    public GameObject hintUI; // "�����̽��� ���� �����ϱ�" UI ������Ʈ

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (hintUI != null) hintUI.SetActive(false);
    }

    private void Update()
    {
        if (isRescued || !playerInRange) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPresses++;
            Debug.Log($"Rescue Progress: {currentPresses}/{requiredPresses}");

            if (currentPresses >= requiredPresses)
            {
                RescueGoose();
            }
        }
    }

    private void RescueGoose()
    {
        isRescued = true;

        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false; // �ִϸ��̼� ����

        spriteRenderer.sprite = emptyCageSprite; // ������ ����

        if (rescuedGoosePrefab != null && rescuedSpawnPoint != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = rescuedSpawnPoint.position + new Vector3(i * 0.5f, 0, 0); // �ణ�� ��ġ ����
                GameObject goose = Instantiate(rescuedGoosePrefab, spawnPos, Quaternion.identity);

                GooseRescued gooseScript = goose.GetComponent<GooseRescued>();
                if (gooseScript != null)
                {
                    gooseScript.walkSpeed = Random.Range(5f, 10f); // 5~10 ���� �ӵ�
                }
            }
        }


        if (hintUI != null) hintUI.SetActive(false);

        Debug.Log("Goose rescued!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRescued) return;

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (hintUI != null) hintUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (hintUI != null) hintUI.SetActive(false);
        }
    }
}
