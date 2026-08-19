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
**Track Orchestrator** places *construction parts* of a *construction* on consequent tracks, which are intended to be spreaded along Y axis. The orchestrator splits the construction by construction parts -  one *construction part* at a *track*.  
**Construction Database** is a set of **Construction Blueprints**. *Track Orchestrator* use it to form a level.  
**Construction Blueprint** describes how to construct a corresponding construction. It consists of **Construction Part Blueprints**, which describe how to construct corresponding construction parts.
### Construction Database
Create a scritpable objects with *Create->Scriptable Objects->Construction Database*  
Now you can add a *construction* scriptable object to it
<p align="center"><img width="292" height="236" alt="image" src="https://github.com/user-attachments/assets/3a14bc59-5319-4d53-a420-9caa69c7b230" /></p>

### Construction Blueprint
There are 3 out-of-the-box types of a *Construction Blueprint* scriptable object:
- **Construction Blueprint** - a base class of construction blueprints. It describes when and on which tracks a construction will be spawned and optionally sets a height of the construction.
- **Soaring Construction Blueprint** describes a soaring construction. It also accounts an altitude.
- **Gap Construction Bluepring** - the same as the base class, but is ready for some or all of its *construction part blueprints* implement *IGapConstructionPartBlueprint*
<p align="center"><img width="294" height="360" alt="image" src="https://github.com/user-attachments/assets/ea55f7d2-308c-4123-a453-183349a76f95" /></p>

All of the paramteres except *Start Height* and *End Height*, which belong to *Soaring Blueprints*, are the same for all of the types of *Construction Blueprints*:
- **Range Start** and **Range End** - start and points of a range, in which a construction's start track can be randomly set. 0 corresponds to a first track, 1 - to a last track.
- **Is Range Reversed** splits the range of possible start positions into two ranges, so *range start/end* now describes a range where no start track is possible.
- **Height** - a height of a construction. The parameter tries to set a height to underlying construction parts if it is applicable. 0 - floor, 1 - ceil. For example, the bars' heights equal 1.
- **Min/Max Delay** 

<p align="center"><img width="603" height="335" alt="thumbnail" src="https://github.com/user-attachments/assets/b22f7d09-d243-4b39-b7a9-76cba741ecb8" /></p>
