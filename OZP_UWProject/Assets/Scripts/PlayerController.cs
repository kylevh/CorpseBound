using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Animator anim;
    public Animator ghostAnim;
    public Rigidbody2D rb;
    public SpriteRenderer mainSprite;
    public SwitchCharacter switcheroo;  //On player holder
    public ScreenShakeController shakira; //On player holder
    [SerializeField] public PPVFX vfx; //On Post Processing Object
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
    public bool attacking = false;
    public bool stairCaseAnimation = false;
    public bool obtainedSword = false;


    //Objects and sound
    public ParticleSystem runTrail;
    private UnityEngine.Object explosionRef;
    private UnityEngine.Object corpseBody;
    public GameObject reviveUI;
    



    public float timer;
    private float damageDelay = 0;
    [HideInInspector] public bool timing = false;
    [HideInInspector] public float clock;
    [HideInInspector] public bool isWhite = false;

    private void Awake()
    {
        instance = this;
        reviveUI.SetActive(false);
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

        damageDelay -= Time.deltaTime;
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
        setIdleDirection();
        if (!inGhostMode & obtainedSword)
        {
            attack();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    void DoTheMovementThing() //does the movement thing
    {
        if (!inDialogue() && !attacking)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Mathf.Abs(movement.y) >= 1.1f && Mathf.Abs(movement.x) >= 1.1f) //Clips movement diagonally
                {
                    movement.x = Input.GetAxisRaw("Horizontal") * 1.2f;
                    movement.y = Input.GetAxisRaw("Vertical") * 1.2f;
                }
                else
                {
                    movement.x = Input.GetAxisRaw("Horizontal") * 1.8f;
                    movement.y = Input.GetAxisRaw("Vertical") * 1.8f;
                }
                anim.SetFloat("Horizontal", movement.x);
                anim.SetFloat("Vertical", movement.y);
                anim.SetFloat("Magnitude", movement.magnitude);
                ghostAnim.SetFloat("Horizontal", movement.x);
                ghostAnim.SetFloat("Vertical", movement.y);
                ghostAnim.SetFloat("Magnitude", movement.magnitude);
                CreateDustTrail();
            }
            else
            {
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
                ghostAnim.SetFloat("Horizontal", movement.x);
                ghostAnim.SetFloat("Vertical", movement.y);
                ghostAnim.SetFloat("Magnitude", movement.magnitude);
            }

        }
        else if(inDialogue() || attacking)
        {
            movement.x = 0;
            movement.y = 0;
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Magnitude", movement.magnitude);
            ghostAnim.SetFloat("Horizontal", movement.x);
            ghostAnim.SetFloat("Vertical", movement.y);
            ghostAnim.SetFloat("Magnitude", movement.magnitude);
        }
        if (disableMovement && stairCaseAnimation && !attacking)
        {
            enterStaircaseAnim();
        }

    }


    void CreateDustTrail()
    {
        runTrail.Play();
    }

    public void goGhostMode(int check) //if check ==1, go to ghost mode, if 0, exit ghost mode
    {
        AudioManager.instance.changeMusic();
        if (check == 1) //enter ghost mode
        {
            if (inGhostMode == false)
            {
                reviveUI.SetActive(true);
                GameObject explosion = (GameObject)Instantiate(explosionRef);
                GameObject corpse = (GameObject)Instantiate(corpseBody);
                explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                corpse.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                inGhostMode = true;
                switcheroo.switchCharacter(2); //switches to ghost
                shakira.StartShake(10f, .3f);
                vfx.enteredGhostMode();
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;

                shadow.SetActive(false);
                deathPoint = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z); ;
            }
        }

        if (check == 0)
        {
            inGhostMode = false;
            reviveUI.SetActive(false);
            switcheroo.switchCharacter(1); //switches to main character
            shakira.StartShake(2f, .1f);
            vfx.exitedGhostMode();
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
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

    public void setIdleDirection()
    {
        if (!attacking)
        {
            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                anim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                anim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
                ghostAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                ghostAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }
    }

    public void attack()
    {
        if (Input.GetMouseButtonDown(0) && attacking == false)
        {
            if (damageDelay <= 0)
            {
                if (!inDialogue())
                {
                    anim.SetTrigger("attack");
                    StartCoroutine(attackCo());
                }
            }
            damageDelay = .3f;
        }
    }

    private IEnumerator attackCo()
    {
        attacking = true;
        yield return null;
        yield return new WaitForSeconds(.3f);
        attacking = false;
        
    }

    void enterStaircaseAnim()
    {
        //UITransistions.ui.fadeAnim(2, 5);
        movement.x = 0;
        movement.y = 0;
        anim.SetFloat("Vertical", movement.y );
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Magnitude", movement.magnitude);
        anim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
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
        AudioManager.instance.takeDamageSound();
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
            if (currentHealth < (ghostCooldown * .45))
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
