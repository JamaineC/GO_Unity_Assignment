using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// class responsible of players turn and score
/// </summary>
public class PlayerTurn : MonoBehaviour
{


    public static bool isBlackTurn = true; // to control player turn
    public static int[] playerScore = new int [2] { 0, 0}; // to store player score

    public static int[] playerCaptures = new int [2] { 0, 0}; // to store player captures
    public TextMeshProUGUI turnTracker; // gui text to display whos turn it is
    public TextMeshProUGUI scoreTracker; // gui text to display score of current player


    public static int numberOfPasses = 0; // to track the number of consecutive passes

    public static Vector2Int protectedLocation = new Vector2Int(-1,-1); // off board initially

    /// <summary>
    /// function used to pass the players turn
    /// </summary>
    public static void PassTurn()
    {
        // swap turns
        if (isBlackTurn) isBlackTurn = false; 
        else isBlackTurn = true;
        GameLogic.SetScore(); // update the score for both players
    }

    /// <summary>
    /// function to increase the pass counter used on button press
    /// </summary>
    public static void increasePassCount()
    {
        numberOfPasses++; // increase count
    }

    
    /// <summary>
    /// function to reset the turns for restart
    /// </summary>
    public static void resetTurns()
    {
        // reset score && player captures
        for (int i = 0; i < 2; i ++){
            PlayerTurn.playerScore[i] = 0;
            PlayerTurn.playerCaptures[i] = 0;
        }
        // reset protected piece
        protectedLocation = new Vector2Int(-1, -1);

        numberOfPasses = 0; // reset number of passes
         isBlackTurn = true; // black goes first
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // score and turn text updater
        if (isBlackTurn){
            turnTracker.text = "Blacks Turn!";
            scoreTracker.text = "Blacks Score: " + playerScore[0];
        }
        else 
        {
            turnTracker.text = "Whites Turn!";
            scoreTracker.text = "Whites Score: " + playerScore[1];
        }
        
    }
}
