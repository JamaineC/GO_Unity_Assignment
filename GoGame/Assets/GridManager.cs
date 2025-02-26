using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// creates the 9x9 grid for the stones to be placed on
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject gridPrefab; // prefab for the grid tiles
    [SerializeField] float sizeOfGrid; // size of grid

    private const int level1 = 1; // only playable level 9x9 grid
    private const int level2 = 2; // no longer used
    private const int level3 = 3; // no longer used

    public static int size = 0; // size of the board
    
    // Start is called before the first frame update
    
    void Start()
    {
        // draw the grid
        for (int x = 0; x < sizeOfGrid - 1; x++)
        {
            for (int y = 0; y < sizeOfGrid - 1 ; y++)
            {
                GameObject grid = Instantiate(gridPrefab); //create grid tile object
                grid.GetComponent<SpriteRenderer>().sortingOrder = 1; // make grid appear behind the stones
                grid.transform.position = new Vector3(x, y, 0f); // create grid
            }
        }

        if (SceneLoader.GetActiveScene().buildIndex == level1) size = 9;
        else if (SceneLoader.GetActiveScene().buildIndex == level2) size = 13; // no longer extra levels
        else if (SceneLoader.GetActiveScene().buildIndex == level3) size = 16; // no longer extra levels
        
    }

    void Awake()
    {

    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
