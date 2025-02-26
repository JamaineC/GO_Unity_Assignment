using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to control the behaviour of all the stones on the board
/// </summary>
public class StonePlacer : MonoBehaviour
{
    [SerializeField] GameObject blackStone; // set prefab of black stone
    [SerializeField] GameObject whiteStone; // set prefab of white stone
    [SerializeField] GameObject highlightPrefab; // set prefab of highlighted connections/territory
    [SerializeField] GameObject libertyPrefab; // set prefab of liberties
    [SerializeField] GameObject whiteTerritory; // set prefab for white territory
    [SerializeField] GameObject blackTerritory; // set prefab for black territory


    List<Vector2Int> highlight = new List<Vector2Int>(); // list for vectors to highlight
    List<Vector2Int> liberty = new List<Vector2Int>(); // list for liberties to highlight
  

    // dictionary to keep track of  stones for later removal through captures
    private Dictionary<Vector2Int, GameObject> stonesOnBoard = new Dictionary<Vector2Int, GameObject>();

    // dictionary to keep track of highlighted stones for later removal
    private Dictionary<Vector2Int, GameObject> highlightedStones = new Dictionary<Vector2Int, GameObject>();

    // dictionary to keep track of highlighted territory for later removal
    private Dictionary<Vector2Int, GameObject> highlightedTerritory = new Dictionary<Vector2Int, GameObject>();

    // dictionary to keep track of highlighted liberties for later removal
    private Dictionary<Vector2Int, GameObject> highlightedLiberties = new Dictionary<Vector2Int, GameObject>();


    /// <summary>
    /// Function to get stone object from a coordinate (helper method for removing stones)
    /// </summary>
    /// <param name="x"> x coordinate of the stone </param>
    /// <param name="y"> y coordinate of the stone </param>
    /// <returns></returns>
    // 
    private GameObject GetStoneAtPosition(int x, int y)
    {
        Vector2Int currentStone = new Vector2Int(x, y);

        // if the stone exists at position
        if (stonesOnBoard.ContainsKey(currentStone))
        {
            return stonesOnBoard[currentStone];
        }
        else
        {
            return null;
        }
    }

    void Start()
    {

        // Set the board up
        for (int i = 0; i < GridManager.size; i++)
        {
            for (int j = 0; j < GridManager.size; j++) 
            {
                SearchAlg.placedStone[i, j] = 0; // nothing is placed
                SearchAlg.ownedTerritory[i, j] = 0; // no territory is claimed
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // don't update if paused or game over
        if (PauseMenu.isPaused || GameLogic.IsGameOver()) return; 

        // clear previous stone highlights
        foreach (GameObject highlighted in highlightedStones.Values)
        {
            Destroy(highlighted); // destroy the object from the game
        }
        highlightedStones.Clear(); // clear the dictionary

        // clear previous territory highlights
        foreach (GameObject highlighted in highlightedTerritory.Values)
        {
            Destroy(highlighted); // destoy the objects from the game
        }
        highlightedTerritory.Clear(); // clear the dictionary

        // clear previous liberty highlights
        foreach (GameObject liberty in highlightedLiberties.Values)
        {
            Destroy(liberty); // destoy the objects from the game
        }
        highlightedLiberties.Clear(); // clear the dictionary

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get coordinates of mouse click
        mousePos.z = 0;

        // calculate position of mouse
        float xClick = mousePos.x;
        float yClick = mousePos.y;

        float xVertice = -5;
        float yVertice = -5;

        // Calculate position to the nearest grid position
        if (xClick >= -0.5 && xClick < GridManager.size + 0.5) // check bounds
        {
            xVertice = Mathf.Round(xClick * 2) / 2; // round to nearest 0.5
        }
        if (yClick >= -0.5 && yClick < GridManager.size + 0.5) // check bounds
        {
            yVertice = Mathf.Round(yClick * 2) / 2; // round to nearest 0.5
        }

        // calculate nearest whole number
        float xRound = Mathf.Round(xClick);
        float yRound = Mathf.Round(yClick);

        // makes sure its an intersection and not in centre of tile
        if (xRound == xVertice) xVertice += 0.5f;
        if (yRound == yVertice) yVertice += 0.5f;

        // calculate index of grid where the mouse is pointing
        int xIndex = (int)(xVertice + 0.5f); 
        int yIndex = (int)(yVertice + 0.5f);

        if (Input.GetMouseButtonDown(0)) // check for left mouse click
        {
            if (xVertice != -5 && yVertice != -5 && SearchAlg.CanPlaceStone(xIndex, yIndex))
            {
                // create the stone based on turn
                GameObject stone = Instantiate(PlayerTurn.isBlackTurn ? blackStone : whiteStone);

                // ensure the stones appear infront of the grid
                stone.GetComponent<SpriteRenderer>().sortingOrder = 2;
                stone.transform.position = new Vector3(xVertice, yVertice, 0f); // place the stone
                SearchAlg.placedStone[xIndex, yIndex] = PlayerTurn.isBlackTurn ? 1 : 2; // mark stone as placed

                // add stone to dictionary
                stonesOnBoard[new Vector2Int(xIndex, yIndex)] = stone;

                List<Vector2Int> capturablePieces = new List<Vector2Int>(); // new list to store possible captures
                capturablePieces = SearchAlg.CapturablePieces(xIndex, yIndex); // get capturable pieces

                // Debug for checking if stone removal works
                // string capturableStonesString = "Removable Stones: ";
                // foreach (Vector2Int capturableStone in capturablePieces) // for each removable stone
                // {
                //     capturableStonesString += $" x: {capturableStone.x}, y: {capturableStone.y} |";
                // }

                // Debug.Log(capturableStonesString);

                // Prevent replayable action

                if (capturablePieces.Count == 1) // if one piece captured
                {
                    foreach (Vector2Int capturableStone in capturablePieces) // for the stone
                    {
                        PlayerTurn.protectedLocation = capturableStone; // protect the location
                    }
                }
                else
                {
                    PlayerTurn.protectedLocation = new Vector2Int(-1, -1); // no longer protected, as a new board configuration has been achieved
                }

                int index = 0; // used for counting captures
                int colour = PlayerTurn.isBlackTurn ? 1 : 2; // get the player
                // for every capturable piece
                foreach (Vector2Int capturableStone in capturablePieces)
                {
                    // get target stone to remove
                    GameObject stoneToRemove = GetStoneAtPosition(capturableStone.x, capturableStone.y);
                    if (stoneToRemove != null) // if there is a stone here
                    {
                        index++; // count the stone removal
                        Destroy(stoneToRemove); // remove game object from the game
                        Vector2Int targetRemovable = new Vector2Int(capturableStone.x, capturableStone.y); // vector for removal
                        stonesOnBoard.Remove(targetRemovable); // remove from dictionary
                        SearchAlg.placedStone[capturableStone.x, capturableStone.y] = 0; // "place" empty stone
                    }
                }

                PlayerTurn.playerCaptures[colour - 1] += index; // add amount of captures
                SearchAlg.SetTerritory(); // set the territory of that player
                PlayerTurn.numberOfPasses = 0; // reset the pass counter
                PlayerTurn.PassTurn(); // toggle turn
            }
            else
            {
                Debug.LogWarning("Null Pointer");
            }

            // Debugging
            Debug.Log("Mouse Position: " + mousePos);
        }

        // Highlight Logic
        if (SearchAlg.placedStone[xIndex, yIndex] != 0) // if stone placed here
        {

            // highlight connected stones
            highlight = SearchAlg.GetConnectedStones(xIndex, yIndex); // get connected stones
            if (highlight.Count > 1) // if it has a connection
            {
                foreach (Vector2Int stoneToHighlight in highlight) // for every connection
                {
                    if (!highlightedStones.ContainsKey(stoneToHighlight)) // Check if already highlighted
                    {
                        GameObject highlighted = Instantiate(highlightPrefab); // instantiate the highlight tile
                        highlightedStones[stoneToHighlight] = highlighted; // add to highlighted stone dictionary
                        highlighted.transform.position = new Vector3(stoneToHighlight.x - 0.5f, stoneToHighlight.y - 0.5f, 0f); // Correctly position the highlight
                        highlighted.GetComponent<SpriteRenderer>().sortingOrder = 3; // place on top of stones
                    }
                }

                // highlight territory

                List<Vector2Int> emptyAdjacents = new List<Vector2Int>(); // list for adjacent empty spaces (includes diagonals)
                emptyAdjacents = SearchAlg.GetAdjacentEmpties(xIndex, yIndex); // set the list
                foreach (Vector2Int emptyPoint in emptyAdjacents) // for every empty point
                {
                    if (SearchAlg.ownedTerritory[emptyPoint.x, emptyPoint.y] != 0) // if empty is territory
                    {
                        List<Vector2Int> territoryToHighlight = new List<Vector2Int>(); // list of territory to highlight
                        territoryToHighlight = SearchAlg.GetConnectedStones(emptyPoint.x, emptyPoint.y); // get connected empties (corners not includes)
                        foreach (Vector2Int territory in territoryToHighlight) // for every piece of territory
                        {
                            if (!highlightedTerritory.ContainsKey(territory)) //  if not highlighted already
                            {
                                GameObject highlighted = new GameObject(); // create the object
                                if (SearchAlg.ownedTerritory[emptyPoint.x,emptyPoint.y] == 1) // if its a black territory
                                {
                                     highlighted = Instantiate(blackTerritory); // instantiate the balck territory highlight tile
                                }
                                if (SearchAlg.ownedTerritory[emptyPoint.x,emptyPoint.y] == 2) // if its white territory
                                {
                                     highlighted = Instantiate(whiteTerritory); // instantiate the balck territory highlight tile
                                }
                                highlightedTerritory[territory] = highlighted; // add to highlighted territory dictionary
                                highlighted.transform.position = new Vector3(territory.x - 0.5f, territory.y - 0.5f, 0f); // Correctly position the highlight
                                highlighted.GetComponent<SpriteRenderer>().sortingOrder = 3; // place on top of stones
                            }
                        }
                    }
                }
            }

            // highlight liberties
            List<Vector2Int> libertiesToHighlight = new List<Vector2Int>(); // create list for storing liberties
            libertiesToHighlight = SearchAlg.GetLiberties(xIndex, yIndex); // find liberties of stone
            if (libertiesToHighlight.Count > 0){
                foreach (Vector2Int libertyTo in libertiesToHighlight) // for every liberty of stone
                {
                    if (!highlightedLiberties.ContainsKey(libertyTo)) // if not highlighted
                    {
                        GameObject highlighted = Instantiate(libertyPrefab); // instantiate the highlight tile
                        highlightedLiberties[libertyTo] = highlighted; // add to highlighted liberty dictionary
                        highlighted.transform.position = new Vector3(libertyTo.x - 0.5f, libertyTo.y - 0.5f, 0f); // Correctly position the highlight
                        highlighted.GetComponent<SpriteRenderer>().sortingOrder = 2; // place on top of board
                    }
                }
            }
        }

  

    }
}
