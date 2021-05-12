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

    public bool showDebug;
    public float placementThreshold = .1f;
    private MeshGenerator meshGen;
 
    
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
    
    public void GenerateNewMaze(int rows, int cols)
    {
        data = FromDimensions(rows, cols);
        DisplayMaze();
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

}
