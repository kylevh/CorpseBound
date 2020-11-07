using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthFill;
    public Color healthColor;
    public Color ghostColor;

    public void SetHealth(float health)
    {
        healthFill.value = health;
    }

    public void SetMaxHealth(float health)
    {
        healthFill.maxValue = health;
        healthFill.value = health;
    }

    public void SetColor(int select)
    {
        if (select == 1)
            healthFill.fillRect.GetComponent<Image>().color = healthColor;
        if(select == 2)
            healthFill.fillRect.GetComponent<Image>().color = ghostColor;
    }

    public float getHealth()
    {
        return healthFill.value;
    }
}
