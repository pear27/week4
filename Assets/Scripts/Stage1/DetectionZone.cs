using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
    private bool playerInZone = false;
    private bool npcInZone = false;

    public Transform player; // 플레이어 트랜스폼
    public Transform npc;    // NPC 트랜스폼
    public Animator npcAnimator; // NPC 애니메이터

    public float shakeAmount = 0.1f; // 흔들림 강도
    public float shakeDuration = 1f; // 흔들림 지속 시간
    public NPCPatrol npcPatrolScript; // NPC 이동 스크립트 참조

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
        else if (other.CompareTag("NPC"))
        {
            npcInZone = true;
        }

        CheckDetection();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;
        else if (other.CompareTag("NPC"))
            npcInZone = false;
    }

    private void CheckDetection()
    {
        if (playerInZone && npcInZone)
        {
            Debug.Log("Player detected by NPC!");
            StartCoroutine(ShakeReaction());
        }
    }

    private IEnumerator ShakeReaction()
    {
        if (npcAnimator != null)
        {
            npcAnimator.speed = 0f; // NPC 애니메이션 정지
        }

        if (npcPatrolScript != null)
        {
            npcPatrolScript.StopAllCoroutines(); // NPC 스크립트에서 돌고 있는 Coroutine 정지
        }

        Vector3 originalPlayerPos = player.position;
        Vector3 originalNpcPos = npc.position;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float offset = Mathf.Sin(Time.time * 50f) * shakeAmount;
            player.position = originalPlayerPos + new Vector3(0, offset, 0);
            npc.position = originalNpcPos + new Vector3(0, offset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = originalPlayerPos; // 원래 위치로 복원
        npc.position = originalNpcPos; // 원래 위치로 복원

        yield return new WaitForSeconds(1f); // 흔들림 후 잠시 대기

        FadeManager.Instance.FadeOutAndLoad("Dead1");
    }
}