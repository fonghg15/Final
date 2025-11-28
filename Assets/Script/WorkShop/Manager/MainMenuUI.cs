using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Name")]
    public string gameSceneName = "GameScene";

    public void OnPlayButton()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene(gameSceneName);
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}