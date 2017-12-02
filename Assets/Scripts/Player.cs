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
        Debug.Log(state);
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        movement = new Vector2(speed * horizontal,
                               speed * vertical);

        switch(state)
        {
            case EventState.OUT_EVENT_ZONE:
                eventEnCours = false;
                break;

            case EventState.ON_EVENT_ZONE:
                if(Input.GetMouseButtonDown(0))
                {
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Event") && !eventEnCours)
        {
            state = EventState.ON_EVENT_ZONE;
        }

        /*if(collision.gameObject.layer == ZoneToGo && state == EventState.ON_EVENT)
        {
            
        }*/
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Event") && !eventEnCours)
        {
            state = EventState.OUT_EVENT_ZONE;
        }
    }
}
