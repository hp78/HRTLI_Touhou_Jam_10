using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("MainMultiplayer");
    }
}
