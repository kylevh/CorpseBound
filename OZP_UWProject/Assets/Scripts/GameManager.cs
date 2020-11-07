using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;
    public float healthPercent = 1f;

    void Start()
    {
        healthBar.SetSize(1f);
        healthBar.SetColor(Color.white);
    }

    void Update()
    {
        
    }

    public void damage(int DAMAGE)
    {

        /*Animator anim1 = player.GetComponent<Animator>(); //To be implemented*
        anim1.SetTrigger("hurt");*/ 
    }
}
