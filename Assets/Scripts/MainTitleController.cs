using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitleController : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}