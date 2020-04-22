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

### Naming convention!

All the instance variables in Behaviour classes whose values are obtained
from Data Scriptable Objects are prefixed with a `d`.
So for example, the private instance variable that controls the rate of
fire of a `ShooterBehaviour` is called `_dFireRate`.