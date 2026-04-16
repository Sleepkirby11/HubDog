using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    public void ReturnMain()
    {
        SceneManager.LoadScene(0);
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
