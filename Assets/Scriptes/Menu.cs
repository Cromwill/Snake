using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Scene_4");
    }
}
