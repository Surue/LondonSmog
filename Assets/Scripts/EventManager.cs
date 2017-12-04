using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    static int numberOfEvent = 2;

    static List<Evenement> evenements = new List<Evenement>();

    static Evenement currentEvenement;

    public static GameObject player;

    GameManager gameManager;

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

        currentEvenement = null;

        gameManager = GameObject.FindObjectOfType<GameManager>();
        Debug.Log(gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEvenement != null)
        {
            currentEvenement.SetVelocity();
        }

        if(evenements.Count == 0)
        {
            Debug.Log("Succes day");
            gameManager.SuccesDay();
        }
    }

    void GenerateAllEventForTheDay()
    {
        for (int i = 0; i < numberOfEvent; i++)
        {
            //Random event
            Evenement.EventType tmpEventType;

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
                tmpEventType = Evenement.EventType.BOAT;
                tmpMainObject = Instantiate(prefabBoatEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
            }
            else
            {
                tmpEventType = (Evenement.EventType)Random.Range(0,(float)Evenement.EventType.BOAT);

                //Create new evenement
                switch(tmpEventType)
                {
                    case Evenement.EventType.CAR_FIRE:
                        tmpMainObject = Instantiate(prefabCarFireEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                        break;

                    case Evenement.EventType.LOST:
                        tmpMainObject = Instantiate(prefabLostEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
                        break;

                    case Evenement.EventType.THIEF:
                        tmpMainObject = Instantiate(prefabThiefEvent,tmpSpawn.transform.position,tmpSpawn.transform.rotation);
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
        evenements.Remove(currentEvenement);
        Destroy(currentEvenement.GetNPC());
        Destroy(currentEvenement.GetMainObject());
        currentEvenement = null;
    }

    static public LayerMask ZoneToGo()
    {
        Debug.Log(currentEvenement.GetEventType());
        switch (currentEvenement.GetEventType())
        {
            case Evenement.EventType.BOAT:
            case Evenement.EventType.CAR_FIRE:
            case Evenement.EventType.WOUNDED:
                return LayerMask.NameToLayer("Hospital");

            case Evenement.EventType.LOST:
                return LayerMask.NameToLayer("House");

            case Evenement.EventType.THIEF:
                return LayerMask.NameToLayer("PoliceStation");
        }

        Debug.LogError("NO ZONE SELECTED, ERROR IN EVENT TYPE");
        return LayerMask.NameToLayer("");
    }
}


