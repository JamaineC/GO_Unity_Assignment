# Turn-Based Go Game – Unity & C#  
## Project Overview  
This project is a **Computer Games and Graphics** assignment from **Aberystwyth University**, focusing on **game development using Unity and C#**. The project involves developing a **turn-based Go game** on a **9x9 grid**, where two players take turns placing stones to capture territory and outscore their opponent. The game includes **turn management, scoring, rule validation, scene transitions, UI controls**, and a structured **modular design**.  

## Prerequisites  
Unity 2022.3.13f1 or later, Visual Studio Code with Unity and C# extensions installed.  

## Installation & Setup  
Clone the repository: `git clone https://github.com/your-repo/go-game-unity.git && cd go-game-unity`  
Open the project in Unity Editor: `Open Unity Hub > Open Project > Select go-game-unity`  
Run the game in **Play Mode** to start testing.  

## Game Features  
The game starts on a **home screen** where players can **start a new game, read instructions, or quit**. The **grid is dynamically generated**, and players take turns placing stones using **mouse input**. The game implements **Go rules**, where **captures occur when a stone is surrounded with no liberties**. If both players pass consecutively, the game ends, and the **final score is calculated based on stones, captured pieces, and territory controlled**. The UI updates dynamically to show **turns, scores, and game status**.  

## Core Components  
The **GridManager** dynamically creates a **9x9 board**, while **StonePlacer** handles **stone interactions, placement, and removal**. The **PlayerTurn** system manages **turns and score updates**, and **GameLogic** controls **game-ending conditions and score calculations**. **SearchAlg** implements **Go's rule validation**, including **territory calculation, capture detection, and adjacency checks**. **SceneLoader** and **UI components** handle **scene transitions, pause menus, and game-over screens**.  

## Troubleshooting  
If the **game does not start**, ensure **Unity and required dependencies are installed**. If **stone placement is incorrect**, verify **SearchAlg logic for capturing and territory validation**. If **scores are miscalculated**, check **GameLogic's final score computation**.  

## Future Improvements  
Enhancements could include **support for larger board sizes**, **AI opponent for single-player mode**, **networked multiplayer**, and **optimized rule validation for performance improvements**.  

## License  
This project is released for **educational and research purposes**. Contributions and improvements are welcome!  
