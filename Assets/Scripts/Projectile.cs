using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    // Contrary to Start, Awake is called immediately when the object is created (when Instantiate is called), so Rigidbody2d is properly initialized before calling Launch.
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    /*
    This function calls AddForce on the Rigidbody with the force being the direction multiplied by the force. 
    When that force is added, the Physics Engine will move the Projectile every frame based on that force and direction. 
    */
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }
        //we also add a debug log to know what the projectile touch
        // Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    }

    // // Update is called once per frame
    void Update()
    {
        // clean up the cog far from the scene
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
}
