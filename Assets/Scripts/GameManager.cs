using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //reference to text component
    public Text winText;

    //reference to panel
    public GameObject endGamePanel;

    //reference to movement controller
    public MovementController movementController;

    //
    private bool winTextSet = false;

    //check if player is last alive
    //reference to players
    public GameObject[] players;

    public void CheckWin()
    {
        int aliveCount = 0;

        foreach(GameObject player in players)
        {
            if(player.activeSelf)
            {
                aliveCount++;
            }
        }

        if(aliveCount <= 1)
        {
            Invoke(nameof(EndGame), .5f);
        }
    }

    private void EndGame()
    {

        //win text is already chosen exit
        if (winTextSet)
        {
            return;
        }

        int activePlayerCount = 0;
        GameObject lastActivePlayer = null;

    foreach (GameObject player in players)
    {
        if (player.activeSelf)
        {
            activePlayerCount++;
            lastActivePlayer = player;
        }
    }

    if (activePlayerCount == 1)
    {
        //
        GameObject winner = lastActivePlayer;
        MovementController winnerMovementController = winner.GetComponent<MovementController>();
        winnerMovementController.playerWin(winner);

        //update the text to show who wins
        winText.text = winner.name + " Wins!";
        winTextSet = true;
    }
    else
    {
        //no clear winner
        winText.text = "Tie!";
        winTextSet = true;
    }
    
        endGamePanel.SetActive(true);
    }

    //function to exit game
    public void ExitGame()
    {
        Application.Quit();
    }
}
