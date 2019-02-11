using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DG.Tweening;
using UnityEngine;

public class DogFactory : MonoBehaviour
{
    public Transform StartPoint, EndPoint;

    private List<GameObject> dogs = new List<GameObject>(); //The Dogs in the scene

    public float DogFrequency = 1f; //The rate of how many dogs to make per 10 seconds;

    public float BeltSpeed = 0.1f; //How fast the dogs move on the belt

    private float timer; //Timer
    private float dogCreateDelay;
    
    private float roundsComplete = 0;
    
    void Start()
    {
        timer = 10f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        dogCreateDelay -= Time.deltaTime;

        if (dogs.Count < (int)DogFrequency && dogCreateDelay <= 0)
        {
            CreateDog();
            dogCreateDelay = 10 / DogFrequency;
        }

        if (timer <= 0f)
            timer = 10f;
        GameObject offScreenDog = null;
        
        foreach (GameObject dog in dogs)
        {
            dog.transform.Translate(-1 * BeltSpeed, 0f, 0f, Space.World);
            if (dog.transform.position.x <= EndPoint.position.x)
            {
                RobotGameManager.instance.DogDestroy(dog.gameObject.GetInstanceID(), dog.CompareTag("Raccoon"));
                offScreenDog = dog;
                DogFrequency += 0.5f;
            }
            //Debug.Log(dog.GetInstanceID());
        }

        if (offScreenDog != null)
        {
            Destroy(offScreenDog);
            dogs.Remove(offScreenDog);
            if (dogs.Count == 0)
            {
                dogCreateDelay = 0;
            }
        }
    }

    private void CreateDog()
    {
        int dogType = Random.Range(0, 4);
        switch (dogType)
        {
            case 0:
                dogs.Add((GameObject)Instantiate(Resources.Load("Prefabs/Beagle")));
                break;
            case 1:
                dogs.Add((GameObject)Instantiate(Resources.Load("Prefabs/Lab")));
                break;
            case 2:
                dogs.Add((GameObject)Instantiate(Resources.Load("Prefabs/Mutt")));
                break;
            case 3:
                dogs.Add((GameObject)Instantiate(Resources.Load("Prefabs/Raccoon")));
                break;
        }

        dogs[dogs.Count - 1].transform.position = StartPoint.position;
    }
    
    
    
}
