using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public void LoadLoseScene()
    {
     SceneManager.LoadScene("LoseScene");   
    }

    public void LoadMainScene()
    {
     SceneManager.LoadScene("MainScene");   
    }
}
