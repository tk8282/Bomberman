using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    //how long for block to break
    public float breakTime = 1f;

    private void Start()
    {
        Destroy(gameObject, breakTime);
    }

    //spawn chance for powerups
    public float powerUpSpawnChance = 0.2f;

    //array of possible powerups
    public GameObject[] spawnablePowerups;

    //when destroyed
    private void OnDestroy()
    {
        //if the number chance hits the percentage chance
        if(spawnablePowerups.Length > 0 && Random.value < powerUpSpawnChance)
        {
            //choose a random powerup and spawn on location
            int randomIndex = Random.Range(0, spawnablePowerups.Length);
            Instantiate(spawnablePowerups[randomIndex], transform.position, Quaternion.identity);
        }
    }

}
