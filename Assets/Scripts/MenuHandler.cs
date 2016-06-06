using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	// Use this for initialization
	public void LoadLevel(int level){
		SceneManager.LoadScene(level);

	}

	public void QuitGame(){
		Application.Quit();
	
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
