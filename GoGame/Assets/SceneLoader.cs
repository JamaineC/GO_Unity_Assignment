using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// class to control the scenes
/// </summary>
public class SceneLoader : MonoBehaviour
{

public static Scene activeScene;

/// <summary>
/// function to get the current scene
/// </summary>
/// <returns> the active scene </returns>
public static Scene GetActiveScene()
{
    activeScene = SceneManager.GetActiveScene();
    return activeScene;
}

/// <summary>
/// function to load the level/home screen
/// </summary>
/// <param name="sceneNumber"> the intended scene to load </param>
public static void LoadScene(int sceneNumber)
{

    // reset player turn
    PlayerTurn.resetTurns();

    // load new scene
    SceneManager.LoadSceneAsync(sceneNumber);
}

void Update()
{
    //GetActiveScene();
}

}