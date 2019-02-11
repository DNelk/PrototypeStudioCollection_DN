using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
	public float speed = 2.0f;
	private float roundStartSpeed;
	
	public float fatness = 1.0f;
	private float roundStartFatness;
	private Rigidbody rb;

	private int stageNum;

	public string name;
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		stageNum = 1;
		roundStartFatness = fatness;
		roundStartSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		float rot = transform.rotation.eulerAngles.y;
		if (Input.GetKey ("w"))
		{
			pos += transform.right * Time.deltaTime * speed; //Bear go in direction we're facing
		}
		if (Input.GetKey ("s")) {
			pos.z -= speed/2 * Time.deltaTime;
		}
		if (Input.GetKey ("d")) {
			pos.x += speed/2 * Time.deltaTime;
		}
		if (Input.GetKey ("a")) {
			pos.x -= speed/2 * Time.deltaTime;
		}


		transform.position = pos; //Set position
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Food")) //Can we eat this
		{
			Debug.Log("HAUMPH");
			float nutritionalVal = other.gameObject.GetComponent<EdibleObject>().getNutritionalValue();
			getFat(nutritionalVal);
			Destroy(other.gameObject); //Eat the food
			
		}
	}

	public int StageNum
	{
		get { return stageNum; }
		set 
		{ 
			stageNum = value;
			roundStartFatness = fatness;
			roundStartSpeed = speed;
		}
	}

	public void ResetFatness()
	{
		fatness = roundStartFatness;
		getFat(0);
		speed = roundStartSpeed;
	}

	private void getFat(float nutritionalVal)
	{
		fatness += nutritionalVal * 0.05f; //Get large
		Vector3 newScale = new Vector3(fatness,fatness,fatness); //Scale
		transform.localScale = newScale;
		speed -= fatness * 20; //Get Slow
		speed = Mathf.Clamp(speed, 10, 1000);
	}
}
