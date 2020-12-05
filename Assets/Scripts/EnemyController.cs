using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 3.0f;
    public bool vertical;
    public  float changeTime = 3.0f;
    public ParticleSystem smokeEffect;
    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1; 
    bool broken = true;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        // Debug.Log(timer);
    }

    void Update() {
        if(!broken)
        {
            return;
        }
        timer = timer - Time.deltaTime;
        // Debug.Log(timer);
        if(timer <= 0) {
            direction = -direction;
            timer = changeTime;
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        rigidbody2d.MovePosition(position);
    }
    /*
    Unlike your damage zone, 
    you can’t use a Trigger because 
    you want the enemy Collider to be “solid” and actually collide with 
    things. Thankfully, Unity offers a second set of functions!
    */
    /*
    the type of other here is Collision2D not Collider2D. 
    A Collision2D doesn’t have a GetComponent function, 
    but it contains lots of data about the collision, 
    like the GameObject with which the enemy collided. 
    So you call GetComponent on that GameObject
    */
    void OnCollisionEnter2D(Collision2D other) { 
        RubyController player = other.gameObject.GetComponent<RubyController>();
        if(player != null) {
            player.changeHealth(-1);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        // This removes the Rigidbody from the Physics System simulation, so it won’t be taken into account by the system for collision, and the fixed robot won’t stop the Projectile anymore or be able to hurt the main character.
        rigidbody2d.simulated = false;
        smokeEffect.Stop();
        // Destroy(smokeEffect.gameObject);
    }
}
