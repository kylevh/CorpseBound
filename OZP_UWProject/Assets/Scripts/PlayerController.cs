using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer mainSprite;
    public SwitchCharacter switcheroo;  //On player holder
    public ScreenShakeController shakira; //On player holder
    [SerializeField] public PPVFX vfx; //On Post Processing Object
    public AudioManager audio;
    Vector2 movement;
    private NPC_Controller npc;
    GameObject GameManager;

    [Header("Player Settings")]
    public float movementSpeed = 5f;
    public float maxHealth = 100;
    public float currentHealth;
    public HealthBar healthMeter;
    GameObject shadow;
    Vector3 deathPoint;

    //Player stats
    [HideInInspector]
    public bool inGhostMode = false;
    public float ghostCooldown = 4f;
    public bool disableMovement = false;

    //Objects and sound
    public ParticleSystem dust;
    private UnityEngine.Object explosionRef;
    private UnityEngine.Object corpseBody;
    



    public float timer;
    [HideInInspector] public bool timing = false;
    [HideInInspector] public float clock;
    [HideInInspector] public bool isWhite = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        #region Basic setters
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainSprite = GetComponentInChildren<SpriteRenderer>();
        switcheroo = GetComponent<SwitchCharacter>();
        shakira = GetComponent<ScreenShakeController>();
        healthMeter.SetMaxHealth(maxHealth);
        healthMeter.SetColor(1);


        #endregion
        explosionRef = Resources.Load("Explosion");
        corpseBody = Resources.Load("corpse");
        shadow = GameObject.Find("shadow");

    }

    void Update()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            healthMeter.SetHealth(timer);
        }

        if (Input.GetKeyDown(KeyCode.Q)) //If death button pressed, do all this shit
        {
            if (inGhostMode == false)
            {
                goGhostMode(1);
                countDownGhost();
            }
            else
            {
                goGhostMode(0);
            }     
        }

        if (healthMeter.getHealth() == 0 && inGhostMode == true)
        {
            goGhostMode(0);
        }
        else if (healthMeter.getHealth() == 0 && inGhostMode == false)
        {
            goGhostMode(1);
            countDownGhost();
        }

        DoTheMovementThing();
        healthBarUpdate();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    void DoTheMovementThing() //does the movement thing
    {
        if (!inDialogue())
        {
            if(disableMovement == false)
               

            //Input
            if (Mathf.Abs(movement.y) >= .6f && Mathf.Abs(movement.x) >= .6f) //Clips movement diagonally
            {
                movement.x = Input.GetAxisRaw("Horizontal") * .75f;
                movement.y = Input.GetAxisRaw("Vertical") * .75f;
            }
            else
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
            }
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Magnitude", movement.magnitude);

            //CreateDustTrail();
        }
        else if (inDialogue())
        {
            movement.x = 0;
            movement.y = 0;
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Magnitude", movement.magnitude);
        }

    }


    void CreateDustTrail()
    {
        dust.Play();
    }

    public void goGhostMode(int check) //if check ==1, go to ghost mode, if 0, exit ghost mode
    {
        audio.changeMusic();
        if (check == 1) //enter ghost mode
        {
            if (inGhostMode == false)
            {
                GameObject explosion = (GameObject)Instantiate(explosionRef);
                GameObject corpse = (GameObject)Instantiate(corpseBody);
                explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                corpse.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                inGhostMode = true;
                switcheroo.switchCharacter(2); //switches to ghost
                shakira.StartShake(10f, .3f);
                vfx.enteredGhostMode();
                GetComponent<BoxCollider2D>().enabled = false;
                
                shadow.SetActive(false);
                deathPoint = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z); ;
            }
        }

        if (check == 0)
        {
            inGhostMode = false;
            switcheroo.switchCharacter(1); //switches to main character
            shakira.StartShake(2f, .1f);
            vfx.exitedGhostMode();
            GetComponent<BoxCollider2D>().enabled = true;
            shadow.SetActive(true);
            healthMeter.SetMaxHealth(100);
            healthMeter.SetHealth(100);
            healthMeter.SetColor(1);
            timer = 0;
            gameObject.transform.position = deathPoint;
            Destroy(GameObject.FindGameObjectWithTag("dead"));
            
        }
    }

    private bool inDialogue()
    {
        if (npc != null)
            return npc.DialogueActive();
        else
            return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            npc = collision.gameObject.GetComponent<NPC_Controller>();

            if (Input.GetKey(KeyCode.F))
                npc.ActivateDialogue();
        }

        /*if(collision.gameObject.tag == "Fratboy")
        {
            StartCoroutine(Knockback(2, 5, collision.transform));
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        npc = null;
    }

    public void takeDamage(int DAMAGE)
    {
        currentHealth -= DAMAGE;
        healthMeter.SetHealth(currentHealth);
        shakira.StartShake(10, .1f);
        audio.takeDamageSound();
    }

    public void countDownGhost()
    {
        healthMeter.SetMaxHealth(ghostCooldown);
        healthMeter.SetColor(2);
        timer = ghostCooldown;
    }

    public IEnumerator Knockback(float knockbackDuration, float power, Transform obj)
    {
        float kbTimer = 0;

        while(knockbackDuration > kbTimer)
        {
            kbTimer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * power);
        }

        yield return 0;
    }

    public IEnumerator countDown(float waitSeconds)
    {

        yield return new WaitForSeconds(waitSeconds);
    }

    public void healthBarUpdate()
    {

        clock += Time.deltaTime;
        currentHealth = healthMeter.getHealth();
        if (inGhostMode == false)
        {
            if (currentHealth < 30)
            {
                if (clock > .1f)
                {
                    if (isWhite == true)
                    {
                        isWhite = false;
                        healthMeter.SetColor(1);
                        clock = 0;

                    }
                    else if (isWhite == false)
                    {
                        isWhite = true;
                        healthMeter.SetColor(3);
                        clock = 0;
                    }
                }

            }
        }

        else if (inGhostMode == true)
        {
            if (currentHealth < 2)
            {
                if (clock > .07f)
                {
                    if (isWhite == true)
                    {
                        isWhite = false;
                        healthMeter.SetColor(2);
                        clock = 0;

                    }
                    else if (isWhite == false)
                    {
                        isWhite = true;
                        healthMeter.SetColor(3);
                        clock = 0;
                    }
                }

            }
        }
    }

}
