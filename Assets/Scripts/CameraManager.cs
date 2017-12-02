using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject player;
    private Vector3 offset;
    float minPosX = -10.58f;
    float maxPosX = 10.5f;
    float minPosY = -1.7f;
    float maxPosY = 28.6f;

	void Start ()
    {
        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position; 
	}

    void LateUpdate()
    {
        Vector3 CameraPosition = player.transform.position + offset;
        if (CameraPosition.x < minPosX)
        {
            CameraPosition.x = minPosX;
        }
        if (CameraPosition.x > maxPosX)
        {
            CameraPosition.x = maxPosX;
        }
        if (CameraPosition.y < minPosY)
        {
            CameraPosition.y = minPosY;
        }
        if (CameraPosition.y > maxPosY)
        {
            CameraPosition.y = maxPosY;
        }
        transform.position = CameraPosition;
    }
}
