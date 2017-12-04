using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    int indexLastScene = -1;
    Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();

        switch(SceneManager.GetActiveScene().name){
            case "Level_1":
            case "Level_2":
            case "Level_3":
                InfoPlayer.Instance.NewDay();
                break;

            default:
                Debug.Log(SceneManager.GetActiveScene().name);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadSceneWithName(string nameScene)
    {
        indexLastScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nameScene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLastScene()
    {
        InfoPlayer.Instance.RestartDay();
        SceneManager.LoadScene(SceneManager.GetSceneAt(indexLastScene).name);
    }

    public void Death()
    {
        SceneManager.LoadScene("DeathScreen");
    }

    public void SuccesDay()
    {
        Debug.Log("GameManager: SuccesDay()");
        float gainTime = InfoPlayer.Instance.GetTimeInSecondsForADay() - player.GetPassedTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
