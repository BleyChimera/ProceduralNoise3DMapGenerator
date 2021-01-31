# ProceduralNoise3DMapGenerator
A perlin noise 3d world generator created by gridmaps, but only stacked in one layer. Uses Godot 3.2.3 Mono.

## Disclaimer
- I do need to note that this may crash. To solve this search for ```await System.Threading.Tasks.Task.Delay(150);``` and make the miliseconds (150 by default) bigger.
- Doesn't include player character/camera (use the ```Game Camera Override``` while running the game)
- Doesn't include materials
- Will need slight tinkering to work on your project, experiment with the noise and time values
