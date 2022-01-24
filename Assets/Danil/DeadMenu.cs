using Zenject;
using GameMode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public void OnRestart()
    {
        LoadScene();
    }
    private void OnExit()
    {
        LoadScene();
    }
    private void LoadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
