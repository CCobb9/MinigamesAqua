using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Game_1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Game_2");
    }

    public void LoadLevel3A()
    {
        SceneManager.LoadScene("Game_3_A");
    }

    public void LoadLevel3B()
    {
        SceneManager.LoadScene("Game_3_B");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Game_4");
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}