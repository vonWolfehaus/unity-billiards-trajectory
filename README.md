![](trajectory-prediction.jpg)

Built with Unity 2019.3.7 with these extra packages:
- ProBuilder
- ProGrids (preview)

I exposed Inspector controls in the `TrajectoryController` script of the GameController object. Changing any of the force variables there in the Inspector will update the trajectory in the game view.

Controls:
- Left-click on a ball to select it and see its trajectory
- Left-click and hold to orbit the camera
- Right-click to shoot the active ball

### Overview

The videos I learned from used some practices that adversely affect performance which I wanted to avoid, and to do so the architecture I ended up with is a little different.

At the top is the GameController with individual scripts for handling ball selection (`BallSelection`), then scene management and player input (`TrajectoryController`).

The hidden physics scene is generated on `TrajectoryController.Awake`. If values in that script in the inspector are changed, it will update the active ball's trajectory.

The balls and the table have the `SimulatedPiece` script which creates a clone of itself, destroys or hides its renderers, and moves that hidden clone over to the hidden physics scene. Every time the trajectory changes or a new ball is selected, its Reset method will be called that moves the hidden ball back to where the visible ball is.

`BallController` on each ball takes care of updating the LineRenderer whenever `SimulatedPiece.SimulateImpulse` runs the simulation.

### Why

I was building a billiards-style game when Unity 2018.3 was released and multi-scene physics along with it. Accurate trajectory prediction has always been a challenge but this feature greatly simplifies the solution using built-in physics. 

In Unity's [blog post](https://blogs.unity3d.com/2018/11/12/physics-changes-in-unity-2018-3-beta/) there is a billiard ball video of the feature in action but as far as I know it was never released (UPDATE: [released here](https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/PhysicsSamples)), so thanks to some other user-made videos ([this](https://www.youtube.com/watch?v=GLu1T5Y2SSc) and [this](https://www.youtube.com/watch?v=DcGiUcfLbes)) I was able to put this solution together and decided to share it with everyone since I know there are others wondering as well. 

I'm still a total novice to Unity so I'd appreciate pull requests to help me (and everyone looking at this) improve. However, I don't have the bandwidth to maintain this often so [here's the forum thread](https://forum.unity.com/threads/multi-scene-physics-billiards-example-for-trajectory-prediction.854695/) that others might have answers for questions you may have.
