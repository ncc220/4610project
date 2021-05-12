using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeGenerate))]

public class GameController : MonoBehaviour
{
    private MazeGenerate generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<MazeGenerate>();

        generator.GenerateNewMaze(13, 15);
    }


}
