using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public GameObject mainCharacter, ghostCharacter;
    public int selectCharacter = 1;
    void Start()
    {
        mainCharacter.gameObject.SetActive(true);
        ghostCharacter.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void switchCharacter()
    {
        switch (selectCharacter)
        {
            case 1:
                selectCharacter = 2;
                mainCharacter.gameObject.SetActive(false);
                ghostCharacter.gameObject.SetActive(true);
                break;

            case 2:
                selectCharacter = 1;
                mainCharacter.gameObject.SetActive(true);
                ghostCharacter.gameObject.SetActive(false);
                break;
        }

     }

    public void switchCharacter(int select) //forces selection instead of automated
    {
        switch (select)
        {
            case 1:
                selectCharacter = 1;
                mainCharacter.gameObject.SetActive(true);
                ghostCharacter.gameObject.SetActive(false);
                break;

            case 2:
                selectCharacter = 2;
                mainCharacter.gameObject.SetActive(false);
                ghostCharacter.gameObject.SetActive(true);
                break;
        }

    }
}
