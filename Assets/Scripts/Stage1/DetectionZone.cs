using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private bool playerInZone = false;
    private bool npcInZone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Player entered detection zone.");
        }
        else if (other.CompareTag("NPC"))
        {
            npcInZone = true;
            Debug.Log("NPC entered detection zone.");
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
            // → 여기서 Dead 처리 (애니메이션, Game Over UI 등)
        }
    }
}
