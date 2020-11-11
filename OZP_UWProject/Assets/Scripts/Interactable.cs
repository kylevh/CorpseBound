using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public bool canInteract = true;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    public GameObject interactUI;
    private Image image;
    private Text text;
    public Vector3 offset;
    void Start()
    {
        interactUI.GetComponent<Image>().DOFade(0f, 0f);
        interactUI.GetComponentInChildren<Text>().DOFade(0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && canInteract)
        {
            interactAppear();
            if(Input.GetKeyDown(interactKey) && canInteract == true)
            {
                interactAction.Invoke();
            }

        }
        if(!isInRange || !canInteract)
        {
            interactDissappear();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ghostPlayer"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ghostPlayer"))
        {
            isInRange = false;
        }
    }

    public void interactAppear()
    {
        interactUI.GetComponent<Image>().DOFade(5f, .7f);
        interactUI.GetComponentInChildren<Text>().DOFade(5f, .7f);
    }

    public void interactDissappear()
    {
        interactUI.GetComponent<Image>().DOFade(0f, .7f);
        interactUI.GetComponentInChildren<Text>().DOFade(0f, .7f);
    }

    public void isInteractable()
    {
        canInteract = true;
    }

    public void cantInteract()
    {
        canInteract = false;
    }

}
