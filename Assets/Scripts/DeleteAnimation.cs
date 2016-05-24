using UnityEngine;
using System.Collections;

public class DeleteAnimation : MonoBehaviour {
	public float delay = 0f;

	// Use this for initialization
	void Start () {
	
		Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
