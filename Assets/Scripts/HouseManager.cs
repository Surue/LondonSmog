using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
   
    SpriteRenderer facade;

	void Start ()
    {
        facade = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("WORK");
            facade.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            facade.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
