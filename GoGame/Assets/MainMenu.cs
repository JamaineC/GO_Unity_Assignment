using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the play and quit button on the homescreen
/// </summary>
public class MainMenu : MonoBehaviour
{

public void Play()
{
    SceneLoader.LoadScene(1);
}

public void Quit()
{
    Application.Quit();
}

}
