using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused = false;
    public static GameManager gm;
    public GameObject winCon;
    public GameObject door;
    public GameObject message;
    public void Awake()
    {
        gm = this;
        message.SetActive(false);

    }
    void Update()
    {

        if (winCon == null)
        {
            door.SetActive(false);
            message.SetActive(true);
        }
    }

    public void PauseGame()
    {
        if (!gameIsPaused)
        {
            Time.timeScale = 0f;
            gameIsPaused = true;

        }

    }


    public void ResumeGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }


}
