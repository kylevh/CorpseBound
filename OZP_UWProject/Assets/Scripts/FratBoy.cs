using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FratBoy : MonoBehaviour
{
    #region Mumbo jumbo
    private Animator anim;
    [SerializeField] private List<GameObject> Waypoints;
    [SerializeField] public Tilemap tilemap;
    private AIPath aiPath;
    private Rigidbody2D rb2d;
    private AIDestinationSetter _aiDestinationSetter;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    #endregion

    [SerializeField] private int index;
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] public float searchRadius = 1f;

    public EnemyHealthBar healthBar;
    public float maxHealth;
    public float currentHealth;
    public float Timer { get; set; }
    private float damageDelay = 0;


    void Start()
    {
        anim = GetComponent<Animator>();
        //rb2d = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        if (!tilemap)
        {
            tilemap = GameObject.FindObjectOfType<Tilemap>();
        }
        _aiDestinationSetter.target = Waypoints[index].transform;
        aiPath.maxSpeed = moveSpeed;
        transform.position = tilemap.AlignToGrid(transform.position) + new Vector3(0, 0, -0.01f);
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        _aiDestinationSetter.target = other.gameObject.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _aiDestinationSetter.target = Waypoints[index].transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (damageDelay <= 0)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().takeDamage((int)UnityEngine.Random.Range(10, 30));
            }
            damageDelay = .5f;
        }
    }

    void Update()
    {
        damageDelay -= Time.deltaTime;

        if (((transform.position - Waypoints[index].transform.position).sqrMagnitude <
             searchRadius))
        {
            index = (index + 1) % Waypoints.Count;
            _aiDestinationSetter.target = Waypoints[index].transform;
        }


        anim.SetFloat(Speed,
            aiPath.desiredVelocity.sqrMagnitude);
        //Debug.Log(rb2d.velocity.sqrMagnitude);
        anim.SetFloat(X, aiPath.desiredVelocity.x);
        anim.SetFloat(Y, aiPath.desiredVelocity.y );
    }

}