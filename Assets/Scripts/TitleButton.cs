using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    
    public void LoadMainScene()
    {
        DeleteUnnecessaryObjs();
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1.0f;
    }

    public void ExitGame()
    {
        DeleteUnnecessaryObjs();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    
    public void ReturnTitle()
    {
        DeleteUnnecessaryObjs();
        SceneManager.LoadScene("TitleScene");
    }

    private void DeleteUnnecessaryObjs()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] beams = GameObject.FindGameObjectsWithTag("Beam");
        foreach(var enemy in enemys)
        {
            Destroy(enemy);
        }
        foreach(var beam in beams)
        {
            Destroy(beam);
        }
    }
}
