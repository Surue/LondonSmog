using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Evenement : MonoBehaviour {

    EventType type;
    GameObject spawnPoint;
    GameObject mainObject;
    GameObject npc;
    Rigidbody2D rigid;
    bool rescued;
    SkeletonAnimation skeletonAnimation;
    bool lookingRight;
    Collider2D collider;
    Player player;

    public enum EventType
    {
        CAR_FIRE,
        WOUNDED,
        LOST,
        LOST_OBJECT,
        BOAT,
        LENGTH
    }

    bool soundPlayed = false;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<Player>();
    }

    public void Set(EventType eventType,GameObject spawn,GameObject coll)
    {
        type = eventType;
        spawnPoint = spawn;
        mainObject = coll;
        if(eventType != EventType.LENGTH)
        {
            npc = mainObject.transform.Find("NPC").gameObject;
            rigid = npc.GetComponent<Rigidbody2D>();
            skeletonAnimation = npc.GetComponent<SkeletonAnimation>();
        }
        else
        {
            npc = null;
            rigid = null;
            skeletonAnimation = null;
        }
        rescued = false;

        lookingRight = false;

        collider = npc.GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(npc.transform.position, player.transform.position) < 7 && !rescued && !soundPlayed)
        {
            switch(type)
            {
                case EventType.LOST:
                    SoundManager.instance.Hello();
                    break;

                case EventType.BOAT:
                case EventType.CAR_FIRE:
                case EventType.WOUNDED:
                    SoundManager.instance.Help();
                    break;
            }
            soundPlayed = true;
        }

        if(Vector3.Distance(npc.transform.position,player.transform.position) > 8 && soundPlayed)
        {
            soundPlayed = false;
        }
    }

    public GameObject GetSpawnPoint()
    {
        return spawnPoint;
    }

    public GameObject GetMainObject()
    {
        return mainObject;
    }

    public EventType GetEventType()
    {
        return type;
    }

    public GameObject GetNPC()
    {
        return npc;
    }

    public void SetRescued()
    {
        rescued = true;
        collider.enabled = true;
    }

    public bool IsRescued()
    {
        return rescued;
    }

    void Flip()
    {
        Vector3 theScale = npc.transform.localScale;
        theScale.x *= -1;
        npc.transform.localScale = theScale;
        lookingRight = !lookingRight;
    }

    public void SetVelocity()
    {
        rigid.velocity = (EventManager.player.transform.position - npc.transform.position).normalized * 5f;

        if(Mathf.Abs(rigid.velocity.x) > Mathf.Abs(rigid.velocity.y))
        {
            if(rigid.velocity.x > 0)
            {
                if(!lookingRight)
                {
                    Flip();
                }

                skeletonAnimation.AnimationName = "Marche_Cote_Femme";
            }
            else
            {
                if(lookingRight)
                {
                    Flip();
                }

                skeletonAnimation.AnimationName = "Marche_Cote_Femme";
            }
        }
        else
        {
            if(rigid.velocity.y > 0)
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
