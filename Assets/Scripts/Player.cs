using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontal;
    float vertical;
    Vector2 movement;
    [SerializeField]
    float speed;
    Rigidbody2D body;
    bool eventEnCours = false;
    GameObject currentEventZone = null;

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
                if(Input.GetMouseButtonDown(0))
                {
                    EventManager.LaunchEvent(currentEventZone);
                    state = EventState.ON_EVENT;
                }
                break;

            case EventState.ON_EVENT:
                eventEnCours = true;
                break;
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
            Debug.Log("Fin de l'event");
            EventManager.EndEvent();
            state = EventState.OUT_EVENT_ZONE;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Event") && !eventEnCours)
        {
            state = EventState.OUT_EVENT_ZONE;
        }
    }
}
