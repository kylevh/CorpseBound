using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FratBoy : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private List<GameObject> Waypoints;
    private static Tilemap tilemap;
    private AIPath aiPath;
    private Rigidbody2D rb2d;
    private AIDestinationSetter _aiDestinationSetter;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    [SerializeField] private int index;
    [SerializeField] public float moveSpeed = 0.05f;

    private bool attacking = false;

    // Start is called before the first frame update
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
        transform.position =
            tilemap.AlignToGrid(transform.position) + new Vector3(0, 0, -0.01f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _aiDestinationSetter.target = other.gameObject.transform;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _aiDestinationSetter.target = Waypoints[index].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (((transform.position - Waypoints[index].transform.position).sqrMagnitude <
             1.0f))
        {
            index = (index + 1) % Waypoints.Count;
            _aiDestinationSetter.target = Waypoints[index].transform;
        }


        anim.SetFloat(Speed,
            aiPath.desiredVelocity.sqrMagnitude);
        //Debug.Log(rb2d.velocity.sqrMagnitude);
        anim.SetFloat(X, aiPath.desiredVelocity.x);
        anim.SetFloat(Y, aiPath.desiredVelocity.y);
    }

    public float Timer { get; set; }
}