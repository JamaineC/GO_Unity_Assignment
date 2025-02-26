using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// class to control the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // gui pause menu panel
    [SerializeField] GameObject passButton; // gui pass button
    public static bool isPaused = false; // state of the game and whether a stone can be placed

    /// <summary>
    /// function to pause game
    /// </summary>
    public void Pause()
    {
        pauseMenu.SetActive(true); // open pause panel
        passButton.SetActive(false); // deactivate buttons
        isPaused = true; // game is paused
    }

    /// <summary>
    /// function to resume the game
    /// </summary>
    public void Resume()
    {
        pauseMenu.SetActive(false); // close pause panel
        passButton.SetActive(true); // activate buttons
        isPaused = false;  // game resumed
    }


    /// <summary>
    /// function to restart the game
    /// </summary>
    public void Restart()
    {
        SceneLoader.LoadScene(SceneLoader.GetActiveScene().buildIndex); // reload the current scene
        isPaused = false; // no longer paused
    }

    /// <summary>
    /// function to quit to the home screen
    /// </summary>
    public void Quit()
    {
        SceneLoader.LoadScene(0); // load the home screen
        isPaused = false; // no longer paused
    }

}
