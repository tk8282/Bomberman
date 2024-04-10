using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_Dropdown mapSelect;

    public void StartGame()
    {
        //load the scene based on the player select
        SceneManager.LoadScene(mapSelect.value + 1);
    }
}
