using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class DetectionZone : MonoBehaviour
{
    private bool playerInZone = false;
    private bool npcInZone = false;

    public Transform player; // �÷��̾� Ʈ������
    public Transform npc;    // NPC Ʈ������
    public Animator npcAnimator; // NPC �ִϸ�����

    public float shakeAmount = 0.1f; // ��鸲 ����
    public float shakeDuration = 1f; // ��鸲 ���� �ð�
    public NPCPatrol npcPatrolScript; // NPC �̵� ��ũ��Ʈ ����

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
            npcAnimator.speed = 0f; // NPC �ִϸ��̼� ����
        }

        if (npcPatrolScript != null)
        {
            npcPatrolScript.StopAllCoroutines(); // NPC ��ũ��Ʈ���� ���� �ִ� Coroutine ����
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

        player.position = originalPlayerPos; // ���� ��ġ�� ����
        npc.position = originalNpcPos; // ���� ��ġ�� ����

        yield return new WaitForSeconds(1f); // ��鸲 �� ��� ���

        FadeManager.Instance.FadeOutAndLoad("Dead1");
    }
}