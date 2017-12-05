using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlayer : MonoBehaviour {

    int scoreLost = 0;
    int scoreBoat = 0;
    int scoreCarFire = 0;
    int scoreWounded = 0;
    int scoreLostObject = 0;

    int currentDay = 0;

    const float dayDurationInSeconds = 120f;

    float timeGaineLastTime = 0;

    public static InfoPlayer Instance;
    Player player;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Multiple instances of InfoPlayer");
        }
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DisplayTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
        }
    }

    //All logic for th enew day
    public void NewDay()
    {
        currentDay++;
        //function for the player
        player = GameObject.FindObjectOfType<Player>();
        player.UpdateIntoxication((currentDay -1) * 10);
        player.UpdateTimeForTheDay(dayDurationInSeconds + timeGaineLastTime);

        player.LauncheCoroutine();
        
        GameObject.FindObjectOfType<TimeRemaning>().SetTimeForLevel(dayDurationInSeconds + timeGaineLastTime);
        timeGaineLastTime = 0;
    }

    public void RestartDay()
    {
        currentDay--;
    }

    public float GetTimeInSecondsForADay()
    {
        return dayDurationInSeconds;
    }

    public void AddTimeForNextDay(float timeExcedant)
    {
        timeGaineLastTime = timeExcedant;
    }

    public void AddScore(int scoreBoat, int scoreCarFire, int scoreLost, int scoreLostObject, int scoreWounded)
    {
        this.scoreBoat += scoreBoat;
        this.scoreCarFire += scoreCarFire;
        this.scoreLost += scoreLost;
        this.scoreLostObject += scoreLostObject;
        this.scoreWounded += scoreWounded;
    }

    public int GetScore()
    {
        return scoreLost + scoreBoat + scoreCarFire + scoreWounded + scoreLostObject;
    }
}
