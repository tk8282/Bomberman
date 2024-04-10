using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    //reference to bomb prefab
    public GameObject bombPrefab;

    //define keycode to place bomb (changeable for different players)
    public KeyCode inputKey = KeyCode.Space;

    //time for bomb to explode
    public float bombTime = 3f;

    //max amount of bombs at one time by one player (power-ups will affect)
    public int bombAmount = 1;

    //track the amount of bombs
    private int bombsRemaining;

    //reference to explosion prefab
    public ExplosionScript explosionPrefab;

    //how long explosion lasts for (Power-ups can affect)
    public float explosionTime = 1f;

    //explosion size
    public int explosionSize = 1;

    //layer mask for overlap function
    public LayerMask explosionLayerMask;

    //reference to tilemap
    public Tilemap breakableTiles;

    //reference to breakable prefab
    public Breakable breakablePrefab;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if(bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(placeBomb());
        }
    }

    //use of coroutine to perform logic after 
    private IEnumerator placeBomb()
    {
        //where player is 
        Vector2 position = transform.position;

        //rounding to ensure placing bomb is aligned to the grid
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //instantiate bomb prefab
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        //suspend function until bomb explosion
        yield return new WaitForSeconds(bombTime);

        //explosion
        //retrack position incase bomb was pushed since placement
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //instantiate explosion prefab
        ExplosionScript explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        //set current renderer
        explosion.SetCurrentRenderer(explosion.start);

        //destroy gameobject
        Destroy(explosion.gameObject, explosionTime);

        //call explode for each direction
        Explode(position, Vector2.up, explosionSize);
        Explode(position, Vector2.down, explosionSize);
        Explode(position, Vector2.left, explosionSize);
        Explode(position, Vector2.right, explosionSize);

        Destroy(bomb);
        bombsRemaining++;

    }

    
    //exiting trigger zone, will turn off trigger and have proper collision
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
    
    //recusive function to extend explosion in directions and size
    //will continue exploding until length is 0
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        //exit condition
        if(length <= 0)
        {
            return;
        }

        position += direction;

        //check to ensure that a tile is not populated by something
        //allows for no overlapping with the explosition sprite, if something is there return and stop function
        if(Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            clearBlock(position);
            return;
        }

        //instantiate new explosion
        ExplosionScript explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        
        //depending on length change explosion type
        //once there is one length left, then explosion is at end
        explosion.SetCurrentRenderer(length > 1 ? explosion.middle : explosion.end);

        //set direction/rotation
        explosion.SetDirection(direction);

        //destroy explosion after the certain time
        Destroy(explosion.gameObject, explosionTime);

        //minus -1 to length and run again
        Explode(position, direction, length - 1);
    }

    //clear block at given position
    private void clearBlock(Vector2 position)
    {
        Vector3Int cell = breakableTiles.WorldToCell(position);
        TileBase tile = breakableTiles.GetTile(cell);

        //check if tile is null
        if(tile != null)
        {
            //removing tile underneath and replacing it with animated tile
            Instantiate(breakablePrefab, position, Quaternion.identity);
            breakableTiles.SetTile(cell, null);
        }
    }
    
    //function to add a bomb to player after Power-up
    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }
}
