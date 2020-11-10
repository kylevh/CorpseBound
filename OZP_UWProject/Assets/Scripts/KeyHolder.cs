using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{

    private List<Key.KeyType> keyList;
    public GameObject blueKeyUI;
    public GameObject redKeyUI;
    public GameObject purpleKeyUI;
    public GameObject yellowKeyUI;
    public GameObject greenKeyUI;

    private void Awake()
    {
        blueKeyUI.SetActive(false);
        redKeyUI.SetActive(false);
        purpleKeyUI.SetActive(false);
        yellowKeyUI.SetActive(false);
        greenKeyUI.SetActive(false);
        keyList = new List<Key.KeyType>();
    }


    public void AddKey(Key.KeyType keyType)
    {
        keyList.Add(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Key keyPickedUp = collider.GetComponent<Key>();
        if (keyPickedUp != null)
        {
            AddKey(keyPickedUp.GetKeyType());
            Destroy(keyPickedUp.gameObject);
            if (keyPickedUp.GetKeyType() == Key.KeyType.Blue)
            {
                blueKeyUI.SetActive(true);
            }
            if (keyPickedUp.GetKeyType() == Key.KeyType.Red)
            {
                redKeyUI.SetActive(true);
            }
            if (keyPickedUp.GetKeyType() == Key.KeyType.Green)
            {
                greenKeyUI.SetActive(true);
            }
            if (keyPickedUp.GetKeyType() == Key.KeyType.Purple)
            {
                purpleKeyUI.SetActive(true);
            }
            if (keyPickedUp.GetKeyType() == Key.KeyType.Yellow)
            {
                yellowKeyUI.SetActive(true);
            }

        }

        KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
        if(keyDoor!= null)
        {
            if (ContainsKey(keyDoor.GetKeyType()))
            {

                RemoveKey(keyDoor.GetKeyType());
                if (keyDoor.GetKeyType() == Key.KeyType.Blue)
                {
                    blueKeyUI.SetActive(false);
                }
                if (keyDoor.GetKeyType() == Key.KeyType.Red)
                {
                    redKeyUI.SetActive(false);
                }
                if (keyDoor.GetKeyType() == Key.KeyType.Green)
                {
                    greenKeyUI.SetActive(false);
                }
                if (keyDoor.GetKeyType() == Key.KeyType.Purple)
                {
                    purpleKeyUI.SetActive(false);
                }
                if (keyDoor.GetKeyType() == Key.KeyType.Yellow)
                {
                    yellowKeyUI.SetActive(false);
                }
                keyDoor.OpenDoor();

            }
        }
    }
}
