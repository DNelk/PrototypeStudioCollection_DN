using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{	
	/*Stole this from unity help*/

	public GameObject player; //Player

	private Vector3 offset; //Our Distance and position relative to the player
	// Use this for initialization
	void Start ()
	{
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	private void LateUpdate()
	{
		transform.position = player.transform.position + offset;
	}
}
