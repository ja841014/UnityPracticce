using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    // need to make it a GameObject type because that’s what Prefabs are
    public GameObject projectilePrefab;
    public int health { get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    float horizontal;
    float vertical;
    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    // Start is called before the first frame update
    void Start()
    {
        //  It tells Unity to give you the Rigidbody2D that is attached to the same GameObject that your script is attached to,
        //  which is your character.
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        // Use Mathf.Approximately instead of == because the way computers store float numbers means there is a tiny loss in precision.
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            // you will normalize vectors that store direction because length is not important, only the direction is
            lookDirection.Normalize();
        }
        
        /*
        Then you have the three lines that send the data to the Animator, 
        which is the direction you look in and the speed 
        (the length of the move vector). 
        */
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(isInvincible) {
            invincibleTimer -= Time.deltaTime;
            Debug.Log(invincibleTimer);
            if(invincibleTimer < 0) {
                Debug.Log("i am in if condition");
                isInvincible = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // A layer mask which allows us to test only certain layers. Any layers that are not part of the mask will be ignored during the intersection test. Here, you will select the NPC layer, because that one contains your frog. 
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPCDialog character = hit.collider.GetComponent<NPCDialog>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

    }

    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal *  Time.deltaTime;
        position.y = position.y + speed * vertical  * Time.deltaTime;
        // Instead of setting the new position with 
        // transform.position = position; you are now doing it with the Rigidbody position
        rigidbody2d.MovePosition(position);
    }

    public void changeHealth(int amount) {
        if(amount < 0) {
            animator.SetTrigger("Hit");
            if(isInvincible) {
                return;
            }
            Debug.Log("isInvincible: " + isInvincible);
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        // This sets the current health using another built-in function called Mathf.Clamp. 
        // Clamping ensures that the first parameter (here currentHealth + amount) 
        // never goes lower than the second parameter (here 0) and 
        // never goes above the third parameter (maxHealth)
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        // Debug.Log(currentHealth + "/" + maxHealth);
    }

    // public int health { get { return currentHealth; }}
    // public int getcurrentHealth() {
    //     return currentHealth;
    // }

    void Launch()
    {
        /*
        Instantiate is a Unity function that you haven’t seen yet. 
        Instantiate takes an object as the first parameter and 
        creates a copy at the position in the second parameter, 
        with the rotation in the third parameter. => Quaternion.identity means “no rotation”.
        */
        /*
        The object you will copy is your Prefab and you will place it at the position of your Rigidbody (but offset it a bit upward so the object is near Ruby’s hands not her feet), with a rotation of Quaternion.identity.
        */
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}
