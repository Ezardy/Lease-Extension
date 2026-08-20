<p align="center"><img width="1088" height="522" alt="banner" src="https://github.com/user-attachments/assets/3384a357-f977-480d-92f7-e48d23de1eb6" /></p>

# Play
* [Unity Play](https://play.unity.com/en/games/fc2eb307-fd13-4d5b-8b4e-bdccd85a8cd7/webgl-release)
* [itch.io](https://peatacho.itch.io/lease-extension)
# Updates Roadmap
The first - the nearest
- [ ] Characters' cosmetics shop
- [ ] Effects: character's speed up/slow down, etc
- [ ] Depth of Level: a player will be able to change their line of flight
# Customization
## Track constructions
### Theory
The most important part of the level in Lease Extension is the **tracks**. **Track** is a line that translates GameObjects along itself - from the right side of a screen to the left side.
The game's level contains 11 tracks in order to create parallax effect, the jail bars use it.  
GameObjects, which are translated by a track, are called **Construction Parts**. It is important, Tracks operate only with **Construction Parts**, not with **Constructions**.  
**Track Orchestrator** places *construction parts* of a *construction* on consequent tracks, which are implied to be spreaded along Y axis. The orchestrator splits the construction by construction parts -  one *construction part* at a *track*.  
**Construction Database** is a set of **Construction Blueprints**. *Track Orchestrator* use it to form a level.  
**Construction Blueprint** describes how to construct a corresponding construction. It consists of **Construction Part Blueprints**, which describe how to construct corresponding construction parts.
### Construction Database
Create the scritpable object with *Create->Scriptable Objects->Construction Database*  
Now you can add a *construction* scriptable object to it
<p align="center"><img width="292" height="236" alt="image" src="https://github.com/user-attachments/assets/3a14bc59-5319-4d53-a420-9caa69c7b230" /></p>

### Construction Blueprints
There are 3 out-of-the-box types of *Construction Blueprint* scriptable objects:
- **Construction Blueprint** - a base class of construction blueprints. It describes when and on which tracks a construction will be spawned and optionally sets a height of the construction.
- **Soaring Construction Blueprint** describes a soaring construction. It also accounts an altitude.
- **Gap Construction Bluepring** - the same as the base class, but is ready for some or all of its *construction part blueprints* implement *IGapConstructionPartBlueprint*  
The scriptable objects can be created via *Create->Scriptable Objects->Blueprints*
<p align="center"><img width="294" height="360" alt="image" src="https://github.com/user-attachments/assets/ea55f7d2-308c-4123-a453-183349a76f95" /></p>

All of the paramteres except *Start Height* and *End Height*, which belong to *Soaring Blueprints*, are the same for all of the types of *Construction Blueprints*:
- **Range Start** and **Range End** - start and points of a range, in which a construction's start track can be randomly set. 0 corresponds to a first track, 1 - to a last track.
- **Is Range Reversed** splits the range of possible start positions into two ranges, so *range start/end* now describes a range where no start track is possible.
- **Height** - a normalized height of a construction. The parameter tries to set a height to underlying construction parts if it is applicable. 0 - floor, 1 - ceil. For example, the bars' heights equal 1.
- **Min/Max Delay** - a range of a random delay in meters, with which a construction starts to be spawned
- **Min/Max Margin** - a range of a random margin in meters, which must be between constructions of the same type. If the distance of a margin is passed, but a track can't accomodate at least one of the construction's parts, the construction will not be placed on the tracks.
- **Parts** - a set of construction parts
- **Start/End Height** - a range of a random altitude. 0 - floor, 1 - ceil

### Construction Part Blueprints
There are 4 out-of-the-box types of construction part blueprints. Basically, they describe what to place on a track.  
The scriptable objects can be created via *Create->Scriptable Objects->Blueprints*
#### Construction Part Blueprint
<p align="center"><img width="294" height="300" alt="image" src="https://github.com/user-attachments/assets/6f32eba7-f779-4d8e-bbc4-ec847a225cae" /></p>

- **Prefab** - prefab for the construction part.
- **Initial pool** - all of the out-of-the-box part blueprints are pooling *construction parts*. This value describes the initial pool size.
- **Width** - a width of a *construction part* to be spawned. *Track Orchestrator* uses this to spawn the *construction part* periodically with maintained margin between *construction parts* of a same type. It is not used for collision check between *construction parts*.
- **Auto Width** - whether a *construction part*'s width is set manually via the parameter above, or it can be detected, if the *construction part blueprint*'s prefab contains SpriteRenderer or BoxCollider2D.
- **Margin** is used to add an extra width to a *construction part*. It is usefull when *auto width* is set.
- **Interfere Width** is used by *Track Orchestrator* for the collision check specially.
- **Auto Interfere Width** - the same as **Auto Width**, but for *Interfere Width*
- **Same Width** - whether to use *Width* as *Interefere Width* too
- **Use Sprite Renderer** - whether to use SpriteRenderer or BoxCollider2D to detect *Width*
- **Z ordering** - whether to set z value of a transform.position over a render order setting.*Track* set render order for SpriteRenderers in order to get parallax effect.
#### Behaviour Construction Part Blueprint
It has the same parameters as Construction Part Blueprint. The only difference is this type of part blueprints implies presence of **AConstructionPartBehaviour** inherited component on the prefab:  
```
internal abstract class AConstructionPartBehaviour : MonoBehaviour {
  public class Factory : PlaceholderFactory<Object, AConstructionPartBehaviour> { }
}
```
You can define a behaviour for the *construction part*.
#### Gap Construction Part Blueprint 
This is a composite Construction Part Blueprint. It stacks *construction parts* on top of each other. It was specially developed for 3-component full-size (from floor to ceil) structures like gaps in walls.
<p align="center"><img width="295" height="175" alt="image" src="https://github.com/user-attachments/assets/d6176c89-3ee6-4030-bcf4-b9c495ba135f" /></p>

- **Bottom/Gap/Top Part** - IConstructionPartBlueprint-inherited objects such as a ConstructionPartBlueprint scriptable object.
- **Gap Size** - the size of the gap part in normalized height.
#### Virtual Construction Part Blueprint
This type of *construction part blueprints* has no prefab. It is only a placeholder. For example, it is used as gap in background and foreground cell bars.
<p align="center"><img width="293" height="144" alt="image" src="https://github.com/user-attachments/assets/daddbfc8-eeff-4f33-9045-7879dd662f6b" /></p>

---

<p align="center"><img width="603" height="335" alt="thumbnail" src="https://github.com/user-attachments/assets/b22f7d09-d243-4b39-b7a9-76cba741ecb8" /></p>
