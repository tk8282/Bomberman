using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used for character movement
public class MovementController : MonoBehaviour
{
    //reference to rigidbody
    public Rigidbody2D rigidbody { get; private set; }

    //used for tracking direction (down as default)
    private Vector2 direction = Vector2.down;

    //used to assign character speed
    public float speed = 5f;

    //differentiate different input keys for multiple players (allows for changes in the editor)
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    //for the different animation
    public Animation spriteAnimationUp;
    public Animation spriteAnimationDown;
    public Animation spriteAnimationLeft;
    public Animation spriteAnimationRight;
    private Animation currentSprite;

    //for player death
    public Animation spriteAnimationDeath;

    //called when scene is first initialized
    private void Awake()
    {   
        //assign rigidbody component to variable
        rigidbody = GetComponent<Rigidbody2D>();

        //set default to down
        currentSprite = spriteAnimationDown;
    }

    //called every frame
    private void Update()
    {
        //checks for key being pressed or held every frame
        if(Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteAnimationUp);
        }
        else if(Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteAnimationDown);
        }
        else if(Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteAnimationLeft);
        }
        else if(Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteAnimationRight);
        }
        else //if player does not press anything 
        {
            SetDirection(Vector2.zero, currentSprite);
        }

    }

    //used to assign the new direction based on what movement key was pressed
    private void SetDirection(Vector2 newDirection, Animation spriteRenderer)
    {
        direction = newDirection;

        
        spriteAnimationUp.enabled = spriteRenderer == spriteAnimationUp;
        spriteAnimationDown.enabled = spriteRenderer == spriteAnimationDown;
        spriteAnimationLeft.enabled = spriteRenderer == spriteAnimationLeft;
        spriteAnimationRight.enabled = spriteRenderer == spriteAnimationRight;

        //set current sprite to last sprite
        currentSprite = spriteRenderer;
        //set to idle if vector is zero
        currentSprite.idle = direction == Vector2.zero;
    }

    //called for fixed time, used for physics (fps can affect physics if placed in update)
    private void FixedUpdate()
    {
        //character position
        Vector2 position = rigidbody.position;

        //how much the character will move
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        //perform the actual move
        rigidbody.MovePosition(position + translation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if object that collided is on the explosion layer
        if(other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            PlayerDeath();
        }
    }

    //when a player dies
    private void PlayerDeath()
    {
        //disables movement
        enabled = false;

        //disables bomb placements
        GetComponent<BombController>().enabled = false;

        //disable sprites
        spriteAnimationUp.enabled = false;
        spriteAnimationDown.enabled = false;
        spriteAnimationLeft.enabled = false;
        spriteAnimationRight.enabled = false;

        //enable death animation
        spriteAnimationDeath.enabled = true;

        //completely turn off player game object
        Invoke(nameof(OnDeathAnimationEnd), 1.5f);
    }

    private void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWin();
    }

    public void playerWin(GameObject winner)
    {
        //disable movement
        enabled = false;

        //disable bomb placement
        winner.GetComponent<BombController>().enabled = false;
    }

}
