using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to loop through sprites to perform animation
public class Animation : MonoBehaviour
{
    //reference to sprite renderer
    private SpriteRenderer spriteRenderer;

    // store sprites
    public Sprite idleSprite;
    public Sprite [] animationSprites;

    // variables to check if sprites are done or need to loop
    public bool loop = true;
    public bool idle = true;

    //every .25 of a second, the frame will advance
    public float animationTime = .25f;
    private int animationFrame;

    //assign sprite renderer
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //set sprite to on
    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    //set sprite to off
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        //invoke a function
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    private void NextFrame()
    {
        animationFrame++;

        //if loop is true
        //if animationFrame is greater or equal to array, it will be out of bounds so set to 0
        if(loop && animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        //if sprite is idle
        if(idle)
        {
            spriteRenderer.sprite = idleSprite;
        }
        //esnure that spriterenderer will not attempt to access array out of bounds
        else if(animationFrame >= 0 && animationFrame < animationSprites.Length)
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
}
