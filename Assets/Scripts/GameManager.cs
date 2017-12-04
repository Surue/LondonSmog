using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    int currentScoreLost;
    int currentScoreBoat;
    int currentScoreCarFire;
    int currentScoreWounded;
    int currentScoreLostObject;

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
        InfoPlayer.Instance.AddScore(currentScoreBoat, currentScoreCarFire, currentScoreLost, currentScoreLostObject, currentScoreWounded);

        float gainTime = InfoPlayer.Instance.GetTimeInSecondsForADay() - player.GetPassedTime();
        InfoPlayer.Instance.AddTimeForNextDay(gainTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FinishDay()
    {
        InfoPlayer.Instance.AddScore(currentScoreBoat,currentScoreCarFire,currentScoreLost,currentScoreLostObject,currentScoreWounded);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndOfTime()
    {
        FinishDay();
    }

    public void AddScore(Evenement.EventType evenement)
    {
        switch(evenement)
        {
            case Evenement.EventType.BOAT:
                currentScoreBoat++;
                break;

            case Evenement.EventType.CAR_FIRE:
                currentScoreCarFire++;
                break;

            case Evenement.EventType.WOUNDED:
                currentScoreWounded++;
                break;

            case Evenement.EventType.LOST:
                currentScoreLost++;
                break;

            case Evenement.EventType.LOST_OBJECT:
                currentScoreLostObject++;
                break;
        }
    }
}
