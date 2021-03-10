using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBehaviour : MonoBehaviour
{
#region Variables
    public float moveSpeed;
    public GameObject target;
    private Animator anim;
    private Rigidbody2D rigidbody;
#endregion
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rigidbody.velocity.y) < 0.001f){
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
