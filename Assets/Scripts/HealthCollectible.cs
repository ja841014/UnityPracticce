using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

	// it calls this OnTriggerEnter2D function on the first frame when it detects a new Rigidbody entering the Trigger
	void OnTriggerEnter2D(Collider2D other) {
		// if enemy collide with this it will retinr null because it is not a RubyController class
		RubyController controller = other.GetComponent<RubyController>();
		if(controller.health < controller.maxHealth) {
			if(controller != null) {
				controller.changeHealth(1);
				Destroy(gameObject);
			}
		}
		
	}

}
