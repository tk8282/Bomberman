using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //list of powerup types
    public enum PowerUpType
    {
        ExtraBomb, BlastSize, Speed,
    }

    public PowerUpType type;

    //detect if player picked up
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if the object that collided with power is the player
        if(other.CompareTag("Player"))
        {
            OnPowerUpPickUp(other.gameObject);
        }
    }

    //function for when the player does pick up powerup
    private void OnPowerUpPickUp(GameObject player)
    {
        switch(type)
        {
            case PowerUpType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
            break;
            case PowerUpType.BlastSize:
                player.GetComponent<BombController>().explosionSize++;
            break;
            case PowerUpType.Speed:
                player.GetComponent<MovementController>().speed++;
            break;
        }

        Destroy(gameObject);
    }
}
