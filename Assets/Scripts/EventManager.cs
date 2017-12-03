using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EventManager : MonoBehaviour
{

    [SerializeField]
    GameObject prefabBoatEvent;
    [SerializeField]
    GameObject prefabThiefEvent;
    [SerializeField]
    GameObject prefabLostEvent;
    [SerializeField]
    GameObject prefabWoundedEvent;
    [SerializeField]
    GameObject prefabCarFireEvent;

    static List<GameObject> spawnsPointForEvent = new List<GameObject>(); //List of spawn point for all event

    static int numberOfEvent = 10;

    static List<Evenement> evenements = new List<Evenement>();

    static Evenement currentEvenement;

    static GameObject player;

    //type of event
    enum EventType
    {
        THIEF,
        CAR_FIRE,
        WOUNDED,
        LOST,
        BOAT,
        LENGTH
    }

    struct Evenement
    {
        EventType type;
        GameObject spawnPoint;
        GameObject mainObject;
        GameObject npc;
        Rigidbody2D rigid;
        bool rescued;
        SkeletonAnimation skeletonAnimation;
        bool lookingRight;

        public Evenement(EventType eventType, GameObject spawn, GameObject coll)
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
        }

        public void Velocity()
        {

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
            rigid.velocity = (player.transform.position - npc.transform.position).normalized * 4.5f;
            
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

    Evenement emptyEvenement = new Evenement(EventType.LENGTH, null, null);

    public static EventManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Multiple instances of SoundEffects");
        }
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //Get all spawn point 
        GameObject[] tmpSpawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        GameObject[] tmpSpawnPointWater = GameObject.FindGameObjectsWithTag("SpawnPointWater");
        spawnsPointForEvent.InsertRange(0, tmpSpawnPoint);
        spawnsPointForEvent.InsertRange(spawnsPointForEvent.Count - 1, tmpSpawnPointWater);

        GenerateAllEventForTheDay();

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEvenement.GetEventType() != EventType.LENGTH && currentEvenement.GetNPC() != null)
        {
            currentEvenement.SetVelocity();
        }
    }

    void GenerateAllEventForTheDay()
    {
        for (int i = 0; i < numberOfEvent; i++)
        {
            //Random event
            EventType tmpEventType;

            //Random free Position
            GameObject tmpSpawn = null;
            bool found = false;
            while (!found)
            {
                tmpSpawn = spawnsPointForEvent[Random.Range(0, spawnsPointForEvent.Count)];

                if (CheckIfPositionIsFree(tmpSpawn))
                {
                    found = true;
                }
            }

            GameObject tmpMainObject = null;

            if(tmpSpawn.tag == "SpawnPointWater")
            {
                tmpEventType = EventType.BOAT;
                tmpMainObject = Instantiate(prefabBoatEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
            }
            else
            {
                tmpEventType = (EventType)Random.Range(0,(float)EventType.BOAT);

                //Create new evenement
                switch(tmpEventType)
                {
                    case EventType.CAR_FIRE:
                        tmpMainObject = Instantiate(prefabCarFireEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                        break;

                    case EventType.LOST:
                        tmpMainObject = Instantiate(prefabLostEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                        break;

                    case EventType.THIEF:
                        tmpMainObject = Instantiate(prefabThiefEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                        break;

                    case EventType.WOUNDED:
                        tmpMainObject = Instantiate(prefabWoundedEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                        break;
                }
        }

        evenements.Add(new Evenement(tmpEventType,tmpSpawn,tmpMainObject));
        }

        foreach(GameObject spawn in spawnsPointForEvent)
        {
            Destroy(spawn);
        }
    }

    static bool CheckIfPositionIsFree(GameObject spawn)
    {
        foreach (Evenement evenement in evenements)
        {
            if (evenement.GetSpawnPoint().GetInstanceID() == spawn.GetInstanceID())
            {
                return false;
            }
        }

        return true;
    }

    static public void LaunchEvent(GameObject currentEventZone)
    { 
        if (currentEvenement.GetMainObject() == null)
        {
            foreach (Evenement evenement in evenements)
            {
                if (evenement.GetMainObject().GetInstanceID() == currentEventZone.GetInstanceID())
                {
                    currentEvenement = evenement;
                    currentEvenement.GetNPC().transform.parent = player.transform;
                    currentEvenement.GetMainObject().GetComponent<Collider2D>().enabled = false;
                    currentEvenement.SetRescued();
                    break;
                }
            }
        }
    }

    static public void EndEvent()
    {
        evenements.Remove(currentEvenement);
        Destroy(currentEvenement.GetNPC());
        Destroy(currentEvenement.GetMainObject());
    }

    static public LayerMask ZoneToGo()
    {
        Debug.Log(currentEvenement.GetEventType());
        switch (currentEvenement.GetEventType())
        {
            case EventType.BOAT:
            case EventType.CAR_FIRE:
            case EventType.WOUNDED:
                return LayerMask.NameToLayer("Hospital");

            case EventType.LOST:
                return LayerMask.NameToLayer("House");

            case EventType.THIEF:
                return LayerMask.NameToLayer("PoliceStation");
        }

        Debug.LogError("NO ZONE SELECTED, ERROR IN EVENT TYPE");
        return LayerMask.NameToLayer("");
    }
}


