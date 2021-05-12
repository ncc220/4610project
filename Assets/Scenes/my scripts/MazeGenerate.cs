using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerate : MonoBehaviour
{
//     public GameObject prefab;
//     // Start is called before the first frame update
//    public void GenerateMaze(bool[,] mazeArray){
//        var size = mazeArray.GetLength(dimension 0);
//        for(int x =0; x< size; x++){
//            for int y = 0; y < size; y++){
//                if(mazeArray[x,y] == true){
//                    var quad = Instantiate[prefab];
//                    quad.name = $"block({x}, {y})";
//                    quad.transform.parent = transform;
//                    quad.transform.position = new vec3(x,y);
//                }
//            }
//        }
//    }


//help from https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

    public bool showDebug;
    public float placementThreshold = .1f;
    private MeshGenerator meshGen;

    public float hallWidth
    {
        get; private set;
    }
    public float hallHeight
    {
        get; private set;
    }

    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }

    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }
 
    
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material endMat;


    public int[,] data
    {
        get; private set;
    }


    void Awake()
    {
        meshGen = new MeshGenerator();
    }
    
public void GenerateNewMaze(int sizeRows, int sizeCols, TriggerEventHandler startCallback=null, TriggerEventHandler goalCallback=null)
{
    if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
    {
        Debug.LogError("Odd numbers work better for dungeon size.");
    }

    DisposeOldMaze();

    data = FromDimensions(sizeRows, sizeCols);

    FindStartPosition();
    FindGoalPosition();

    // store values used to generate this mesh
    hallWidth = meshGen.width;
    hallHeight = meshGen.height;

    DisplayMaze();

    PlaceStartTrigger(startCallback);
    PlaceGoalTrigger(goalCallback);
}

    void OnGUI()
{
    
    if (!showDebug)
    {
        return;
    }

    
    int[,] maze = data;
    int rMax = maze.GetUpperBound(0);
    int cMax = maze.GetUpperBound(1);

    string msg = "";

    
    for (int i = rMax; i >= 0; i--)
    {
        for (int j = 0; j <= cMax; j++)
        {
            if (maze[i, j] == 0)
            {
                msg += "....";
            }
            else
            {
                msg += "==";
            }
        }
        msg += "\n";
    }

    
    GUI.Label(new Rect(20, 20, 500, 500), msg);
}


    
public int[,] FromDimensions(int rows, int cols)
{
    int[,] maze = new int[rows, cols];
        
    int rMax = maze.GetUpperBound(0);
    int cMax = maze.GetUpperBound(1);

    for (int i = 0; i <= rMax; i++)
    {
        for (int j = 0; j <= cMax; j++)
        {
            
            if (i == 0 || j == 0 || i == rMax || j == cMax)
            {
                maze[i, j] = 1;
            }

            
            else if (i % 2 == 0 && j % 2 == 0)
            {
                if (Random.value > placementThreshold)
                {
                    
                    maze[i, j] = 1;

                    int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    maze[i+a, j+b] = 1;
                }
            }   
        }
    }


    return maze;
}

private void DisplayMaze()
{
    GameObject go = new GameObject();
    go.transform.position = Vector3.zero;
    go.name = "Procedural Maze";
    go.tag = "Generated";
    go.layer = 8;

    MeshFilter mf = go.AddComponent<MeshFilter>();
    mf.mesh = meshGen.FromData(data);
    
    MeshCollider mc = go.AddComponent<MeshCollider>();
    mc.sharedMesh = mf.mesh;

    MeshRenderer mr = go.AddComponent<MeshRenderer>();
    mr.materials = new Material[2] {mazeMat1, mazeMat2};
}

public void DisposeOldMaze()
{
    GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
    foreach (GameObject go in objects) {
        Destroy(go);
    }
}

private void FindStartPosition()
{
    int[,] maze = data;
    int rMax = maze.GetUpperBound(0);
    int cMax = maze.GetUpperBound(1);

    for (int i = 0; i <= rMax; i++)
    {
        for (int j = 0; j <= cMax; j++)
        {
            if (maze[i, j] == 0)
            {
                startRow = i;
                startCol = j;
                return;
            }
        }
    }
}

private void FindGoalPosition()
{
    int[,] maze = data;
    int rMax = maze.GetUpperBound(0);
    int cMax = maze.GetUpperBound(1);

    // loop top to bottom, right to left
    for (int i = rMax; i >= 0; i--)
    {
        for (int j = cMax; j >= 0; j--)
        {
            if (maze[i, j] == 0)
            {
                goalRow = i;
                goalCol = j;
                return;
            }
        }
    }
}

private void PlaceStartTrigger(TriggerEventHandler callback)
{
    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    go.transform.position = new Vector3(startCol * hallWidth, .5f, startRow * hallWidth);
    go.name = "Start Trigger";
    go.tag = "Generated";

    go.GetComponent<BoxCollider>().isTrigger = true;
    go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

    TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
    tc.callback = callback;
}

private void PlaceGoalTrigger(TriggerEventHandler callback)
{
    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    go.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
    go.name = "end";
    go.tag = "Generated";

    go.GetComponent<BoxCollider>().isTrigger = true;
    go.GetComponent<MeshRenderer>().sharedMaterial = endMat;

    TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
    tc.callback = callback;
}

}
