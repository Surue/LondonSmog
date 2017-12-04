using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlayer : MonoBehaviour {

    int scoreLost;
    int scoreBoat;
    int scoreCarFire;
    int scoreWounded;
    int scoreLostObject;

    int currentDay = 0;

    const float dayDurationInSeconds = 120f;

    float timeGaineLastTime = 0;

    public static InfoPlayer Instance;
    Player player;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
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
}
