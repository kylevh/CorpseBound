using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FratBoy : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private static Tilemap tilemap;
    private AIPath aiPath;
    private Rigidbody2D rb2d;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        if (!tilemap)
        {
            tilemap = GameObject.FindObjectOfType<Tilemap>();
            
        }

        transform.position = tilemap.AlignToGrid(transform.position) + new Vector3(0,0,-0.01f);
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat(Speed,aiPath.desiredVelocity.sqrMagnitude);
        anim.SetFloat(X,aiPath.desiredVelocity.x);
        anim.SetFloat(Y,aiPath.desiredVelocity.y);
    }
}
