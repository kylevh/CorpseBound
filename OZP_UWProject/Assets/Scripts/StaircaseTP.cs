using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaircaseTP : MonoBehaviour
{
    public GameObject Stair, Player;
    public float offsetX, offsetY;
    public float upOrDown;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController.instance.disableMovement = true;
            PlayerController.instance.stairCaseAnimation = true;
            stairTransistion();
        }
    }

    void stairTransistion()
    {
        UITransistions.ui.fadeAnim(2, 1.5f);
        StartCoroutine(Teleport());

    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(.5f);
        Player.transform.position = new Vector3(Stair.transform.position.x + offsetX, Stair.transform.position.y + offsetY, Stair.transform.position.z);
        PlayerController.instance.disableMovement = false;
        PlayerController.instance.stairCaseAnimation = false;
        yield return new WaitForSeconds(.5f);
        UITransistions.ui.fadeAnim(0, 1);
    }


}
