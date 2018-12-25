using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	#region Properties
	public Rigidbody2D myRigidbody;
	public Animator myAnim;
	public GameObject renderObject;

	public bool isGrounded;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask groundType;

	public GameObject hitBox;
	public GameObject hurtBox;
	#endregion 

	public static class HurtTriggers
	{
		public static string HurtHigh { get { return "hurt_High"; } }
		public static string HurtMid { get { return "hurt_Mid"; } }
		public static string HurtLaunch { get { return "hurt_Launch"; } }
		public static string HurtWakeUp { get { return "hurt_WakeUp"; } }
	}

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnim = renderObject.GetComponent<Animator>();
		//StartCoroutine (MoveOverSeconds (gameObject, new Vector3 (0.0f, 10f, 0f), 2f));
		//StartCoroutine (MoveOverSpeed (gameObject, new Vector3 (0.0f, 10f, 0f), 120f));

	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundType);

		myAnim.SetBool("isGrounded", isGrounded);
	}

	public void IsHit(int hurtType, float knockBackDistance) {
		//Debug.Log("***** " + gameObject.name + " - Is Hit, with type:" + hurtType);

		if (hurtType == 0) {
			myAnim.SetTrigger(HurtTriggers.HurtHigh);			
		}
		else if (hurtType == 1) {
			myAnim.SetTrigger(HurtTriggers.HurtMid);
		}
		else if (hurtType == 2) {
			// launched
			myAnim.SetTrigger(HurtTriggers.HurtLaunch);
		}

		if (hurtType != 2) {
			
			StartCoroutine(HurtKnockBack(knockBackDistance));
		}
	}

	public IEnumerator HurtKnockBack(float knockBackDistance)
	{
		
		var endPosition = transform.position;
		endPosition.x = endPosition.x + knockBackDistance;

		while (transform.position != endPosition) {			
			transform.position = 
					Vector3.MoveTowards(transform.position, endPosition, 10f * Time.deltaTime);
						
			yield return new WaitForEndOfFrame ();
		}

	}

	public float GetRecoveryTime(int hurtType) {
		if (hurtType == 2) 
		{
			return 0.4f;
		}	
		return 0f;
	}

	#region Animator_SetNewPosition
	void SetNewPosition()
	{
		Debug.Log ("over here");
		Debug.Log (myAnim.transform.position);
		Debug.Log (gameObject.transform.position);

		//gameObject.transform.position = myAnim.transform.position;
	}
	#endregion

	#region Animator_ToggleHurtHigh
	void ToggleHurtHigh()
	{
	}
	#endregion

	#region Animator_ToggleHurtHigh
	// public void ToggleHurtWakeUp()
	// {
	// 	myAnim.SetTrigger(HurtTriggers.HurtWakeUp);
	// }

	public IEnumerator ToggleHurtWakeUp (int attackHurtType)
	{
		//Debug.Log("Toggle wakeup ---------------------------");
		
		var seconds = GetRecoveryTime(attackHurtType);

		float elapsedTime = 0;
		while (elapsedTime < seconds)
		{
			//Debug.Log(elapsedTime + " - " + seconds);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		myAnim.SetTrigger(HurtTriggers.HurtWakeUp);
	}
	#endregion
}
