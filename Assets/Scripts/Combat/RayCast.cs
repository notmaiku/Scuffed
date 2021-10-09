using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
  // Update is called once per frame
  [SerializeField]
  private Color tintColor = Color.green;
  void FixedUpdate(){
    Vector3 origin = transform.position;
    Vector3 direction = transform.forward;

    float maxDistance = 5;

    
    if(Input.GetButtonDown("Fire1")){
      shoot(origin, direction, maxDistance);
    }
  }

  public void shoot(Vector3 origin, Vector3 direction, float maxDistance){
    print("shoot a guy");
    Ray ray = new Ray(origin, direction);
    bool result = Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance);
    Debug.DrawRay(origin , direction * 5, Color.red);

    if(result){
      print("shot a guy");
        // raycastHit.collider.GetComponent<Renderer>().material.color = tintColor;
        // Destroy(raycastHit.collider.GetComponent<Renderer>());
        EnemyController enemy = raycastHit.collider.GetComponent("EnemyController") as EnemyController;
        enemy.HP -= 25;
        print(enemy.HP);
    }
  }
}
