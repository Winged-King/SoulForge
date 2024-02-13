using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Ending Game...");
        Application.Quit();
    }

    public void TitleScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Returning to Title...");
        SceneManager.LoadScene(0);
    }

    public void HubWorld()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Going to Hub...");
        SceneManager.LoadScene(1);
    }

    public void TimeWorld()
    {
        Debug.Log("Going to World 1...");
        SceneManager.LoadScene(2);
    }

    public void SpaceWorld()
    {
        Debug.Log("Going to World 2...");
        SceneManager.LoadScene(3);
    }

    public void SoulWorld()
    {
        Debug.Log("Going to World 3...");
        SceneManager.LoadScene(4);
    }
}
