using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator myAnim;
    private HitCollider hitCollider;
    public AttackFramesManager attackFramesManager;
    private EnemyAudio enemyAudio;
    private EnemyController enemyController;
    private EnemySpecialAttack enemySpecialAttack;
    public GameObject hitBoxGameObject;
    public bool isAttacking = false;
    public bool trackAttackCooldown = false;
    public float attackCooldown = 1;
    private int currentAttackType;
    private bool isFrameTracking = false;    
    private int currentFrameCount = 0;

    // Start is called before the first frame update
    void Start() {
        enemyController = gameObject.GetComponent<EnemyController>();
        myAnim = gameObject.GetComponent<Animator>();
        attackFramesManager = gameObject.GetComponent<AttackFramesManager>();
        hitCollider = hitBoxGameObject.GetComponent<HitCollider>();
        enemyAudio = gameObject.GetComponent<EnemyAudio>();
        enemySpecialAttack = gameObject.GetComponent<EnemySpecialAttack>();
    }

    // Update is called once per frame
    void Update() {
        if (enemyController.isDefeated) {
            enabled = false;
        }
        
        if (trackAttackCooldown) {
            attackCooldown -= 1 * Time.deltaTime;
            if (attackCooldown <= 0) {
                trackAttackCooldown = false;
                isAttacking = false;
                enemyController.canMove = true;
            }
        }

        if (enemyController.canAttack) {
            EnemyAttackControl();
        }

        if (isFrameTracking) {
            currentFrameCount++;
            if (currentAttackType == (int)EnemyAttackEnums.TerryAttacks.PowerDunk) {
                if (currentFrameCount == 22) {
                    enemyAudio.PlayAttackSound((int)GeneralEnums.EnemyCharacters.Terry, (int)EnemyAttackEnums.TerryAttacks.PowerDunk, 1);
                }
            }
            if (!isAttacking) {
                currentFrameCount = 0;
                isFrameTracking = false;
            }

        }

    }

    #region Enemy Attack Control
    void EnemyAttackControl() {
        float direction = enemyController.isFacingRight ? 1 : -1;
        int layerMask = LayerMask.GetMask("Player");
        Vector2 leftRaycastStartingPosition = new Vector2(transform.position.x - 1.0f, transform.position.y - 0.6f );
        RaycastHit2D leftRaycastHit = Physics2D.Raycast(leftRaycastStartingPosition, Vector2.left, 3.5f, layerMask);

        Vector2 rightRaycastStartingPosition = new Vector2(transform.position.x + 1.0f, transform.position.y - 0.6f );			
        RaycastHit2D rightRaycastHit = Physics2D.Raycast(rightRaycastStartingPosition, Vector2.right, 3.5f, layerMask);
       
        Vector2 antiAirRaycastStartingPosition = new Vector2(transform.position.x + (1 * direction), transform.position.y + 1.5f);
        Vector2 antiAirDirection = new Vector2(direction, 1);
        RaycastHit2D antiAirRaycastHit = Physics2D.Raycast(antiAirRaycastStartingPosition, antiAirDirection, 4.5f, layerMask);
        if(antiAirRaycastHit.collider != null) {
            Debug.DrawRay(antiAirRaycastStartingPosition, antiAirRaycastHit.point, Color.green, 2.2f);            
        }

        // terry normal attacks
        //if (5 == 3) {
        if (!enemyController.playerInfoManager.isInInvincibleState) {
            // far roundhouse
            if (leftRaycastHit.collider != null || rightRaycastHit.collider != null) {
                float distance;

                if (leftRaycastHit.collider != null) {
                    distance = Vector2.Distance(leftRaycastHit.transform.position, transform.position);
                }
                else {
                    distance = Vector2.Distance(rightRaycastHit.transform.position, transform.position);
                }

                if (antiAirRaycastHit.collider != null) {
                    if (!isAttacking) {
                        
                        enemyController.canMove = false;
                        currentAttackType = (int)EnemyAttackEnums.TerryAttacks.PowerDunk;
                        BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.PowerDunk, (int)GeneralEnums.EnemyCharacters.Terry);
                        enemyAudio.PlayAttackSound((int)GeneralEnums.EnemyCharacters.Terry, (int)EnemyAttackEnums.TerryAttacks.PowerDunk, 0);
                        attackFramesManager.SetAttackFrames((int)EnemyAttackEnums.TerryAttacks.PowerDunk, 2);
                        myAnim.SetTrigger(EnemyAttackEnums.AttackTriggerNames.SpecialPowerDunk);
                        isAttacking = true;
                        attackCooldown = 2;
                        trackAttackCooldown = true;
                        isFrameTracking = true;

                        enemySpecialAttack.InitializeSpecialAttack((int)EnemyAttackEnums.TerryAttacks.PowerDunk, enemyController.isFacingRight);
                    } 
                }
                //else if (distance < 5f && distance > 3.95f) {
                    else if (5 == 4) {
                    //Debug.DrawRay(leftRaycastStartingPosition, leftRaycastHit.point, Color.white, 1.2f);            
                    //Debug.DrawRay(rightRaycastStartingPosition, rightRaycastHit.point, Color.white, 1.2f);
                    if (!isAttacking) {
                        enemyController.canMove = false;
                        BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse, (int)GeneralEnums.EnemyCharacters.Terry);
                        enemyAudio.PlayAttackSound((int)GeneralEnums.EnemyCharacters.Terry, (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse);
                        attackFramesManager.SetAttackFrames((int)EnemyAttackEnums.TerryAttacks.FarRoundhouse, 2);
                        myAnim.SetTrigger(EnemyAttackEnums.AttackTriggerNames.AttackFarRoundhouse);
                        isAttacking = true;
                        attackCooldown = 1;
                    } 
                }
                //else if (distance < 3.95f) {
                    else if (5 == 4) {
                    //Debug.DrawRay(leftRaycastStartingPosition, leftRaycastHit.point, Color.white, 1.2f);            
                    //Debug.DrawRay(rightRaycastStartingPosition, rightRaycastHit.point, Color.white, 1.2f);
                    if (!isAttacking) {
                        enemyController.canMove = false;
                        BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.Fierce, (int)GeneralEnums.EnemyCharacters.Terry);
                        enemyAudio.PlayAttackSound((int)GeneralEnums.EnemyCharacters.Terry, (int)EnemyAttackEnums.TerryAttacks.Fierce);
                        attackFramesManager.SetAttackFrames((int)EnemyAttackEnums.TerryAttacks.Fierce, 2);
                        myAnim.SetTrigger(EnemyAttackEnums.AttackTriggerNames.AttackFierce);
                        isAttacking = true;
                        attackCooldown = 1;
                    } 
                }
            }
        }
    }
    #endregion

    #region 
    public void PlaySpecialAttack(int attackType) {
        if (enemyController.enemyCharacter == (int)GeneralEnums.EnemyCharacters.Terry) {
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.PowerDunk) {

            }
        }
    }
    #endregion

}
