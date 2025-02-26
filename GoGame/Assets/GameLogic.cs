using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



/// <summary>
/// class to control the end game scenario
/// </summary>
public class GameLogic : MonoBehaviour
{

    [SerializeField] GameObject gameOverScreen; // game over panel
    [SerializeField] GameObject passButton; // button to pass go and end game

    public TextMeshProUGUI endScoreBreakDown; // final score breakdown text
    public  TextMeshProUGUI winnerTitle; // winner title text

    public TextMeshProUGUI finalScore; // final score text


    /// <summary>
    /// Function to get the winner
    /// </summary>
    /// <returns> the winner </returns>
    public static int GetWinner()
    {
        int winner = PlayerTurn.playerScore[0] > PlayerTurn.playerScore[1] ?  0 : 1; // find winner

        return winner;
    }


    /// <summary>
    /// function to get the final score of a player
    /// </summary>
    /// <param name="winner"> the winner of the game </param>
    /// <returns> the final score of the winner </returns>
    public static int GetFinalScore(int winner)
    {

        int finalScore = PlayerTurn.playerScore[winner];

        return finalScore;
    }


    /// <summary>
    /// function to get the amount of captures of the winner
    /// </summary>
    /// <param name="winner"> the winner of the game </param>
    /// <returns> the amount of enemy pieces captured </returns>
    public static int GetAmountOfCaptures(int winner)
    {
        int amountOfCaptures = PlayerTurn.playerCaptures[winner];

        return amountOfCaptures;
    }
    

    /// <summary>
    /// function to calculate the amount of territory owned by a player
    /// </summary>
    /// <param name="winner"> the winner of the game </param>
    /// <returns> the amount of territory owned by the player </returns>
    public static int CalculateTerritory(int winner)
    {
        int amountOfTerritory =  0; //SearchAlg.ownedTerritory[,]
        for (int i = 0; i < GridManager.size; i++)
        {
            for (int j = 0; j < GridManager.size; j++)
            {
                if (SearchAlg.ownedTerritory[i,j] == winner + 1) amountOfTerritory++; // add up territory
            }
        }

        return amountOfTerritory;
    }

        /// <summary>
        /// function to calculate the amount of stones a player has
        /// </summary>
        /// <param name="winner"> the winner of the game </param>
        /// <returns> the amount of stones of the winner on the board </returns>
        public static int CalculateStones(int winner)
    {
        int amountOfStones =  0;
        for (int i = 0; i < GridManager.size; i++)
        {
            for (int j = 0; j < GridManager.size; j++)
            {
                if (SearchAlg.placedStone[i,j] == winner + 1) amountOfStones++; // add stones up
            }
        }

        return amountOfStones;
    }



    /// <summary>
    /// function to check if the game over condition has been met
    /// </summary>
    /// <returns> true or false </returns>
    public static bool IsGameOver()
    {
        if (PlayerTurn.numberOfPasses > 1) // if passed twice in a row
        {
            return true;
        }
        else return false;
    }



    /// <summary>
    /// function to update the score for both players
    /// </summary>
    public static void SetScore()
    {
        // black score
        int blackScore = 0;
        blackScore += CalculateStones(0);
        blackScore += CalculateTerritory(0);
        blackScore += GetAmountOfCaptures(0);

        // white score
        int whiteScore = 0;
        whiteScore += CalculateStones(1);
        whiteScore += CalculateTerritory(1);
        whiteScore += GetAmountOfCaptures(1);

        
        // setting the score
        PlayerTurn.playerScore[0] = blackScore;
        PlayerTurn.playerScore[1] = whiteScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (IsGameOver())
       { 

            // Set Winners Score and update the text
            int winner = GetWinner();
            if (winner == 0) winnerTitle.text = "The Winner is Black!";
            else winnerTitle.text = "The Winner is White!";
            int finalPoints = GetFinalScore(winner);
            finalScore.text = $"{finalPoints}";
            int finalTerritory = CalculateTerritory(winner);
            int finalStones = CalculateStones(winner);
            int finalCaptures = GetAmountOfCaptures(winner);
            endScoreBreakDown.text = $" STONES: x{finalStones} \n \n";
            endScoreBreakDown.text += $" OWNED TERRITORY: x{finalTerritory} \n \n";
            endScoreBreakDown.text += $" AMOUNT OF CAPTURES: x{finalCaptures} ";
            gameOverScreen.SetActive(true);
            passButton.SetActive(false);
       }

    }
}
