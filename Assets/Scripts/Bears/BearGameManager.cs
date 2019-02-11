using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using DG.Tweening;

public class BearGameManager : MonoBehaviour
{
	public static BearGameManager instance = null; //Instance of Manager

	public BearController player1, player2; //Our Players
	private float timer;

	private BearController activePlayer; //Who is the active player

	private Text timerText; //GUI Timer
	public Text winText;

	private GameObject cam; //Our camera
	private Vector3 initialCamPos; //Camera starting position

	private Vector3 startingPlayerPos;

	private GameObject[] foods;

	public Camera EndGameCamera;

	private bool gameOver;
	//Called Before Start
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		//DontDestroyOnLoad(gameObject);
	}
	
	// Use this for initialization
	void Start ()
	{
		cam = GameObject.FindWithTag("MainCamera");
		initialCamPos = cam.transform.position;
		activePlayer = player1;
		startingPlayerPos = activePlayer.transform.position;
		ResetTimer();
		PopulateWithFood();
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Decrease Timer
		if(timerText == null)
			ResetTimer();
		timer -= Time.deltaTime;
		timerText.text = Mathf.RoundToInt(timer).ToString();
		if(timer <= 10)
			timerText.color = Color.red;
		if (timer <= 0 && !gameOver)
			EndLevel(true);
		
		if (Input.GetKeyDown("r"))
		{
			SceneManager.LoadScene("bears");
		}
		
	}

	public void EndLevel(bool timeout = false)
	{
		if (activePlayer.name == player2.name && activePlayer.StageNum == 3)
		{
			EndGame();
			return;
		}
		if (timeout)
			activePlayer.ResetFatness();
		//SceneManager.LoadScene("Bears");
		activePlayer.StageNum = activePlayer.StageNum + 1; //Player advances stage
		
		//Get the active player and reset them
		if (activePlayer.name == player1.name)
		{
			player1 = activePlayer;
			player1.gameObject.SetActive(false);
			player2.gameObject.SetActive(true);
			activePlayer = player2;
		}
		else
		{
			player2 = activePlayer;
			player2.gameObject.SetActive(false);
			player1.gameObject.SetActive(true);
			activePlayer = player1;
		}

		cam.transform.position = initialCamPos; //Move Cam
		cam.transform.parent = activePlayer.transform; //Set Camera Parent
		cam.gameObject.GetComponent<TrackPlayer>().player = activePlayer.gameObject;
		activePlayer.transform.position = startingPlayerPos;
		ClearFood();
		PopulateWithFood();
		ResetTimer();
	}

	private void ResetTimer()
	{
		if(timerText == null)
			timerText = GameObject.Find("Timer").GetComponent<Text>();
		if (activePlayer.StageNum == 0)
			activePlayer.StageNum = 1;
		switch (activePlayer.StageNum)
		{
				case 1:
					timer = 30.0f;
					break;
				case 2:
					timer = 20.0f;
					break;
				case 3:
					timer = 10.0f;
					break;
		}
		timerText.color = Color.yellow;
	}

	private void PopulateWithFood()
	{
		if (foods == null)
		{
			foods = new GameObject[20];
		}

		int troutOrBerry;
		float troutY = 244.3f;
		float berryY = 245.26f;
		float randX, randZ;
		for (int i = 0; i < foods.Length; i++)
		{
			troutOrBerry = Random.Range(0,2);
			randX = Random.Range(-60.3f, 27.3f);
			randZ = Random.Range(2484.2f, 3420.2f);
			switch (troutOrBerry)
			{
				case 0:
					foods[i] = (GameObject)Instantiate(Resources.Load("Prefabs/Trout"));
					foods[i].transform.position = new Vector3(randX, troutY, randZ);
					break;
				case 1:
					foods[i] = (GameObject)Instantiate(Resources.Load("Prefabs/Berry"));
					foods[i].transform.position = new Vector3(randX,  berryY, randZ);
					break;
				default:
					Debug.Log("huh");
					break;
			}
		}
	}

	private void ClearFood()
	{
		foreach (GameObject food in foods)
		{
			Destroy(food);
		}
	}

	private void EndGame()
	{
		timerText.gameObject.SetActive(false);
		gameOver = true;
		
		EndGameCamera.gameObject.SetActive(true);
		cam.gameObject.SetActive(false);
		player1.gameObject.SetActive(true);
		player1.transform.position = new Vector3(-28.1f, 245f, 3547f);
		player1.transform.rotation = Quaternion.Euler(0f, -267.6f, 0f);
		player2.transform.position = new Vector3(-6f,241.8f, 3545f);
		player2.transform.rotation = Quaternion.Euler(0f, -273f, 0f);
		player1.GetComponent<MouseLook>().enabled = false;
		player2.GetComponent<MouseLook>().enabled = false;
		player1.transform.DOMove(new Vector3(-28f, 245f, 3364f), 1.0f);
		player2.transform.DOMove(new Vector3(-8f, 246f, 3359f), 1.0f).OnComplete(ShowEndGame);
	}

	private void ShowEndGame()
	{
		winText.gameObject.SetActive(true);
		if (player1.fatness > player2.fatness)
			winText.text = "PLAYER ONE IS THE FATTEST BEAR!";
		else
			winText.text = "PLAYER TWO IS THE FATTEST BEAR!";
	}
	
}
