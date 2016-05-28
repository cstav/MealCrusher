using UnityEngine;
using System.Collections;

public class CigaretteGrow : MonoBehaviour {

	Animator anim;
	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator> ();
		Stop ();
		Debug.Log ("length: " +  GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
	}
	
	// Update is called once per frame
	void Update () {


	}

	void Stop(){
		anim.SetFloat ("scale", 1);
	}
}
