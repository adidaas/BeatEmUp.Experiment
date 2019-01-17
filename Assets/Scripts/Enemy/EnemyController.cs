using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	#region Properties
	public Rigidbody2D myRigidbody;
	public Animator myAnim;
	public GameObject renderObject;
	private EnemyHurt enemyHurt;
	public IEnumerator hitStunCoroutine;
	private CharacterEffectController characterEffectController;
	public PlayerInfoManager playerInfoManager;

	private float currentGravityScale = 3f;
	public bool isGrounded;
	public bool isAirJuggleable = false;
	public bool isInInvincibleState = false;
	public bool isCornered = false;
	public bool isBeingKnockedBack = false;	
	public bool isFacingRight = false;
	public bool isInLaunchBack = false;
	public bool isInWallBounce = false;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask groundType;
	public LayerMask wallType;

	public GameObject hitBox;
	public GameObject hurtBox;
	private GameObject ground;
	public GameObject groundCheckObject;
	public BoxCollider2D groundCheckCollider;
	public GameObject airJuggleBoxObject;
	public BoxCollider2D airJuggleBoxCollider;
	#endregion 

	

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myRigidbody.gravityScale = currentGravityScale;
		myAnim = gameObject.GetComponent<Animator>();
		ground = GameObject.FindWithTag("Ground");
		enemyHurt = gameObject.GetComponent<EnemyHurt>();
		groundCheckCollider = groundCheckObject.GetComponent<BoxCollider2D>();
		airJuggleBoxCollider = airJuggleBoxObject.GetComponent<BoxCollider2D>();
		characterEffectController = gameObject.GetComponent<CharacterEffectController>();
		//StartCoroutine (MoveOverSeconds (gameObject, new Vector3 (0.0f, 10f, 0f), 2f));
		//StartCoroutine (MoveOverSpeed (gameObject, new Vector3 (0.0f, 10f, 0f), 120f));

	}
	
	// Update is called once per frame
	void Update () {
		if (isAirJuggleable) {
			float distanceToGround = groundCheck.position.y - ground.transform.position.y;

			if (distanceToGround <= 1.65f) {
				isInInvincibleState = true;
				//groundCheckCollider.enabled = true;
				myRigidbody.gravityScale = 3f;
				currentGravityScale = 3f;
			}
		}
		isCornered = Physics2D.OverlapCircle(gameObject.transform.position, 2f, wallType);
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundType);		

		// detect if object has touched the ground after being air juggled
		if (isGrounded && isAirJuggleable) {
			isAirJuggleable = false;			
			ToggleGroundCheckActive(true);
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtGround);			
			StartCoroutine(ToggleHurtWakeUp(enemyHurt.hurtType));
		}

		// detect if enemy is being launched back and bounce off wall
		if (!isInWallBounce && isCornered && isInLaunchBack) {
			isInLaunchBack = false;
			isInInvincibleState = true;
			isInWallBounce = true;
			isAirJuggleable = true;
			myRigidbody.velocity = new Vector2(0, 0);
			myRigidbody.bodyType = RigidbodyType2D.Kinematic;
			groundCheckObject.SetActive(false);
			
			myAnim.ResetTrigger(GeneralEnums.HurtTriggers.HurtLaunchBack);
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtLaunch);
			enemyHurt.hurtType = (int)GeneralEnums.AttacksHurtType.WallBounce;
			enemyHurt.currentStep = 0;
			enemyHurt.isAnimationDone = false;
			characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.WallSplash, isFacingRight);
			enemyHurt.HurtWallBounce_MovePosition();
		}
		
		myAnim.SetBool("isGrounded", isGrounded);
	}

	public void IsHit(int hurtType, float knockBackDistance, bool playerFacingRight, float hitStop) {
		//knockBackDistance = 0.0f;
		playerInfoManager.AdjustSpecialMeter(10);
		float verticalKnockBack = myRigidbody.velocity.y;
		float direction = 1.0f;
		float hitshakeDuration = 0.2f;
		isInInvincibleState = true;
		StartCoroutine(InvincibleStateFlicker());

		enemyHurt.hurtType = hurtType;
		if (playerFacingRight) {
			isFacingRight = false;
			transform.localScale = new Vector3(1, 1, 1);			
		}
		else {
			direction = -1.0f;
			isFacingRight = true;
			transform.localScale = new Vector3(-1, 1, 1);
		}
		
		// hitstun
		if (hitStunCoroutine != null) {
			StopCoroutine(hitStunCoroutine);
		}		

		if (!isAirJuggleable) {
			if (hurtType == (int)GeneralEnums.AttacksHurtType.LaunchBack) {
				myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtLaunchBack);
				hitStunCoroutine = enemyHurt.PerformHitStun(0.02f, GeneralEnums.AttacksHurtType.LaunchBack);
				groundCheckObject.SetActive(false);
				StartCoroutine(WaitToReactiveGroundCheck(0.02f));
				isInLaunchBack = true;
				hitshakeDuration = 0.05f;
			}
			else if (hurtType == (int)GeneralEnums.AttacksHurtType.High) {
				myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtHigh);			
				hitStunCoroutine = enemyHurt.PerformHitStun(0.6f + hitStop, GeneralEnums.AttacksHurtType.High);				
			}
			else if (hurtType == (int)GeneralEnums.AttacksHurtType.Mid) {
				myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMid);
				hitStunCoroutine = enemyHurt.PerformHitStun(0.6f + hitStop, GeneralEnums.AttacksHurtType.Mid);
			}
			else if (hurtType == (int)GeneralEnums.AttacksHurtType.Launch) {
				myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMid);
				hitStunCoroutine = enemyHurt.PerformHitStun(hitStop, GeneralEnums.AttacksHurtType.Launch);
			}
			StartCoroutine(hitStunCoroutine);		
			StartCoroutine(enemyHurt.PerformHitShake(hitshakeDuration));		
		}	

		if (!isBeingKnockedBack) {
			// if grounded, do all normal hit types. if not, then the only hit response is air juggles
			if (!isAirJuggleable) {
				verticalKnockBack = 0f;
			}
			else {
				myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtAir);
				//groundCheckRadius = 0.3f;
				knockBackDistance = 12.5f;
				currentGravityScale += 1.3f;
				myRigidbody.gravityScale = currentGravityScale;
				verticalKnockBack = 15f;
			}

			if (isCornered) {
				knockBackDistance = myRigidbody.velocity.x;
			}
			isBeingKnockedBack = true;
			myRigidbody.velocity = new Vector2(knockBackDistance * direction, verticalKnockBack);			
			StartCoroutine(KnockBackWait());
		}
	}
	
	public IEnumerator WaitToReactiveGroundCheck(float duration) {
		yield return new WaitForSeconds(duration);
		groundCheckObject.SetActive(true);
	}

	public void ToggleGroundCheckActive(bool isActive) {
		groundCheckObject.SetActive(isActive);
		airJuggleBoxObject.SetActive(!isActive);
	}

	#region Animator_ToggleGroundCheck
	public void Animator_ToggleGroundCheck() {
		groundCheckObject.SetActive(true);
		airJuggleBoxObject.SetActive(false);
	}
	#endregion

	public IEnumerator InvincibleStateFlicker() {
		yield return new WaitForSeconds(0.05f);
		isInInvincibleState = false;
	}

	public IEnumerator KnockBackWait() {		
		yield return new WaitForSeconds(0.2f);
		isBeingKnockedBack = false;
	}

	public float GetRecoveryTime(int hurtType) {
		if (hurtType == 0) {
			return 0.24f;
		}	
		if (hurtType == 1) {
			return 0.24f;
		}
		if (hurtType == 2) {
			return 0.4f;
		}	
		if (hurtType == 3) {
			return 0.24f;
		}
		if (hurtType == 4) {
			return 0.45f;
		}
		if (hurtType == 5) {
			return 0.24f;
		}	
		return 3f;
	}

	#region Animator_SetNewPosition
	void SetNewPosition() {
		Debug.Log (myAnim.transform.position);
		Debug.Log (gameObject.transform.position);

		//gameObject.transform.position = myAnim.transform.position;
	}
	#endregion

	#region Animator_ToggleHurtWakeUp
	public IEnumerator ToggleHurtWakeUp (int attackHurtType) {
		var seconds = GetRecoveryTime(attackHurtType);
		float elapsedTime = 0;
		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}		
		myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtWakeUp);

		
	}
	#endregion

	#region Animator_IsAirJuggable
	public void Animator_IsAirJuggable () {
		isAirJuggleable = true;
	}	
	#endregion

	#region Animator_IsInInvincibleState
	public void Animator_IsInInvincibleState () {
		isInInvincibleState = false;
	}
	#endregion

	#region Animator_HurtFalling
	public void Animator_HurtFalling () {
		myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtFalling);
	}
	#endregion
}
