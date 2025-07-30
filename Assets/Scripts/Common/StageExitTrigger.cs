using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageExitTrigger : MonoBehaviour
{
    public string nextSceneName; // Inspector에서 지정
    public TMP_Text warningText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (nextSceneName == "HappyEnding")
        {
            if (GooseRescueManager.Instance != null && GooseRescueManager.Instance.AllGeeseRescued())
            {
                Debug.Log("Player reached exit. Loading next stage...");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("Not all geese rescued yet. Cannot exit.");

                if (warningText != null)
                {
                    warningText.text = "아직 모든 거위를 구출하지 않았습니다!";
                    warningText.gameObject.SetActive(true);
                    Invoke(nameof(HideWarningText), 2f); // 2초 후 경고 메시지 숨김
                }
            }
        }
        else
        {
            Debug.Log("Player reached exit. Loading next stage...");
            SceneManager.LoadScene(nextSceneName);
        } 
    }
    
    private void HideWarningText()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }
}
