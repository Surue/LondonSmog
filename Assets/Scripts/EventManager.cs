using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager:MonoBehaviour
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

    List<GameObject> spawnsPointForEvent = new List<GameObject>(); //List of spawn point for all event

    int numberOfEvent = 5;

    List<Evenement> evenements = new List<Evenement>();

    Evenement currentEvenement;

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

<<<<<<< HEAD
        public Evenement(EventType eventType, GameObject spawn)
=======
        public Evenement(EventType eventType,GameObject spawn)
>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
        {
            type = eventType;
            spawnPoint = spawn;
        }

        public GameObject GetSpawnPoint()
        {
            return spawnPoint;
        }

        public EventType GetEventType()
        {
            return type;
        }
    }

    // Use this for initialization
<<<<<<< HEAD
    void Start ()
=======
    void Start()
>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
    {
        //Get all spawn point 
        GameObject[] tmpSpawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnsPointForEvent.InsertRange(0,tmpSpawnPoint);

        GenerateAllEventForTheDay();
<<<<<<< HEAD
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void EnterEvent(GameObject eventGameObject)
=======
    }

    // Update is called once per frame
    void Update()
>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
    {

    }

<<<<<<< HEAD
    void GenerateAllEventForTheDay() {
        for(int i = 0; i < numberOfEvent; i++)
=======
    void GenerateAllEventForTheDay()
    {
        for(int i = 0;i < numberOfEvent;i++)
>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
        {
            //Random event
            EventType tmpEventType = (EventType)Random.Range(0, (float)EventType.LENGTH);

            //Random free Position
            GameObject tmpSpawn = null;
            bool found = false;
            while(!found)
            {
                tmpSpawn = spawnsPointForEvent[Random.Range(0,spawnsPointForEvent.Count)];

                if(CheckIfPositionIsFree(tmpSpawn))
                {
                    found = true;
                }
            }

            //Create new evenement
<<<<<<< HEAD
            evenements.Add(new Evenement(tmpEventType, tmpSpawn));
            Debug.Log(tmpSpawn+" "+tmpEventType);
=======
            evenements.Add(new Evenement(tmpEventType,tmpSpawn));
            Debug.Log(tmpSpawn + " " + tmpEventType);
>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
            switch(tmpEventType)
            {
                case EventType.BOAT:
                    Instantiate(prefabBoatEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                    break;

                case EventType.CAR_FIRE:
                    Instantiate(prefabCarFireEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                    break;

                case EventType.LOST:
                    Instantiate(prefabLostEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                    break;

                case EventType.THIEF:
                    Instantiate(prefabThiefEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                    break;

                case EventType.WOUNDED:
                    Instantiate(prefabWoundedEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                    break;
            }
        }
    }

    bool CheckIfPositionIsFree(GameObject spawn)
    {
<<<<<<< HEAD
=======

>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
        foreach(Evenement evenement in evenements)
        {
            if(evenement.GetSpawnPoint().GetInstanceID() == spawn.GetInstanceID())
            {
<<<<<<< HEAD
                return false; 
=======
                return false;
>>>>>>> fb5daa286937b3dbfcf2aef538beb43dd9013e71
            }
        }
        return true;
    }

    public void LaunchEvent(GameObject currentEventZone)
    {
        foreach(Evenement evenement in evenements)
        {
            if(evenement.GetSpawnPoint().GetInstanceID() == currentEventZone.GetInstanceID())
            {
                currentEvenement = evenement;
                break;
            }
        }
    }

    LayerMask ZoneToGo() {
        switch(currentEvenement.GetEventType()) {
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
