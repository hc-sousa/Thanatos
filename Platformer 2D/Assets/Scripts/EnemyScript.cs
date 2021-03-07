using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
#region Variables
    public int maxHealth = 100;
    int currentHealth;
    public float attackDistance;
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    public Vector3 characterScale;
    public Transform Player;
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
        characterScale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update(){
        if (currentHealth > 0){
            if (Player.position.x > transform.position.x)
                characterScale.x = Mathf.Abs(characterScale.x);
            else
                characterScale.x = -1 * Mathf.Abs(characterScale.x);
            transform.localScale = characterScale;
            if (inRange){
                EnemyLogic();
            }
            else{
                anim.SetBool("canWalk", false);
                StopAttack();
            }
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
    void OnTriggerExit2D(Collider2D trig){
        if (trig.gameObject.tag == "Player"){
            target = trig.gameObject;
            inRange = false;
        }
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        //Play animation

        if (currentHealth <= 0)
            Die();
    }

    void Die(){
        anim.SetTrigger("die");
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
