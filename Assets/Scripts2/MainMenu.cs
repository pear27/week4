using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickNewGame()
    {
        Debug.Log("ó������");
        SceneManager.LoadScene("Opening");
    }

    public void OnClickLoad()
    {
        Debug.Log("�̾");
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
