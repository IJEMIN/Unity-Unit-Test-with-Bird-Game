using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public class Bird : MonoBehaviour 
{
	public float upForce;                   //Upward force of the "flap".
	private bool isDead = false;            //Has the player collided with a wall?

	private Animator anim;                  //Reference to the Animator component.
	private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.

	public IUnityInputService UnityInput;

	public bool IsDead
	{
		get { return isDead; }
		private set { isDead = value; }
	}

	void Start()
	{
		//Get reference to the Animator component attached to this GameObject.
		anim = GetComponent<Animator> ();
		//Get and store a reference to the Rigidbody2D attached to this GameObject.
		rb2d = GetComponent<Rigidbody2D>();

		if (UnityInput == null)
		{
			UnityInput = new UnityInputService();
		}
		
	}

	void Update()
	{
		//Don't allow control if the bird has died.
		if (isDead == false) 
		{
			//Look for input to trigger a "flap".
			if (UnityInput.GetButtonDown("Fire1"))
			{
				Jump();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// Zero out the bird's velocity
		rb2d.velocity = Vector2.zero;
		// If the bird collides with something set it to dead...
		isDead = true;
		//...tell the Animator about it...
		anim.SetTrigger("Die");
		//...and tell the game control about it.

		if (GameControl.instance != null)
		{
			GameControl.instance.BirdDied();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{

		//If the bird hits the trigger collider in between the columns then
		//tell the game control that the bird scored.
		GameControl.instance.BirdScored();

	}

	public void Jump()
	{
		//...tell the animator about it and then...
		anim.SetTrigger("Flap");
		//...zero out the birds current y velocity before...
		rb2d.velocity = Vector2.zero;
		//  new Vector2(rb2d.velocity.x, 0);
		//..giving the bird some upward force.
		rb2d.AddForce(new Vector2(0, upForce));
	}
}