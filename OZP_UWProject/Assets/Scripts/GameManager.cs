using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;
    void Start()
    {
        healthBar.SetSize(.4f);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetColor(Color.white);
    }
}
