using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Image gaugeIntoxication;

    float horizontal;
    float vertical;
    Vector2 movement;
    Rigidbody2D body;
    bool eventEnCours = false;
    GameObject currentEventZone = null;
    bool isInSafeZone = false;
    float intoxicationLevel = 0.0f;
    float timeInSecondsCanGoOut = 120f;

    public enum EventState
    {
        OUT_EVENT_ZONE,
        ON_EVENT_ZONE,
        ON_EVENT
    }
    EventState state = EventState.OUT_EVENT_ZONE;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine(getIntoxicate());
	}

    void Update ()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        movement = new Vector2(speed * horizontal,
                               speed * vertical);

        switch(state)
        {
            case EventState.OUT_EVENT_ZONE:
                eventEnCours = false;
                currentEventZone = null;
                break;

            case EventState.ON_EVENT_ZONE:
                    EventManager.LaunchEvent(currentEventZone);
                    state = EventState.ON_EVENT;
                
                break;

            case EventState.ON_EVENT:
                eventEnCours = true;
                break;
        }

        Debug.Log(intoxicationLevel);
    }

    IEnumerator getIntoxicate()
    {
        while(true)
        {
            if(!isInSafeZone)
            {
                intoxicationLevel += 100 / timeInSecondsCanGoOut;
                gaugeIntoxication.fillAmount = intoxicationLevel / 100;
            }
            yield return new WaitForSeconds(1);
        }
    }

    void FixedUpdate()
    {
        body.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.layer == LayerMask.NameToLayer("Event")) && !eventEnCours)
        {
            state = EventState.ON_EVENT_ZONE;
            currentEventZone = collision.gameObject;
        }

        if(eventEnCours && collision.gameObject.layer == EventManager.ZoneToGo())
        {
            EventManager.EndEvent();
            state = EventState.OUT_EVENT_ZONE;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("House") || collision.gameObject.layer == LayerMask.NameToLayer("Hospital") || collision.gameObject.layer == LayerMask.NameToLayer("PoliceStation"))
        {
            isInSafeZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Event") && !eventEnCours)
        {
            state = EventState.OUT_EVENT_ZONE;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("House") || collision.gameObject.layer == LayerMask.NameToLayer("Hospital") || collision.gameObject.layer == LayerMask.NameToLayer("PoliceStation"))
        {
            isInSafeZone = false;
        }
    }
}
