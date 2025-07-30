using System.Collections;
using UnityEngine;

public class GooseRescueManager : MonoBehaviour
{
    public static GooseRescueManager Instance;

    private int rescuedCageCount = 0;
    public int requiredToTriggerEvent = 5;

    private bool npcSpawned = false;

    public GameObject npcPrefab;
    public Transform npcSpawnPoint;
    public Transform npcTargetPoint;

    public Camera mainCamera;
    public Transform player;
    public GooseWalkingController gooseController;
    public FeedShooter[] feedShooters;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddRescuedCage()
    {
        rescuedCageCount++;
        Debug.Log($"Rescued cages: {rescuedCageCount}");

        if (!npcSpawned && rescuedCageCount >= requiredToTriggerEvent)
        {
            npcSpawned = true;
            Debug.Log("Triggering NPC spawn event!");

            StartCoroutine(SpawnNPCSequence());
        }
    }

    private IEnumerator SpawnNPCSequence()
    {
        if (gooseController != null)
            gooseController.isFrozen = true; // ���� �̵� ����

        if (feedShooters != null && feedShooters.Length > 0)
            foreach (FeedShooter shooter in feedShooters)
                shooter.enabled = false;
        
        GameObject npc = Instantiate(npcPrefab, npcSpawnPoint.position, Quaternion.identity);
        Animator npcAnimator = npc.GetComponent<Animator>();

        CameraFollow camFollow = mainCamera.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.SetTarget(npc.transform); // ī�޶� NPC�� ���󰡵��� ����

        yield return new WaitForSeconds(1f); // ī�޶� �̵� ���� ����

        if (npcAnimator != null)
            npcAnimator.SetBool("isWalking", true); // NPC �ȱ� �ִϸ��̼� ����

        float speed = 2f;
        SpriteRenderer npcSprite = npc.GetComponent<SpriteRenderer>();

        while (Vector2.Distance(npc.transform.position, npcTargetPoint.position) > 1f)
        {
            Vector3 targetPos = new Vector3(
                npcTargetPoint.position.x,
                npcTargetPoint.position.y,
                npc.transform.position.z // Z�� ����
            );

            Vector3 dir = (targetPos - npc.transform.position).normalized;

            npc.transform.position += dir * speed * Time.deltaTime;

            yield return null;
        }

        // �������� ��ġ ����
        npc.transform.position = new Vector3(
            npcTargetPoint.position.x,
            npcTargetPoint.position.y,
            npc.transform.position.z
        );

        if (npcAnimator != null)
            npcAnimator.SetBool("isWalking", false); // NPC �ȱ� �ִϸ��̼� ����

        yield return new WaitForSeconds(2f); // NPC ���� �� ��� ���

        if (camFollow != null)
            camFollow.SetTarget(player); // ī�޶� �ٽ� �÷��̾ ���󰡵��� ����
        else
            mainCamera.transform.position = new Vector3(player.position.x, mainCamera.transform.position.y, -10f);

        if (gooseController != null)
            gooseController.isFrozen = false; // ���� �̵� �簳

        if (feedShooters != null && feedShooters.Length > 0)
            foreach (FeedShooter shooter in feedShooters)
                shooter.enabled = true;
    }
}
