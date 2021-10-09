using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
     // Assign a Rigidbody component in the inspector to instantiate

    public Rigidbody projectile;

    void Update() {
        // Ctrl was pressed, launch a projectile
        if (Input.GetButtonDown("Fire1")) {
            // Instantiate the projectile at the position and rotation of this transform
            Rigidbody clone;
            clone = Instantiate(projectile, transform.position, transform.rotation);

            // Give the cloned object an initial velocity along the current
            // object's Z axis
            // clone.velocity = transform.TransformDirection(Vector3.forward * 10, Space.World);
        }
    }
    
}
