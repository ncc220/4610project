using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerate : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
   public void GenerateMaze(bool[,] mazeArray){
       var size = mazeArray.GetLength(dimension 0);
       for(int x =0; x< size; x++){
           for int y = 0; y < size; y++){
               if(mazeArray[x,y] == true){
                   var quad = Instantiate[prefab];
                   quad.name = $"block({x}, {y})";
                   quad.transform.parent = transform;
                   quad.transform.position = new vec3(x,y);
               }
           }
       }
   }
}
