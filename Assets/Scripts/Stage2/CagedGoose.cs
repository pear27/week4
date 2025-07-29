using UnityEngine;
using UnityEngine.UI;

public class CagedGoose : MonoBehaviour
{
    public Sprite emptyCageSprite;        // 케이지가 열린 이미지
    public GameObject rescuedGoosePrefab; // 해방된 거위 프리팹
    public Transform rescuedSpawnPoint;   // 해방된 거위가 나올 위치
    public int requiredPresses = 7;       // 구조에 필요한 스페이스바 횟수

    private int currentPresses = 0;
    private bool playerInRange = false;
    private bool isRescued = false;
    private SpriteRenderer spriteRenderer;

    public GameObject hintUI; // "스페이스바 눌러 구조하기" UI 오브젝트

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
            animator.enabled = false; // 애니메이션 멈춤

        spriteRenderer.sprite = emptyCageSprite; // 케이지 열기

        if (rescuedGoosePrefab != null && rescuedSpawnPoint != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = rescuedSpawnPoint.position + new Vector3(i * 0.5f, 0, 0); // 약간씩 위치 차이
                GameObject goose = Instantiate(rescuedGoosePrefab, spawnPos, Quaternion.identity);

                GooseRescued gooseScript = goose.GetComponent<GooseRescued>();
                if (gooseScript != null)
                {
                    gooseScript.walkSpeed = Random.Range(5f, 10f); // 5~10 사이 속도
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
