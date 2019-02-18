using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;
    public PlayerInfoManager playerInfoManager;
    private PlayerMovement playerMovement;
    public GameObject hitBoxGameObject;
	private HitCollider hitCollider;
    public Rigidbody2D myRigidbody;
    private Animator myAnim;
    private SpriteRenderer mySpriteRenderer;
    private AttackFramesManager attackFramesManager;
    private PlayerSpecialAttacks playerSpecialAttacks;
    private CharacterEffectController characterEffectController;
    private PlayerAudio playerAudio;

    private int currentComboRouteCount = 0;
    private int currentComboRouteType;
    private bool getComboCoolDown = true;
    private float comboRouteCooldown;
    private float buttonHeldTime = 0f;
    private bool trackButtonHeldTime = false;
    private bool exKeyActive;
    private float exTapCooldown = 0f;
    public float jumpSpeed;


    [Range(0f, 1f)]
    public float r = 1.0f;
    [Range(0f, 1f)]
    public float g = 1.0f;
    [Range(0f, 1f)]
    public float b = 1.0f;

    void Start() {
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = hitBoxGameObject.GetComponent<HitCollider>();
        attackFramesManager = GetComponent<AttackFramesManager>();
        playerSpecialAttacks = GetComponent<PlayerSpecialAttacks>();
        playerAudio = GetComponent<PlayerAudio>();
        characterEffectController = GetComponent<CharacterEffectController>();
    }

    void Update() {
        // ex key
        if (Input.GetButton("EXKey")) {
            exKeyActive = true;
            exTapCooldown = 0.3f;
            myAnim.SetBool("IsEXActive", true);
        }
        else {
            exKeyActive = false;
            myAnim.SetBool("IsEXActive", false);
        }

        if (exTapCooldown > 0f) {
            exTapCooldown -= 1 * Time.deltaTime ;
        }
        else {
            exTapCooldown = 0f;            
        }

        if (comboRouteCooldown > 0f) {
            comboRouteCooldown -= 1 * Time.deltaTime;
        }
        else if (getComboCoolDown) {
            currentComboRouteCount = 0;
            currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
        }

        GetPlayerAttackControl();
    }

    #region Attacks Control
    public void GetPlayerAttackControl() {
        
        if (trackButtonHeldTime) {
            buttonHeldTime += 1 * Time.deltaTime;
            // print(buttonHeldTime);
        }

        if (Input.GetButtonUp("AButton")) {
            if (currentComboRouteCount == 2 && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {                    
                BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Overhead);
                playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Overhead, false);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackOverhead);
                comboRouteCooldown = 1.1f;
                currentComboRouteCount++;
                getComboCoolDown = true;

                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Overhead, 1);

            }
            trackButtonHeldTime = false;                
            buttonHeldTime = 0;                
            //print("buttonhledtime: " + result);
        }

        // ground attacks
        if (playerController.canGroundAttack && !playerController.isAttacking && !playerController.isRunning && !playerController.isDashing) {   
            
            // normal attacks
            // ==========================================
            if (Input.GetButtonDown("AButton")) {
                // stop movement velocity when attacking
                if (!playerController.isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                playerController.isAttacking = true;
                playerController.canMove = false;
                playerController.canJump = false;
                myAnim.SetBool("IsAttacking", true);
                if (currentComboRouteCount == 0 && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.None) {      
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Jab);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Jab, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackJab);
                    comboRouteCooldown = 0.62f;
                    currentComboRouteCount++;
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AAA;
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Jab, 1);
                }                
                else if (currentComboRouteCount == 1 && 0f < comboRouteCooldown && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Short);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Short, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackShort);
                    comboRouteCooldown = 0.8f;
                    currentComboRouteCount++;
                    
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Short, 1);
                    trackButtonHeldTime = true;                                     
                }
                else if (currentComboRouteCount == 2 && 0 < comboRouteCooldown && comboRouteCooldown < 0.4f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Fierce);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Fierce, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackFierce);
                    comboRouteCooldown = 1.3f;
                    currentComboRouteCount++;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Fierce, 1);

                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AA_AA;
                }
                else if (currentComboRouteCount == 2 && comboRouteCooldown > 0.4f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Strong);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Strong, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackStrong);
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Strong, 1);
                    
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
                    comboRouteCooldown = 0.3f;
                    currentComboRouteCount = 0;
                }

                
                else if (currentComboRouteCount == 3 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AA_AA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackHighRoundhouse);
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
                    comboRouteCooldown = 0f;
                    currentComboRouteCount = 0;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.HighRoundhouse, 1);
                }  
                
                else if (currentComboRouteCount == 3 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.CrouchForward);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.CrouchForward, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackCrouchForward);
                    comboRouteCooldown = 0.8f;
                    currentComboRouteCount++;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.CrouchForward, 1);
                }  
                else if (currentComboRouteCount == 4 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.CrouchFierce);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.CrouchFierce, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackCrouchFierce);
                    comboRouteCooldown = 0.95f;
                    currentComboRouteCount++;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.CrouchFierce, 1);
                }  
                else if (currentComboRouteCount == 5 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.CloseForward);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.CloseForward, false);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackCloseForward);
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
                    comboRouteCooldown = 0;
                    currentComboRouteCount = 0;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.CloseForward, 1);
                }      
                
            }

            if (currentComboRouteCount == 2 && buttonHeldTime > 0.6f && currentComboRouteType != (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackOverheadIdle);                
                currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA;
                getComboCoolDown = false;
            }

            
            

            // special attacks
            // ==========================================
            if (!exKeyActive) {
                if ((Input.GetAxisRaw("Vertical") > 0f || playerController.inputUp) && Input.GetButtonDown("BButton")) {
                    // shoryuken
                    playerController.isAttacking = true;
                    playerController.canMove = false;
                    playerController.canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialShoryuken);
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Shoryuken);                    
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Shoryuken, 1);

                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Shoryuken, false);
                }
                else if (playerController.buttonTapCooldown > 0 && ( playerController.inputLeft || playerController.inputRight ) && Input.GetButtonDown("BButton")) {       
                    // tatsumaki             
                    playerController.isAttacking = true;
                    playerController.canMove = false;
                    playerController.canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialTatsumaki);
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Tatsumaki);                    
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Tatsumaki, 1);

                    characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust01, playerController.isFacingRight);
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Tatsumaki, playerController.isFacingRight);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Tatsumaki, false);
                }
                else if ( (playerController.inputDown) && Input.GetButtonDown("BButton")) {           
                    // hard knee         
                    playerController.isAttacking = true;
                    playerController.canMove = false;
                    playerController.canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHardKnee);
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.HardKnee);                    
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.HardKnee, 1);
                    // float direction = playerController.isFacingRight ? 15f : -15f;
                    // myRigidbody.velocity = new Vector2(direction, myRigidbody.velocity.y);

                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.HardKnee, false);
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.HardKnee, playerController.isFacingRight);
                }
                else if (playerController.buttonTapCooldown <= 0 && Input.GetButtonDown("BButton")) {       
                    // hadouken             
                    playerController.isAttacking = true;
                    playerController.canMove = false;
                    playerController.canJump = false;
                    myAnim.SetBool("IsAttacking", true);                
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHadouken);

                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Hadouken, playerController.isFacingRight);                
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Hadouken, false);
                }
            }
            // ex attacks
            // ==========================================
            else if (exKeyActive && playerInfoManager.currentSpecialLevel > 0) {
                if ((playerController.inputDown) && Input.GetButtonDown("BButton")) {           
                    // hard knee         
                    
                    playerController.isAttacking = true;
                    playerController.canMove = false;
                    playerController.canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHardKnee);
                    myAnim.SetBool("IsEXActive", true);
                    
                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.HardKnee, exKeyActive);                    
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.HardKnee, 1, true);

                    StartCoroutine(PlayEXFlash(0.041f));
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.HardKnee, playerController.isFacingRight, true);
                    playerInfoManager.ChangeSpecialLevel(-1);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.HardKnee, true);
                }
                else if (Input.GetButtonDown("BButton")) {     
                    
                    playerController.isAttacking = true;
                    playerController.canMove = false;
                    playerController.canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHadouken);
                    myAnim.SetBool("IsEXActive", true);

                    BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Hadouken, exKeyActive);                    
                    StartCoroutine(PlayEXFlash(0.032f));
                    
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Hadouken, playerController.isFacingRight, true);                
                    playerInfoManager.ChangeSpecialLevel(-1);
                    playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.Hadouken, true);
                }
            }
              
        }

        if (playerController.canAirAttack && !playerController.isAttacking) {                
            // jump attacks
            // ==========================================
            if (Input.GetButtonDown("AButton"))
            {                
                playerController.isAttacking = true;
                myAnim.SetBool("IsAttacking", true);
                BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.JumpShort);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.JumpShort);
                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.JumpShort, 1);

                playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.JumpShort, false);
                
            }
        }

        // dashing attacks
        if ((playerController.canDashAttack) && !playerController.isJumping && !playerController.isAttacking) {
            if (Input.GetButtonDown("AButton")) {
                playerController.isAttacking = true;
                playerController.canRun = false;
                playerController.canMove = false;
                playerController.isRunning = false;
                playerController.isDashing = false;
                playerController.canJump = false;
                playerController.canDash = false;
                playerController.canDashAttack = false;
                BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.RunningKick);
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust01, playerController.isFacingRight);
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialRunningKick);
                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.RunningKick, 1);

                playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.RunningKick, false);
                playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.RunningKick, playerController.isFacingRight);
            }

            if (Input.GetButtonDown("BButton")) {
                playerController.isAttacking = true;
                playerController.canRun = false;
                playerController.canMove = false;
                playerController.isRunning = false;
                playerController.isDashing = false;
                playerController.canJump = false;
                playerController.canDash = false;
                playerController.canDashAttack = false;
                BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.SolarPlexus);
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialSolarPlexus);
                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.SolarPlexus, 1);

                playerAudio.PlayAttackSound((int)GeneralEnums.PlayerCharacters.Ryu, (int)PlayerAttackEnums.RyuAttacks.SolarPlexus, false);
                playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.SolarPlexus, playerController.isFacingRight);                
            }
        }
    }
    #endregion

    public IEnumerator PlayEXFlash(float duration)
    {
        var originalColor = mySpriteRenderer.color;
        while (duration >= 0f) {
            duration -= 1 * Time.deltaTime;
            var flashColor = new Color(r, g, b, 1);
            mySpriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.05f);
            mySpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    #region StartAttackFrameTracking
    public void StartAttackFrameTracking() {
        //StartCoroutine(attackFramesManager.PlayAttackFramesRoutine());
        attackFramesManager.StartFrameTracking();

    }
    #endregion

    #region Hitstop
    public IEnumerator PlayHitStop(float hitStopDuration) {
        myAnim.enabled = false;
        yield return new WaitForSeconds(hitStopDuration);
        myAnim.enabled = true;
    }
    #endregion
}
