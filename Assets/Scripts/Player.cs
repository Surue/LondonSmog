using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Player:MonoBehaviour
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
    float timeInSecondsCanGoOut = 0;
    float timePassed = 0;

    bool lookingRight = false;
    SkeletonAnimation skeletonAnimation;


    GameManager gameManager;

    public enum EventState
    {
        OUT_EVENT_ZONE,
        ON_EVENT_ZONE,
        ON_EVENT
    }
    EventState state = EventState.OUT_EVENT_ZONE;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        skeletonAnimation = transform.Find("skeleton").GetComponent<SkeletonAnimation>();
    }

    void Update()
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

        ManageAnimation();
    }

    public IEnumerator GetIntoxicate()
    {
        while(true)
        {
            if(!isInSafeZone)
            {
                intoxicationLevel += 100 / timeInSecondsCanGoOut;
                gaugeIntoxication.fillAmount = intoxicationLevel / 100;

                if(intoxicationLevel >= 100)
                {
                    gameManager.Death();
                }
            }
            timePassed += 1;
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
        
        if (eventEnCours && collision.gameObject.layer == EventManager.ZoneToGo())
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Event") && !eventEnCours)
        {
            state = EventState.OUT_EVENT_ZONE;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("House") || collision.gameObject.layer == LayerMask.NameToLayer("Hospital") || collision.gameObject.layer == LayerMask.NameToLayer("PoliceStation"))
        {
            isInSafeZone = false;
        }
    }

    public void UpdateIntoxication(int startingIntoxication)
    {
        intoxicationLevel += startingIntoxication;
    }

    public void UpdateTimeForTheDay(float timeForTheDayInSeconds)
    {
        timeInSecondsCanGoOut = timeForTheDayInSeconds;
    }

    public void LauncheCoroutine()
    {
        StartCoroutine(GetIntoxicate());
    }

    public float GetPassedTime()
    {
        return timePassed;
    }

    public bool IsInSafeZone()
    {
        return isInSafeZone;
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        lookingRight = !lookingRight;
    }

    void ManageAnimation()
    {
        if(Mathf.Abs(body.velocity.x) > 0 || Mathf.Abs(body.velocity.y) > 0)
        {
            skeletonAnimation.timeScale = 1;
        }
        else
        {
            skeletonAnimation.timeScale = 0;
        }

        if(Mathf.Abs(body.velocity.x) > Mathf.Abs(body.velocity.y))
        {
            skeletonAnimation.AnimationName = "Marche_Cote";
            if(body.velocity.x > 0)
            {
                if(!lookingRight)
                {
                    Flip();
                }
            }
            else
            {
                if(lookingRight)
                {
                    Flip();
                }
            }
        }
        else
        {
            if(body.velocity.y > 0)
            {
                skeletonAnimation.AnimationName = "Marche_Dos";
            }
            else
            {
                skeletonAnimation.AnimationName = "Marche_Face";
            }
        }
    }
}
