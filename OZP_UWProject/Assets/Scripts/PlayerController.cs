using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer mainSprite;
    public SwitchCharacter switcheroo;
    public ScreenShakeController shakira;

    public bool inGhostMode = false;

    public float movementSpeed = 5f;
    Vector2 movement;

    public ParticleSystem dust;
    private UnityEngine.Object explosionRef;

    public float timer;


    void Start()
    {
        #region Basic setters
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainSprite = GetComponentInChildren<SpriteRenderer>();
        switcheroo = GetComponent<SwitchCharacter>();
        shakira = GetComponent<ScreenShakeController>();
        explosionRef = Resources.Load("Explosion");

        #endregion


    }

    void Update()
    {
        DoTheMovementThing();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q was pressed");
            if (inGhostMode == false)
            {
                GameObject explosion = (GameObject)Instantiate(explosionRef);
                explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                inGhostMode = true;
                switcheroo.switchCharacter(2); //switches to ghost
                shakira.StartShake(4f, .3f);
            }
            else
            {
                inGhostMode = false;
                switcheroo.switchCharacter(1); //switches to main character
                shakira.StartShake(2f, .1f);
            }


        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    void DoTheMovementThing() //does the movement thing
    {
        //Input
        if (Mathf.Abs(movement.y) >= .6f && Mathf.Abs(movement.x) >= .6f) //Clips movement diagonally
        {
            movement.x = Input.GetAxisRaw("Horizontal") * .6f;
            movement.y = Input.GetAxisRaw("Vertical") * .6f;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Magnitude", movement.magnitude);

        CreateDustTrail();


    }


    void CreateDustTrail()
    {
        dust.Play();
    }
}
