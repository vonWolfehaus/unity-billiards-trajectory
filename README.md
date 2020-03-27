## Trajectory prediction for Unity using multiple physics scenes

Known issues:
- after adjusting the values in the inspector, the trajectory will no longer be accurate

**Instructions**
Built with Unity 2019.3.7 with these extra packages:
- ProBuilder
- ProGrids (preview)

I exposed controls in the TrajectoryController script of the GameController object. Changing any of the force variables will update the trajectory in the game view.

Controls:
- Left-click on a ball to select it and see its trajectory
- Left-click and hold to orbit the camera
- Right-click to shoot the active ball

**Why**
I was building a billiards-style game when Unity 2018.3 was released and multi-scene physics along with it. Accurate trajectory prediction has always been a challenge but this feature greatly simplifies the solution using built-in physics. 

In Unity's [blog post](https://blogs.unity3d.com/2018/11/12/physics-changes-in-unity-2018-3-beta/) there is a billiard ball video of the feature in action but as far as I know it was never released, which is pretty frustrating, so thanks to some other user-made videos ([this](https://www.youtube.com/watch?v=GLu1T5Y2SSc) and [this](https://www.youtube.com/watch?v=DcGiUcfLbes)) I was able to put this solution together and decided to share it with everyone since I know there are others wondering as well. 

I'm still a total novice to Unity so I'd appreciate pull requests to help me (and everyone looking at this) improve. However, I don't have the bandwidth to maintain this often so here's the forum thread that others might have answers for questions you may have.

### Overview

The videos I learned from used some practices that adversely affect performance which I wanted to avoid, and to do so the architecture I ended up with is a little different.

At the top is the GameController with individual scripts for handling ball selection (BallSelection), then scene management and player input (TrajectoryController).

_...TODO_