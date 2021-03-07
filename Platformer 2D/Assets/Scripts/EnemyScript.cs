using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
#region Variables
    public int maxHealth = 100;
    int currentHealth;
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;
    private bool inRange; // Check if player is in range
    private bool cooling; // Check if enemy is cooling after attack
    private float intTimer;
#endregion

    void Awake(){
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update(){
        if (inRange){
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        if (hit.collider != null) Debug.Log("hit not null");
        //When Player is detected
        if(hit.collider != null)
            EnemyLogic();
        else if (hit.collider == null)
            inRange = false;
        if (!inRange){
            anim.SetBool("canWalk", false);
            StopAttack();
        }
    }

    void EnemyLogic(){
        distance = Vector2.Distance(transform.position, target.transform.position);
        
        if (distance > attackDistance){
            Move();
            StopAttack();
        }
        else if (attackDistance >= distance && !cooling){
            Attack();
        }
        if (cooling){
            Cooldown();
            anim.SetBool("attack", false);
        }
    }

    void Move(){
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack")){
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack(){
        timer = intTimer; // Reset Timer when Player enter Attack Range
        attackMode = true; // To check if Enemy can still attack or not
        anim.SetBool("attack", true);
        anim.SetBool("canWalk", false);
    }

    void StopAttack(){
        cooling = false;
        attackMode = false;
        anim.SetBool("attack", false);
    }

    void OnTriggerEnter2D(Collider2D trig){
        if (trig.gameObject.tag == "Player"){
            target = trig.gameObject;
            inRange = true;
        }
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        //Play animation

        if (currentHealth <= 0)
            Die();
    }

    void Die(){
        Debug.Log("Enemy Died");
    }

    void RaycastDebugger(){
        if (distance > attackDistance){
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if(attackDistance > distance){
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }
    void Cooldown(){
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode){
            cooling = false;
            timer = intTimer;
        }
    }
    public void TriggerCooling(){
        cooling = true;
    }
}
