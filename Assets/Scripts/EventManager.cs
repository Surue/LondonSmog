﻿using System.Collections;
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

    static int numberOfEvent = 5;

    static List<Evenement> evenements = new List<Evenement>();

    static Evenement currentEvenement;

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

        public Evenement(EventType eventType, GameObject spawn, GameObject coll)
        {
            type = eventType;
            spawnPoint = spawn;
            mainObject = coll;
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
    }

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
        spawnsPointForEvent.InsertRange(0, tmpSpawnPoint);

        GenerateAllEventForTheDay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateAllEventForTheDay()
    {
        for (int i = 0; i < numberOfEvent; i++)
        {
            //Random event
            EventType tmpEventType = (EventType)Random.Range(0, (float)EventType.LENGTH);

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

            //Create new evenement
            GameObject collider = null;
            switch (tmpEventType)
            {
                case EventType.BOAT:
                    collider = Instantiate(prefabBoatEvent, tmpSpawn.transform.position, tmpSpawn.transform.rotation);
                    break;

                case EventType.CAR_FIRE:
                    collider = Instantiate(prefabCarFireEvent, tmpSpawn.transform.position, tmpSpawn.transform.rotation);
                    break;

                case EventType.LOST:
                    collider = Instantiate(prefabLostEvent, tmpSpawn.transform.position, tmpSpawn.transform.rotation);
                    break;

                case EventType.THIEF:
                    collider = Instantiate(prefabThiefEvent, tmpSpawn.transform.position, tmpSpawn.transform.rotation);
                    break;

                case EventType.WOUNDED:
                    collider = Instantiate(prefabWoundedEvent, tmpSpawn.transform.position, tmpSpawn.transform.rotation);
                    break;
            }

            evenements.Add(new Evenement(tmpEventType,tmpSpawn, collider));
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
        foreach (Evenement evenement in evenements)
        {
            if (evenement.GetMainObject().GetInstanceID() == currentEventZone.GetInstanceID())
            {
                currentEvenement = evenement;
                break;
            }
        }
    }

    static public void EndEvent()
    {
        evenements.Remove(currentEvenement);
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


