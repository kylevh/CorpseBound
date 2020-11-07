using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer mainSprite;
    public SwitchCharacter switcheroo;  //On player holder
    public ScreenShakeController shakira; //On player holder
    [SerializeField]
    public PPVFX vfx; //On Post Processing Object

    public bool inGhostMode = false;

    public float movementSpeed = 5f;
    Vector2 movement;
    public float ghostCooldown = 7f;

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


        #endregion
        explosionRef = Resources.Load("Explosion");

    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Q)) //If death button pressed, do all this shit
        {
            if (inGhostMode == false)
            {
                goGhostMode(1);
            }
            else
            {
                goGhostMode(0);
            }
        }

        DoTheMovementThing();
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

    public void goGhostMode(int check) //if check ==1, go to ghost mode, if 0, exit ghost mode
    {
        if (check == 1) //enter ghost mode
        {
            if (inGhostMode == false)
            {
                GameObject explosion = (GameObject)Instantiate(explosionRef);
                explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                inGhostMode = true;
                switcheroo.switchCharacter(2); //switches to ghost
                shakira.StartShake(10f, .3f);
                vfx.enteredGhostMode();
            }
        }

        if (check == 0)
        {
            inGhostMode = false;
            switcheroo.switchCharacter(1); //switches to main character
            shakira.StartShake(2f, .1f);
            vfx.exitedGhostMode();
        }
    }
}
