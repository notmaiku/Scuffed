using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    private Animator anim;
    private const int magnitude=2000;
    public GameObject player;
    Collider coll;
    public float hitTime = 0;
    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
        player = GameObject.FindWithTag("Player");
        anim=player.GetComponent<Animator>();
        coll = GetComponent<Collider>();
        coll.isTrigger = false;
    }

// check within animation scope so that hitbox isn't hitting everything when player is not swinging
    void Update(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&  this.hitTime < anim.GetCurrentAnimatorStateInfo(0).length){
            coll.isTrigger = true;
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("None")){
            this.hitTime = 0;
        }
    }
    void OnTriggerEnter(Collider other) {
        coll.isTrigger = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
        try {
            // GetHashCode reference to hitting thin component through unity api 
            // it's cool no import just grab it through this
            this.hitTime += 1;
            EnemyController enemy = other.GetComponent("EnemyController") as EnemyController;
            enemy.HP -= 25;
            print(enemy.HP);
        } catch (System.NullReferenceException e) {
            print("ERROR: Bullet collided with a non enemy - " + e);
        }
        }
    }

// Test where the hitbox so like sword is hitting enemy or thing
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
