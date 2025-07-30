using UnityEngine;

public class Feed : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            || collision.gameObject.CompareTag("Player")
            || collision.gameObject.CompareTag("NPC"))
            Destroy(gameObject);
    }
}
