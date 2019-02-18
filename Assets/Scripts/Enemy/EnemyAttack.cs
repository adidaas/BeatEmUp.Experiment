using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator myAnim;
    private HitCollider hitCollider;
    public AttackFramesManager attackFramesManager;
    public GameObject hitBoxGameObject;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start() {
        myAnim = gameObject.GetComponent<Animator>();
        attackFramesManager = gameObject.GetComponent<AttackFramesManager>();
        hitCollider = hitBoxGameObject.GetComponent<HitCollider>();
    }

    // Update is called once per frame
    void Update() {
        int layerMask = LayerMask.GetMask("Player");
        Vector2 leftRaycastStartingPosition = new Vector2(transform.position.x - 1.0f, transform.position.y - 0.6f );
        RaycastHit2D leftRaycastHit = Physics2D.Raycast(leftRaycastStartingPosition, Vector2.left, 3.5f, layerMask);

        Vector2 rightRaycastStartingPosition = new Vector2(transform.position.x + 1.0f, transform.position.y - 0.6f );			
        RaycastHit2D rightRaycastHit = Physics2D.Raycast(rightRaycastStartingPosition, Vector2.right, 3.5f, layerMask);

        if (leftRaycastHit.collider != null) {
            Debug.DrawRay(leftRaycastStartingPosition, leftRaycastHit.point, Color.white, 1.2f);            
            if (!isAttacking) {
                BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse, 2);
                attackFramesManager.SetAttackFrames((int)EnemyAttackEnums.TerryAttacks.FarRoundhouse, 2);
                myAnim.SetTrigger("attack_FarRoundhouse");
                isAttacking = true;
            }            
        }
        if (rightRaycastHit.collider != null) {
            Debug.DrawRay(rightRaycastStartingPosition, rightRaycastHit.point, Color.white, 1.2f);
        }
    }
}
