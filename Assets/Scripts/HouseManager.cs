using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [SerializeField]
    float offsetForSortingLayer;

    SpriteRenderer facade;
    Player player;

	void Start ()
    {
        facade = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindObjectOfType<Player>();
	}
	
	void Update ()
    {
	    if(transform.position.y - player.transform.position.y < offsetForSortingLayer && transform.position.y - player.transform.position.y > 0)
        {
            facade.sortingOrder = 10;
        }
        else{
            facade.sortingOrder = 0;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            facade.color = new Color(1f, 1f, 1f, 0.2f);
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
