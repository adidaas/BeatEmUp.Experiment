using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator myAnim;
    private HitCollider hitCollider;
    public AttackFramesManager attackFramesManager;
    private EnemyAudio enemyAudio;
    public GameObject hitBoxGameObject;
    public bool isAttacking = false;
    public bool trackAttackCooldown = false;
    public float attackCooldown = 1;

    // Start is called before the first frame update
    void Start() {
        myAnim = gameObject.GetComponent<Animator>();
        attackFramesManager = gameObject.GetComponent<AttackFramesManager>();
        hitCollider = hitBoxGameObject.GetComponent<HitCollider>();
        enemyAudio = gameObject.GetComponent<EnemyAudio>();
    }

    // Update is called once per frame
    void Update() {
        if (trackAttackCooldown) {
            attackCooldown -= 1 * Time.deltaTime;
            if (attackCooldown <= 0) {
                trackAttackCooldown = false;
                isAttacking = false;
            }
        }

        int layerMask = LayerMask.GetMask("Player");
        Vector2 leftRaycastStartingPosition = new Vector2(transform.position.x - 1.0f, transform.position.y - 0.6f );
        RaycastHit2D leftRaycastHit = Physics2D.Raycast(leftRaycastStartingPosition, Vector2.left, 3.5f, layerMask);

        Vector2 rightRaycastStartingPosition = new Vector2(transform.position.x + 1.0f, transform.position.y - 0.6f );			
        RaycastHit2D rightRaycastHit = Physics2D.Raycast(rightRaycastStartingPosition, Vector2.right, 3.5f, layerMask);

        
        // far roundhouse
        if (leftRaycastHit.collider != null || rightRaycastHit.collider != null) {
            float distance;

            if (leftRaycastHit.collider != null) {
                distance = Vector2.Distance(leftRaycastHit.transform.position, transform.position);
            }
            else {
                distance = Vector2.Distance(rightRaycastHit.transform.position, transform.position);
            }

            if (distance < 5f && distance > 3.95f) {
                //Debug.DrawRay(leftRaycastStartingPosition, leftRaycastHit.point, Color.white, 1.2f);            
                //Debug.DrawRay(rightRaycastStartingPosition, rightRaycastHit.point, Color.white, 1.2f);
                if (!isAttacking) {
                    BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse, 2);
                    enemyAudio.PlayAttackSound((int)GeneralEnums.EnemyCharacters.Terry, (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse);
                    attackFramesManager.SetAttackFrames((int)EnemyAttackEnums.TerryAttacks.FarRoundhouse, 2);
                    myAnim.SetTrigger("attack_FarRoundhouse");
                    isAttacking = true;
                    attackCooldown = 1;
                } 
            }
            else if (distance < 3.95f) {
                //Debug.DrawRay(leftRaycastStartingPosition, leftRaycastHit.point, Color.white, 1.2f);            
                //Debug.DrawRay(rightRaycastStartingPosition, rightRaycastHit.point, Color.white, 1.2f);
                if (!isAttacking) {
                    BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.Fierce, 2);
                    enemyAudio.PlayAttackSound((int)GeneralEnums.EnemyCharacters.Terry, (int)EnemyAttackEnums.TerryAttacks.Fierce);
                    attackFramesManager.SetAttackFrames((int)EnemyAttackEnums.TerryAttacks.Fierce, 2);
                    myAnim.SetTrigger("attack_Fierce");
                    isAttacking = true;
                    attackCooldown = 1;
                } 
            }
            
                       
        }

    }
}
