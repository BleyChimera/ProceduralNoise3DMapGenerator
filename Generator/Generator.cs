using Godot;
using System;
using System.Threading;

public class Generator : Spatial
{
    //Noise
    OpenSimplexNoise NoiseGenerator;

    //Gridmaps
    private GridMap FloorGrid;
    private GridMap CeilingGrid;
    private GridMap WallsGrid1;
    private GridMap WallsGrid2;
    private GridMap WallsGrid3;
    private GridMap WallsGrid4;

    //Vector3 for information of generation
    private Vector3 SideOffSet;

    //Thread to not overload entering a level
    private System.Threading.Thread Threadworld;

    // Start the world generation
    public override void _Ready()
    {
        FloorGrid = GetNode<GridMap>("Floor");
        CeilingGrid = GetNode<GridMap>("Ceiling");
        WallsGrid1 = GetNode<GridMap>("Walls1");
        WallsGrid2 = GetNode<GridMap>("Walls2");
        WallsGrid3 = GetNode<GridMap>("Walls3");
        WallsGrid4 = GetNode<GridMap>("Walls4");

        NoiseGenerator = new OpenSimplexNoise();
        NoiseGenerator.Seed = (int)GD.Randi();

        NoiseGenerator.Octaves = 3;
        NoiseGenerator.Period = 9.0f;
        NoiseGenerator.Lacunarity = 2.0f;
        NoiseGenerator.Persistence = 1.0f;

        SideOffSet = new Vector3(51,0,0);
        System.Threading.Thread Threadworld = new System.Threading.Thread(new ThreadStart(GenerateWorld));
        Threadworld.Priority = ThreadPriority.Lowest;
        Threadworld.IsBackground = true;
        Threadworld.Start();
    }

    public void StartGen()
    {
        GD.Print("Starting");
        SideOffSet = new Vector3(51,100,0);
        Threadworld = new System.Threading.Thread(new ThreadStart(GenerateWorld));
        Threadworld.Priority = ThreadPriority.Lowest;
        Threadworld.IsBackground = true;
        Threadworld.Start();
    }

    // Generate the world with the noise
    public async void GenerateWorld()
    {
        GD.Print("Generating");

        for (int x=-(int)SideOffSet.x+(int)SideOffSet.y; x<((int)SideOffSet.x+(int)SideOffSet.y); x++)
        {
            for (int z=-(int)SideOffSet.x+(int)SideOffSet.z; z<((int)SideOffSet.x+(int)SideOffSet.z); z++)
            {
                //Set floor and ceiling tiles to 0 plus walls for good measure
                WallsGrid1.SetCellItem(x,0,z,-1); WallsGrid2.SetCellItem(x,0,z,-1); WallsGrid3.SetCellItem(x,0,z,-1); WallsGrid4.SetCellItem(x,0,z,-1);

                if (NoiseGenerator.GetNoise2d(x,z) > 0)
                {
                    //Set the floor and ceiling tiles to their respective cells
                    FloorGrid.SetCellItem(x,0,z,0); CeilingGrid.SetCellItem(x,0,z,1);
                }
            }
            await System.Threading.Tasks.Task.Delay(150);
        }

        for (int x=-(int)SideOffSet.x+(int)SideOffSet.y; x<((int)SideOffSet.x+(int)SideOffSet.y); x++)
        {
            for (int z=-(int)SideOffSet.x+(int)SideOffSet.z; z<((int)SideOffSet.x+(int)SideOffSet.z); z++)
            {
                //Check floor for walls
                if (FloorGrid.GetCellItem(x,0,z) != -1)
                {
                    int[] WallsCheckVar = new int[4] {0,0,0,0};
                    if (FloorGrid.GetCellItem(x+1,0,z) == -1)
                    {WallsCheckVar[0] = 1;}
                    if (FloorGrid.GetCellItem(x-1,0,z) == -1)
                    {WallsCheckVar[1] = 1;}
                    if (FloorGrid.GetCellItem(x,0,z+1) == -1)
                    {WallsCheckVar[2] = 1;}
                    if (FloorGrid.GetCellItem(x,0,z-1) == -1)
                    {WallsCheckVar[3] = 1;}

                    //Set walls depending on combination
                    if (WallsCheckVar[0] == 1)
                    {
                        WallsGrid1.SetCellItem(x,0,z,2,-10);
                    }

                    if (WallsCheckVar[1] == 1)
                    {
                        WallsGrid2.SetCellItem(x,0,z,2,16);
                    }

                    if (WallsCheckVar[2] == 1)
                    {
                        WallsGrid3.SetCellItem(x,0,z,2,10);
                    }

                    if (WallsCheckVar[3] == 1)
                    {
                       WallsGrid4.SetCellItem(x,0,z,2,0);
                    }
                }
            }
            await System.Threading.Tasks.Task.Delay(150);
        }

        GD.Print("Generated");
    }



/*  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {

  }
*/
}
