# alien-run

## Core mechanics
- Implemented phyics based movement. Player can move around and jump on the platforms. New unity input system was used to control the input. Character also has basic animations (when running and jumping).

- Implemented Chunk based level architecture. Player can move between levels. In order to progress to the finish of the game, player needs to visit the second level, complete a mini game, collect the reward item and then come back to the first level to unlock a passage to the end of the game.
Even though the prototype features only 2 levels, many exit points can be created which can lead to other levels.
What could be improved: On every scene the player at the same point. (previous) Scene based spawn points could be implemented so it looks like the player came back from the same door which lead to the level they came from.

## Item and inventory system
- Player can pick up items from the level. Items can be simple 1x1 or any complex grid based shape.
- Inventory is grid based and has limited storage space. If an item does not fit, it won't be collected.
- Player can arrange their items in the inventory window.
What could be improved: Controller support for inventory window. Even though the rest of the game can be played with both MKB and an Xbox controller, inventory window does not support controller input. 
Inventory items can only be moved with a mouse. There is also a bug in the inventory window which does not allow the player to move their item to certain places (explained why in the inventory window code).

## Map system:
- Not implemented

## Code documentation:
- Bigger systems are documented with comments in their code.

## Multiplatform input support:
- Implemented support for both MKB and Xbox controller by using Unity's new input system.

### Controls
#### Mouse and keyboard
WASD to move, Space to Jump, E to interact and I to open inventory.

#### Controller
Left stick to move, A to Jump, X to interact and Y to open inventory.

### Goal / of the game
- Collect the pink doughnut
- Unlock the first wall
- Move to Snowy level
- Solve the minigame and pick up the Triple Bone Key
- Return to the first level
- Unlock the second wall
- Jump in the pit to finish the game