using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public void LoadLevel(int level){
		Application.LoadLevel (level);
	}

	public void RetryLevel(){
		Application.LoadLevel (Application.loadedLevel);
	}
}
