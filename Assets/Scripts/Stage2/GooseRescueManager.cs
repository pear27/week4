using System.Collections;
using UnityEngine;
using TMPro;

public class GooseRescueManager : MonoBehaviour
{
    public static GooseRescueManager Instance;

    public TMP_Text cageCounterText;
    private int totalCageCount = 7;
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

        if (cageCounterText != null)
            cageCounterText.text = $"{rescuedCageCount}/{totalCageCount}";

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
            gooseController.isFrozen = true; // 거위 이동 멈춤

        if (feedShooters != null && feedShooters.Length > 0)
            foreach (FeedShooter shooter in feedShooters)
                shooter.enabled = false;
        
        GameObject npc = Instantiate(npcPrefab, npcSpawnPoint.position, Quaternion.identity);
        Animator npcAnimator = npc.GetComponent<Animator>();

        CameraFollow camFollow = mainCamera.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.SetTarget(npc.transform); // 카메라가 NPC를 따라가도록 설정

        yield return new WaitForSeconds(1f); // 카메라 이동 연출 여유

        if (npcAnimator != null)
            npcAnimator.SetBool("isWalking", false); // NPC 걷기 애니메이션 멈춤

        yield return new WaitForSeconds(2f); // NPC 도착 후 잠시 대기

        if (camFollow != null)
            camFollow.SetTarget(player); // 카메라가 다시 플레이어를 따라가도록 설정
        else
            mainCamera.transform.position = new Vector3(player.position.x, mainCamera.transform.position.y, -10f);

        if (gooseController != null)
            gooseController.isFrozen = false; // 거위 이동 재개

        if (feedShooters != null && feedShooters.Length > 0)
            foreach (FeedShooter shooter in feedShooters)
                shooter.enabled = true;
    }
}
