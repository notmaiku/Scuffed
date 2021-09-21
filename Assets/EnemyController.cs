using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HP = 100;
  [SerializeField]
  private Color bloody = Color.red;

  [SerializeField]
  private Color skin = new Color(53f, 31f, 23f); 
    void Start()
    {
            gameObject.GetComponent<Renderer>().material.color = skin;
    }

    void Update()
    {
        if(HP <= 50){
            gameObject.GetComponent<Renderer>().material.color = bloody;
        // raycastHit.collider.GetComponent<Renderer>().material.color = tintColor;
        }
        if(HP <= 0){
            Destroy(gameObject);
            print("enemy dead");
        }
    }
}
