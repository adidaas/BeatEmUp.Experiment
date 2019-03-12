using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController enemyController;
    private EnemySense enemySense;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    
    public bool canMove = true;
    private float walkSpeed = 6f;    

    // Start is called before the first frame update
    void Start() {
        enemyController = gameObject.GetComponent<EnemyController>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemySense = gameObject.GetComponentInChildren<EnemySense>();
        myAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (enemyController.isDefeated) {
            enabled = false;
        }
        if (enemyController.canMove) {
            GetEnemyMovement();
        }
    }

    void GetEnemyMovement() {
        if (enemySense.isPlayerInRange && enemyController.canMove) {
            float distanceToTarget = Vector2.Distance(enemySense.targetPosition, transform.position);
            //print(distanceToTarget);

            if (distanceToTarget > 5.7f) {
                float direction;// = enemyController.isFacingRight ? 1f : -1f;
                float facingDirection;

                if (!enemySense.isTargetInFront) {    
                    if (enemyController.isFacingRight) {                    
                        facingDirection = 1;
                        enemyController.isFacingRight = false;
                    }
                    else {
                        facingDirection = -1;
                        enemyController.isFacingRight = true;
                    }
                }
                else {
                    facingDirection = enemyController.isFacingRight ? -1f : 1f;
                }

                direction = enemyController.isFacingRight ? 1f : -1f;

                myAnimator.SetBool("isWalking", true);
                //print("moving enemy");
                // negative moving left, positive moving right
                
                
                myRigidbody.velocity = new Vector2(direction * walkSpeed, myRigidbody.velocity.y);
                transform.localScale = new Vector2(facingDirection * 1f, 1f);
            }
            else {
                myAnimator.SetBool("isWalking", false);
            }
        }
        else 
        {
            myAnimator.SetBool("isWalking", false);
        }
    }
}
