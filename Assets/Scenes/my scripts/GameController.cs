using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using System;
using UnityEngine.UI;

[RequireComponent(typeof(MazeGenerate))]

//help from https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

public class GameController : MonoBehaviour
{
    
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Text timeLabel;
    [SerializeField] private Text scoreLabel;

    private MazeGenerate generator;

    
    private DateTime startTime;
    private int timeLimit;
    private int reduceLimitBy;

    private int score;
    private bool goalReached;
    private int currentWidth = 13, currentHeight = 15;

    
    void Start() {
        generator = GetComponent<MazeGenerate>();
        StartNewGame();
    }

    
    private void StartNewGame()
    {
        timeLimit = 80;
        reduceLimitBy = 5;
        startTime = DateTime.Now;

        score = 0;
        scoreLabel.text = score.ToString();

        StartNewMaze();
    }

    
    private void StartNewMaze()
    {
        generator.GenerateNewMaze(currentWidth, currentHeight, OnStartTrigger, OnGoalTrigger);

        float x = generator.startCol * generator.hallWidth;
        float y = 1;
        float z = generator.startRow * generator.hallWidth;
        player.transform.position = new Vector3(x, y, z);

        goalReached = false;
        player.enabled = true;

        // restart timer
        timeLimit -= reduceLimitBy;
        startTime = DateTime.Now;
    }

    
    void Update()
    {
        if (!player.enabled)
        {
            return;
        }

        int timeUsed = (int)(DateTime.Now - startTime).TotalSeconds;
        int timeLeft = timeLimit - timeUsed;

        if (timeLeft > 0)
        {
            timeLabel.text = timeLeft.ToString();
        }
        else
        {
            timeLabel.text = "TIME UP";
            //send player to game over screen 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
            Cursor.lockState = CursorLockMode.None; 
            generator.DisposeOldMaze();
            //player.enabled = false;

            //Invoke("StartNewGame", 4);
        }
    }

    
    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Goal!");
        goalReached = true;

        score += 1;
        scoreLabel.text = score.ToString();
        currentHeight += 2;
        currentWidth += 2;

        Destroy(trigger);
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        if (goalReached)
        {
            Debug.Log("Finish!");
            player.enabled = false;

            Invoke("StartNewMaze", 4);
        }
    }
}
