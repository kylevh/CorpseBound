using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHit : MonoBehaviour
{
    public float damageDelay = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageDelay -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fratboy"))
        {
            FratBoy frat = other.GetComponent<FratBoy>();
            if (damageDelay <= 0)
            {
                frat.takeDamage(15);
                damageDelay = .3f;
            }
        }
    }
}
