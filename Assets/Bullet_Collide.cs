using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Collide : MonoBehaviour {
  // public Rigidbody explosion;
  // public float radius = 100.0F;
  // public float power = 100.0F;
  // private bool exploded=false;

  void OnCollisionEnter(Collision collision) {
    try {
        EnemyController enemy = collision.collider.GetComponent("EnemyController") as EnemyController;
        enemy.HP -= 25;
        print(enemy.HP);
    } catch (System.NullReferenceException e) {
        print("ERROR: Bullet collided with a non enemy - " + e);
    }
    // if(!exploded)
    // {
    //     exploded=true;
    //     Vector3 explosionPos = transform.position;
    //     Rigidbody bomb = Instantiate(explosion, explosionPos, transform.rotation);
    //     Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
    //     foreach (Collider hit in colliders) {
    //         try {
    //             EnemyController enemy = hit.GetComponent("EnemyController") as EnemyController;
    //             enemy.HP -= 50;
    //             print(enemy.HP);
    //         } catch (System.NullReferenceException e) {
    //             print("ERROR: Bullet collided with a non enemy - " + e);
    //         }
    //     }
    // }
  }
}
