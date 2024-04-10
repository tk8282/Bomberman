using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    //reference sprite renderers for the different points in the explosion
    public Animation start;
    public Animation middle;
    public Animation end;

    //specifically set which renderer to use
    public void SetCurrentRenderer(Animation renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    //setting direction of sprite to face a direction
    //rotates the sprite
    public void SetDirection(Vector2 direction)
    {
        //getting arctan
        float angle = Mathf.Atan2(direction.y, direction.x);
        
        //calculate rotations
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
