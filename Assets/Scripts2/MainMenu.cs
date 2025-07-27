using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickNewGame()
    {
        Debug.Log("처음부터");
        SceneManager.LoadScene("Opening");
    }

    public void OnClickLoad()
    {
        Debug.Log("이어서");
    }

    public void OnClickQuit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
