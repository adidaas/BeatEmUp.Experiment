using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFramesManager : MonoBehaviour
{
    public GameObject hitBoxGameObject;
    public BoxCollider2D hitBoxBoxCollider;
    public IEnumerator playAttackFramesRoutine;
    public PlayerController playerController;
    private EnemyController enemyController;
    private EnemyAttack enemyAttack;
    private int startUpFrame = 0;
    private int activeFrame = 0;
    private int recoveryFrame = 0;
    private int endFrame = 0;
    private int cancelWindowFrames = 0;

    private bool isTrackingFrame = false;
    private int currentFrameCount = 0;
    private bool isInStartUp = false;
    private Phases currentPhase;
    private bool isInRecovery = false;
    private bool isInCancelWindow = false;
    private bool isPlayer = true;
    
    private void Start() {
        hitBoxBoxCollider = hitBoxGameObject.GetComponent<BoxCollider2D>();
        if (gameObject.tag == GeneralEnums.GameObjectTags.Player) {
            playerController = gameObject.GetComponent<PlayerController>();
        }        
        if (gameObject.tag == GeneralEnums.GameObjectTags.Enemy) {
            enemyController = gameObject.GetComponent<EnemyController>();
            enemyAttack = gameObject.GetComponent<EnemyAttack>();
            isPlayer = false;
        }
    }

    public enum Phases { 
		StartUp = 1, Active = 2, Recovery = 3, Cancel = 4, End = 5
	}

    void Update() {
        if (isTrackingFrame) {
            currentFrameCount++;
            //print(currentFrameCount);
            //print(currentPhase);
            if (isPlayer) {
                if (currentFrameCount >= cancelWindowFrames) { 
                    isInCancelWindow = true; 
                    playerController.isAttacking = false;
                }
                else if (currentFrameCount < cancelWindowFrames) { 
                    playerController.isAttacking = true;
                }
            }     
            //print(currentFrameCount);

            if (currentFrameCount == activeFrame ) { 
                currentPhase = Phases.Active; 
                //print("ACTIVE==================");
                SetHitboxActive(); 
            }
            else if (currentFrameCount == recoveryFrame ) { 
                currentPhase = Phases.Recovery; 
                SetHitboxActive(); 
                if (isPlayer) {
                    playerController.canMove = true;
                }
                else {
                    //enemyController.canMove = true;
                }
                
            }
            else if (currentFrameCount == (endFrame) ) { 
                currentPhase = Phases.End; 
                isTrackingFrame = false;

                if (!isPlayer) {
                    enemyAttack.trackAttackCooldown = true;
                }
            }

        }
    }

    public void SetAttackFrames(int attackType, int character, bool isExAttack = false) {
        //print(attackType);
        isTrackingFrame = false;
        startUpFrame = 0;
        activeFrame = 0;
        recoveryFrame = 0;
        

        if (character == 1) {
            //startUpFrame = PlayerAttackEnums.RyuAttacksStartUpFrames(attackType, isExAttack);
            activeFrame = PlayerAttackEnums.RyuAttacksActiveFrames(attackType, isExAttack);
            recoveryFrame = PlayerAttackEnums.RyuAttacksRecoveryFrames(attackType, isExAttack);
            cancelWindowFrames = PlayerAttackEnums.RyuAttacksCancelWindow(attackType, isExAttack);
            endFrame = PlayerAttackEnums.RyuAttacksEndFrames(attackType, isExAttack);
        }
        else if (character == 2) {            
            activeFrame = EnemyAttackEnums.TerryAttacksActiveFrames(attackType);
            recoveryFrame = EnemyAttackEnums.TerryAttacksRecoveryFrames(attackType);
            endFrame = EnemyAttackEnums.TerryAttacksEndFrames(attackType);

        }
    }

    public void StartFrameTracking() {
        currentFrameCount = 0;
        currentPhase = Phases.StartUp; 
        isTrackingFrame = true;
    }

    public void SetHitboxActive() {
        if (currentPhase == Phases.StartUp) {
            //hitBoxGameObject.SetActive(false);
            //hitBoxBoxCollider.enabled = false;
        }
        else if (currentPhase == Phases.Active) {
            //print("actrive");
            //hitBoxGameObject.SetActive(true);
            hitBoxBoxCollider.enabled = true;
        }
        else if (currentPhase == Phases.Recovery) {
            //print("recovery");
            //hitBoxGameObject.SetActive(false);
            hitBoxBoxCollider.enabled = false;
        }
    }

    #region StartAttackFrameTracking
    public void StartAttackFrameTracking() {
        StartFrameTracking();
    }
    #endregion
}
