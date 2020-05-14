# Rock Paper Shooter - DJDIII Project

An arena 3rd-person shooter based on the "rock-paper-scissors" game.

## Team

* Hugo Feliciano - 2180
* Pedro Fernandes - 2180
* Rafael Castro e Silva - 21903059
   
## Project Structure

All data regarding variables that will be iterated upon, altered by design or
make up important data regarding a game object is placed in a scriptable
object, this is to minimize prefab merge conflict fuckery.

The monobehaviour script of a game object should then read from the
apropriate Scriptable Object to get its correct data.

`Scripts/Scriptables`contains the scripts that define the Scriptable Objects.
`Scriptable Objects/` contains the scriptable objects themselves, created by
using the right-click menu of the `Assets` window in the editor.

## Arena generation procedure suggestion

### In Editor

1. **Create a Tower**
   1.  **Create a Tower Piece**
       1. Create Geometry of Tower Piece
       2. Give the created object Mesh Collider and a Rigidbody components
       3. Create an empty and position it on the Piece's "center" - if this is a middle piece then it will be the player's spawn location and direction.
       4. Drag the empty into the "pivot transform" property in the Piece's inspector window.
       5. Make a Prefab out of this object.
   2. Create and empty that will hold all 3 pieces, add to it a ArenaTower component
   3. Add the pieces you want as part of the tower as children of this empty
   4. Position the pieces however you want vertically.
      1. *Script will align all pieces in the x and z axis so all the pivots form a straight vertical line.*
   5. In the ArenaTower component of the empty add the children as "Top", "Mid", "Bot".
   6. Create a prefab of this Empty, this is a *Tower*, put it in a folder with only other *Towers*, or by itself.

2. **Arena Setup**
   1. Create a GameObject with a `ArenaGenerator` component.
   2. Select the amount of Towers to be in that Arena.
   3. **(Optional)** Add to that GameObject all the Tower prefabs you want in the arena.
      1. By default it will select all the towers in the Arena Towers Folder.
   4. **(Optional)** Define a center Tower that will be put in the center of the Arena no matter what.
      1. Default is all towers are treated equal.
   
### In Code

3. **Arena Generation**
   1. Define a Y value to be the base
   2. Define a (X,Z) coordinate as the center of the Arena.
   3. From all the Towers it can use, randomly select the towers up to the previously defined number, these are the *chosen* Towers
      * If a center Tower was defined then immediately chose that one before choosing the others.
   4. In the chosen towers, access the RigidBody components of each piece and set `mass = 0` and turn off Gravity, restrict all rotation axis and increase the Drag and Angular Drag to at least 1.
      * In the center tower set `mass = 99` (higher than 0), and turn off Gravity, restrict all rotation axis and increase the Drag and Angular Drag to at least 1.
   5. Spawn the towers randomly in a small area (> 1 unit away) around the defined center, with the pivot points of the middle pieces of every Tower acting as origins for the GameObjects.
      * If defined, the center Tower will be spawned dead set in the middle.
    * **The physics engine will now push every tower away from each other until no geometry overlap**
      * [*Reference Article*](https://www.gamasutra.com/blogs/AAdonaac/20150903/252889/Procedural_Dungeon_Generation_Algorithm.php)
4. **Additional Setup**
   1. Add a `Box Collider` to every Tower and set `IsTrigger = true`, this will be used to define zones for the AI later.
   2. Spawn players in the pivot of the middle piece of a random tower, looking at the center and both equally distant from it.


### Naming convention!

All the instance variables in Behaviour classes whose values are obtained
from Data Scriptable Objects are prefixed with a `d`.
So for example, the private instance variable that controls the rate of
fire of a `ShooterBehaviour` is called `_dFireRate`.
