using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public GameObject shuriken;
    private GameObject target;
    private Animator anim;
    private bool collided = false;
    public int shurikenDamage = 40;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collided){
            transform.position = target.transform.position;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<EnemyScript>().currentHealth > 0) collision.gameObject.GetComponent<EnemyScript>().TakeDamage(shurikenDamage);
        target = collision.gameObject;
        anim.SetTrigger("collision");
        collided = true;
    }
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player"){
            Destroy(shuriken);
            collision.GetComponent<CharacterMovement>().CanUseShuriken(true);    
        }
    }
}
