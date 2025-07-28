using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExitTrigger : MonoBehaviour
{
    public string nextSceneName; // Inspector���� ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached exit. Loading next stage...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
