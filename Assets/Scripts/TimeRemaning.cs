using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRemaning : MonoBehaviour {

    float timeInSeconds = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetTimeForLevel(float time)
    {
        timeInSeconds = time;
    }

    IEnumerator ClockAnimation()
    {
        while(timeInSeconds >= 0)
        {
            timeInSeconds -= 1;
            DisplayTime();
            yield return new WaitForSeconds(1);
        }
    }

    void DisplayTime()
    {

    }
}
