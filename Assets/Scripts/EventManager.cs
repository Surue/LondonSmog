using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    static int numberOfEvent = 7;

    static List<Evenement> evenements = new List<Evenement>();

    static Evenement currentEvenement;

    public static GameObject player;

    static GameManager gameManager;

    bool levelFinised = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");

        if (SceneManager.GetActiveScene().name == "Tuto")
        {
            Evenement[] tmpEvenement = GameObject.FindObjectsOfType<Evenement>();
            foreach(Evenement tmpEvent in tmpEvenement)
            {
                switch (tmpEvent.name)
                {
                    case "EventLost":
                        tmpEvent.Set(Evenement.EventType.LOST, GameObject.Find("NPC"), tmpEvent.gameObject);
                        break;

                    case "EventWounded":
                        tmpEvent.Set(Evenement.EventType.WOUNDED, GameObject.Find("NPC"), tmpEvent.gameObject);
                        break;

                    case "EventThief":
                        tmpEvent.Set(Evenement.EventType.LOST_OBJECT, GameObject.Find("NPC"), tmpEvent.gameObject);
                        break;

                }
                
            }
            evenements.InsertRange(0, tmpEvenement);
        }

        else
        {
            //Get all spawn point 
            GameObject[] tmpSpawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
            GameObject[] tmpSpawnPointWater = GameObject.FindGameObjectsWithTag("SpawnPointWater");
            GameObject[] tmpSpawnPointHouse = GameObject.FindGameObjectsWithTag("SpawnPointHouse");
            spawnsPointForEvent.InsertRange(0, tmpSpawnPoint);
            spawnsPointForEvent.InsertRange(spawnsPointForEvent.Count - 1, tmpSpawnPointWater);
            spawnsPointForEvent.InsertRange(spawnsPointForEvent.Count - 1, tmpSpawnPointHouse);

            GenerateAllEventForTheDay();
        }

        currentEvenement = null;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEvenement != null)
        {
            currentEvenement.SetVelocity();
        }

        if(levelFinised)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if(evenements.Count == 0)
        {
            Debug.Log("Succes day");
            levelFinised = true;
            gameManager.SuccesDay();
        }
    }

    void GenerateAllEventForTheDay()
    {
        if(spawnsPointForEvent.Count != 0)
        {
            for(int i = 0;i < numberOfEvent;i++)
            {
                //Random event
                Evenement.EventType tmpEventType;

                //Random free Position
                GameObject tmpSpawn = null;
                    tmpSpawn = spawnsPointForEvent[Random.Range(0,spawnsPointForEvent.Count)];

                GameObject tmpMainObject = null;

                if(tmpSpawn.tag == "SpawnPointWater")
                {
                    tmpEventType = Evenement.EventType.BOAT;
                    tmpMainObject = Instantiate(prefabBoatEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                }
                else if(tmpSpawn.tag == "SpawnPointHouse")
                {
                    tmpEventType = Evenement.EventType.LOST_OBJECT;
                    tmpMainObject = Instantiate(prefabThiefEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                }
                else
                {
                    tmpEventType = (Evenement.EventType)Random.Range(0,(float)Evenement.EventType.LOST_OBJECT);

                    //Create new evenement
                    switch(tmpEventType)
                    {
                        case Evenement.EventType.CAR_FIRE:
                            tmpMainObject = Instantiate(prefabCarFireEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                            break;

                        case Evenement.EventType.LOST:
                            tmpMainObject = Instantiate(prefabLostEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                            break;

                        case Evenement.EventType.WOUNDED:
                            tmpMainObject = Instantiate(prefabWoundedEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                            break;
                    }
                }
                Evenement tmpEvenement = tmpMainObject.GetComponent<Evenement>();
                tmpEvenement.Set(tmpEventType,tmpSpawn,tmpMainObject);

                evenements.Add(tmpEvenement); //TO DO
            }

            foreach(GameObject spawn in spawnsPointForEvent)
            {
                Destroy(spawn);
            }
            spawnsPointForEvent.RemoveRange(0,spawnsPointForEvent.Count);
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
        if (currentEvenement == null)
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
        gameManager.AddScore(currentEvenement.GetEventType());
        evenements.Remove(currentEvenement);
        Destroy(currentEvenement.GetNPC());
        Destroy(currentEvenement.GetMainObject());
        currentEvenement = null;
    }

    static public LayerMask ZoneToGo()
    {
        switch (currentEvenement.GetEventType())
        {
            case Evenement.EventType.BOAT:
            case Evenement.EventType.CAR_FIRE:
            case Evenement.EventType.WOUNDED:
                return LayerMask.NameToLayer("Hospital");

            case Evenement.EventType.LOST:
                Debug.Log("ICI");
                return LayerMask.NameToLayer("House");

            case Evenement.EventType.LOST_OBJECT:
                return LayerMask.NameToLayer("PoliceStation");
        }

        Debug.LogError("NO ZONE SELECTED, ERROR IN EVENT TYPE");
        return LayerMask.NameToLayer("");
    }
}


