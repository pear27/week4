using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageExitTrigger : MonoBehaviour
{
    public string nextSceneName; // Inspector���� ����
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
                    warningText.text = "���� ��� ������ �������� �ʾҽ��ϴ�!";
                    warningText.gameObject.SetActive(true);
                    Invoke(nameof(HideWarningText), 2f); // 2�� �� ��� �޽��� ����
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
