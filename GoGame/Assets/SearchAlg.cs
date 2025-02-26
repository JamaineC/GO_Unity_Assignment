using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// class responsible for all the search algorithms
/// </summary>
public class SearchAlg : MonoBehaviour
{

    public static int[,] placedStone = new int[GridManager.size,GridManager.size]; // 0 = empty | 1 = black | 2 = white

     public static int[,] ownedTerritory = new int[GridManager.size,GridManager.size]; // 0 = neutral, 1 = black, 2 = white
    public static List<Vector2Int> emptyPoints = new List<Vector2Int>(); // list for all empty points on board

        /// <summary>
        /// add all empty points in a list
        /// </summary>
        public static void SetListOfEmptys()
    {
        emptyPoints.Clear(); // Clear the list at the beginning

        // Add all empty points to the list initially
        for(int i = 0; i < GridManager.size; i++)
        {
            for (int j = 0; j < GridManager.size; j++)
            {
                if (placedStone[i,j] == 0)
                {
                    emptyPoints.Add(new Vector2Int(i, j));
                }
            }
        }
    }

        /// <summary>
    /// used to get the adjacent positons for marking connections
    /// </summary>
    /// <param name="x"> x coordinate of position</param>
    /// <param name="y"> y coordinate of position</param>
    /// <returns> a list of adjacents excluding diagonals </returns>
    public static List<Vector2Int> GetAdjacents(int x, int y)
    {
        List<Vector2Int> adjacentPositions = new List<Vector2Int>();

        if (x > 0)
        {
            adjacentPositions.Add(new Vector2Int(x - 1, y)); // Left
        }
        if (x < GridManager.size - 1)
        {
            adjacentPositions.Add(new Vector2Int(x + 1, y)); // Right
        }
        if (y > 0)
        {
            adjacentPositions.Add(new Vector2Int(x, y - 1)); // Down
        }
        if (y < GridManager.size - 1)
        {
            adjacentPositions.Add(new Vector2Int(x, y + 1)); // Up
        }

        return adjacentPositions;
    }

        /// <summary>
    /// used for marking terrirory, diagonals need to be covered
    /// </summary>
    /// <param name="x"> x coordinate of position </param>
    /// <param name="y"> y coordinate of position</param>
    /// <returns> a list of all adjacents including diagonals </returns>
    public static List<Vector2Int> GetAdjacentEmpties(int x, int y)
    {
        List<Vector2Int> adjacentPositions = new List<Vector2Int>();

        if (x > 0)
        {
            adjacentPositions.Add(new Vector2Int(x - 1, y)); // Left
        }
        if (x < GridManager.size - 1)
        {
            adjacentPositions.Add(new Vector2Int(x + 1, y)); // Right
        }
        if (y > 0)
        {
            adjacentPositions.Add(new Vector2Int(x, y - 1)); // Down
        }
        if (y < GridManager.size - 1)
        {
            adjacentPositions.Add(new Vector2Int(x, y + 1)); // Up
        }
        if (x > 0 && y < GridManager.size -1)
        {
            adjacentPositions.Add(new Vector2Int(x - 1, y + 1)); // Up-Left
        }
        if (x < GridManager.size - 1 && y < GridManager.size - 1)
        {
            adjacentPositions.Add(new Vector2Int(x + 1, y + 1)); // Up-Right
        }
        if (x > 0 && y > 0)
        {
            adjacentPositions.Add(new Vector2Int(x - 1, y - 1)); // Down-Left
        }
        if (x < GridManager.size - 1 && y > 0)
        {
            adjacentPositions.Add(new Vector2Int(x + 1, y - 1)); // Down-Right
        }

        return adjacentPositions;
    }

    /// <summary>
    /// function to get all stones within a connection
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns> a list of connected stones </returns>
    public static List<Vector2Int> GetConnectedStones(int x, int y)
    {
        bool[,] isChecked = new bool[GridManager.size, GridManager.size];
        List<Vector2Int> connectedStones = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();


        // CRUCIAL FOR STONE PLACEMENT TO WORK CORRECTLY
        int maxLinks = 30; // prevents first tile thinking everything is a territory and limits size of territory
        int colour = placedStone[x, y]; // get colour of stone placed
        queue.Enqueue(new Vector2Int(x, y)); // queue this position
        isChecked[x, y] = true; // mark position as checked

        while (queue.Count > 0) // while queue is not empty
        {
            // add current connected stone to the list of connected stone and remove from queue
            Vector2Int current = queue.Dequeue();
            connectedStones.Add(current);

            // Debug.Log($"Checking stone at ({current.x}, {current.y})");

            // if it is a stone
            if (colour != 0)
            {
                // get adjacent positions
                List<Vector2Int> adjacentPositions = GetAdjacents(current.x, current.y);

                foreach (Vector2Int adjacentPosition in adjacentPositions) // for each adjacent
                {
                    // if not checked and the same color
                    if (!isChecked[adjacentPosition.x, adjacentPosition.y] && placedStone[adjacentPosition.x, adjacentPosition.y] == colour)
                    {
                        queue.Enqueue(adjacentPosition); // add to queue
                        isChecked[adjacentPosition.x, adjacentPosition.y] = true; // mark as checked
                        // Debug.Log($"Found connected stone at ({adjacentPosition.x}, {adjacentPosition.y})");
                    }
                }
            }
            else 
            {
                // get adjacent positions
                List<Vector2Int> adjacentPositions = GetAdjacentEmpties(current.x, current.y);

                foreach (Vector2Int adjacentPosition in adjacentPositions) // For each adjacent
                {
                    // if not checked and  same color
                    if (!isChecked[adjacentPosition.x, adjacentPosition.y] && placedStone[adjacentPosition.x, adjacentPosition.y] == colour)
                    {
                        queue.Enqueue(adjacentPosition); // Add to queue
                        isChecked[adjacentPosition.x, adjacentPosition.y] = true; // Mark as checked
                        // Debug.Log($"Found connected stone at ({adjacentPosition.x}, {adjacentPosition.y})");
                    }
                }
            }
        }

        //  if the number of connected empties is more than the maxLinks
        if (connectedStones.Count > maxLinks || connectedStones.Count == 0)
        {
            // if the number of connected empties exceeds the limit, return an empty list
            return new List<Vector2Int>();
        }

        return connectedStones;
    }

    /// <summary>
    /// function to check the surrounding stones of empty connections to make sure they are territory
    /// </summary>
    /// <param name="connection"> takes in a list of empties to find out if it is terrioty or not </param>
    /// <returns> the colour of the territory if one is found </returns>
    public static int CheckSurroundingStones(List<Vector2Int> connection)
    {
        HashSet<int> surroundingColours = new HashSet<int>();
        
        foreach (Vector2Int point in connection) // for every point in connection of empties
        {
            List<Vector2Int> adjacents = GetAdjacents(point.x, point.y);
            foreach (Vector2Int adjacent in adjacents) // for each adjacent
            {
                int colour = placedStone[adjacent.x, adjacent.y]; // find what stone it is
                if (colour != 0) // if colour around it 
                {
                    surroundingColours.Add(colour); // add this colour 
                }

            }
        }

        if (surroundingColours.Count == 1) // if only one colour surrounds
        {
            foreach (int colour in surroundingColours) 
            {
                return colour; // return the colour
            }
        }

        return 0; //  if surrounded by different colors or no surrounding stones then its no territory
    }






    /// <summary>
    /// function to set the territory of all pieces on the board
    /// </summary>
    public static void SetTerritory()
    {
        SetListOfEmptys();
        bool[,] isChecked = new bool[GridManager.size, GridManager.size];

        foreach (Vector2Int point in emptyPoints) // for every empty point
        {
            if (!isChecked[point.x, point.y]) // if not already checked
            {
                List<Vector2Int> connection = GetConnectedStones(point.x, point.y); // get connected stones
                int colour = CheckSurroundingStones(connection); // check the borders to make sure it is territory and mark this colour
                isChecked[point.x, point.y] = true; // mark point as checked
                if (colour != 0)  // if a stone surrounds
                {
                    foreach (Vector2Int connectionPoint in connection) // for every connection
                    {
                        ownedTerritory[connectionPoint.x, connectionPoint.y] = colour; // mark it as the correct territory
                        Debug.Log($"Territory of Colour {colour} has been marked"); // debug
                    }
                }  

            } 
        }
    }


    /// <summary>
    /// function to check if a stone has liberties
    /// </summary>
    /// <param name="x"> x coordinate of stone</param>
    /// <param name="y"> y coordinate of stone </param>
    /// <returns> whether or not it has liberties </returns>
    public static bool HasLiberties(int x, int y)
    {
        List<Vector2Int> connectedStones = new List<Vector2Int>();
        connectedStones = GetConnectedStones(x,y);

        foreach (Vector2Int stone in connectedStones)  // for each stone in that connection
        {
            List<Vector2Int> adjacentPositions = new List<Vector2Int>();
            adjacentPositions = GetAdjacents(stone.x,stone.y); // get adjacent stones
            foreach (Vector2Int adjacent in adjacentPositions) // for each adjacent
            {
                int colour = placedStone[adjacent.x, adjacent.y];
                if (colour == 0) // if it is a liberty
                {
                    Debug.Log("Connection has Liberties");
                    return true;
                } 
            }
        }

        Debug.Log("No Liberties found!");
        return false;
    }

    /// <summary>
    /// function to get all the liberties, used later for highlighting liberties
    /// </summary>
    /// <param name="x"> x coordinate of stone </param>
    /// <param name="y"> y coordinate of stone </param>
    /// <returns> a list of all the positions where there are liberties </returns>
    public static List<Vector2Int> GetLiberties(int x, int y){
        List<Vector2Int> liberties = new List<Vector2Int>();
        if (HasLiberties(x, y)) // if has liberties
        {
            List<Vector2Int> connections = GetConnectedStones(x,y); // get connected stones
            foreach (Vector2Int connection in connections) // for each stone
            {
                List<Vector2Int> adjacents = GetAdjacents(connection.x,connection.y); // get adjacent objects
                foreach (Vector2Int adjacent in adjacents) // for each adjacent object
                {
                    // if not owened territory or a placed stone
                    if (placedStone[adjacent.x,adjacent.y] == 0 && ownedTerritory[adjacent.x,adjacent.y] == 0) 
                    {
                        liberties.Add(adjacent); //add to list of liberties
                    }
                }
                
            }
        }
        else Debug.Log("No liberties found");
        return liberties;

    }


    /// <summary>
    /// function to get the list of capturable pieces after a move is playes
    /// </summary>
    /// <param name="x"> x coordinate of stone placed</param>
    /// <param name="y"> y coordinate of stone placed </param>
    /// <returns> the list of stones that can be removed </returns>
    public static List<Vector2Int> CapturablePieces(int x, int y)
     {
        List<Vector2Int> adjacentPieces = new List<Vector2Int>();
        List<Vector2Int> capturablePieces = new List<Vector2Int>();
        adjacentPieces = GetAdjacents(x, y);
        int colour = PlayerTurn.isBlackTurn ? 1: 2;

        foreach (Vector2Int adjacentPiece in adjacentPieces)
        {
            int adjacentColour =  placedStone[adjacentPiece.x, adjacentPiece.y];

            // if stone placed is opposite colour to adjacent intersection
            if ( (colour == 1 && adjacentColour == 2) || ( colour == 2 && adjacentColour == 1)) 
            {
                // if no liberties found on adjacent stone
                if (!HasLiberties(adjacentPiece.x,adjacentPiece.y))
                {
                    List<Vector2Int> removables = new List<Vector2Int>();
                    removables = GetConnectedStones(adjacentPiece.x,adjacentPiece.y);
                    capturablePieces.AddRange(removables);
                }
            }

        }
        return capturablePieces;
     }

    /// <summary>
    /// function to check if it is a legal move
    /// </summary>
    /// <param name="x"> x coordinate of attempted stone place </param>
    /// <param name="y"> y coordinate of attempted stone place </param>
    /// <returns> whether or not the stone can be placed </returns>
    public static bool CanPlaceStone(int x, int y)
    {
        if (placedStone[x, y] != 0) return false; // if stone there already return false
        if (ownedTerritory[x, y] != 0) return false; // if this area owned already return false
        Vector2Int attemptedPlace = new Vector2Int(x,y);
        if (PlayerTurn.protectedLocation == attemptedPlace) return false; // if this repeats old configuration

        int colour = PlayerTurn.isBlackTurn ? 1 : 2;

        // Temporarily place the stone
        placedStone[x, y] = colour;

        // Check for possible captures
        List<Vector2Int> capturedStones = CapturablePieces(x, y);

        if (capturedStones.Count > 0 )
        {

            // Remove the temporary stone
            placedStone[x, y] = 0;

            return true;
        }
        else
        {
            // Check if the placed stone itself has liberties
            bool hasLiberties = HasLiberties(x, y);

            // Remove the temporary stone
            placedStone[x, y] = 0;

            
            return hasLiberties;
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
