using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    int indexLastScene = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadSceneWithName(string nameScene)
    {
        indexLastScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nameScene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(indexLastScene).name);
    }
}
