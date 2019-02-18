using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySense : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    public bool isPlayerInRange = false;
    private GameObject parentObject;
    private GameObject playerObject;

    public SpriteRenderer spr;
    public Transform lineOfSightEnd;

    public Vector2 targetPosition;
    public bool isTargetInFront;

    void Start() {
        // lineOfSightEnd.transform.position = new Vector3(-17, -7, 0);
         enemyMovement = transform.parent.gameObject.GetComponent<EnemyMovement>();
    }

    void FixedUpdate() {
        if (CanPlayerBeSeen()) {
            spr.color = Color.red;
        }            
        else {
            spr.color = Color.white;
        }            
    }


    bool CanPlayerBeSeen() {
        // we only need to check visibility if the player is within the enemy's visual range
        if (isPlayerInRange) {
            if (PlayerInFieldOfView())
                return (!PlayerHiddenByObstacles());
            else
                return false;

        }
        else {
            // always false if the player is not within the enemy's range
            return false;
        }

        //return playerInRange;

    }
    void OnTriggerStay2D(Collider2D other) {
        // if 'other' is player, the player is seen a
        // note, we don't really need to check the transform tag since the collision matrix is set to only 'see' collisions with the player layer
        if (other.tag != gameObject.tag && other.tag == GeneralEnums.GameObjectTags.Player) {
            playerObject = other.transform.gameObject;
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // if 'other' is player, the player is seen
        // note, we don't really need to check the transform tag since the collision matrix is set to only 'see' collisions with the player layer
        if (other.tag != gameObject.tag && other.tag == GeneralEnums.GameObjectTags.Player) {
            isPlayerInRange = false;
        }
    }

    bool PlayerInFieldOfView() {
        // check if the player is within the enemy's field of view
        // this is only checked if the player is within the enemy's sight range

        // find the angle between the enemy's 'forward' direction and the player's location and return true if it's within 65 degrees (for 130 degree field of view)
 
        Vector2 directionToPlayer = playerObject.transform.position - transform.position; // represents the direction from the enemy to the player    
        Debug.DrawLine(transform.position, playerObject.transform.position, Color.magenta); // a line drawn in the Scene window equivalent to directionToPlayer
        
        Vector2 lineOfSight = lineOfSightEnd.position - transform.position; // the centre of the enemy's field of view, the direction of looking directly ahead
        Debug.DrawLine(transform.position, lineOfSightEnd.position, Color.yellow); // a line drawn in the Scene window equivalent to the enemy's field of view centre
        
        // calculate the angle formed between the player's position and the centre of the enemy's line of sight
        float angle = Vector2.Angle(directionToPlayer, lineOfSight);
        targetPosition = new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);
        // if the player is within 65 degrees (either direction) of the enemy's centre of vision (i.e. within a 130 degree cone whose centre is directly ahead of the enemy) return true
        if (angle < 110) {            
            isTargetInFront = true;
            return true;
        }
        else {
            isTargetInFront = false;
            return true;            
        }
            
    }

    bool PlayerHiddenByObstacles() {
        float distanceToPlayer = Vector2.Distance(transform.position, playerObject.transform.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, playerObject.transform.position - transform.position, distanceToPlayer);
        Debug.DrawRay(transform.position, playerObject.transform.position - transform.position, Color.blue); // draw line in the Scene window to show where the raycast is looking
        List<float> distances = new List<float>();
     
        foreach (RaycastHit2D hit in hits) {           
            // ignore the enemy's own colliders (and other enemies)
            if (hit.transform.tag == "Enemy")
                continue;
            
            // if anything other than the player is hit then it must be between the player and the enemy's eyes (since the player can only see as far as the player)
            if (hit.transform.tag != "Player") {
                return true;
            }
        }

        // if no objects were closer to the enemy than the player return false (player is not hidden by an object)
        return false; 
    }
   
}
