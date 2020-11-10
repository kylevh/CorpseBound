using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UITransistions : MonoBehaviour
{
    public static UITransistions ui;
    [SerializeField] public GameObject fade;
    // Start is called before the first frame update
    void Start()
    {
        //ALL UI OBJECTS SHOULD BE SET TO INACTIVE
        ui = this;
        fade = GameObject.Find("fade");
    }

    public void fadeAnim(float opacity, float duration)
    {
        fade.GetComponent<Image>().DOFade(opacity, duration);
            
    }
}
