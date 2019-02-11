using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RobotGameManager : MonoBehaviour
{    
    public static RobotGameManager instance = null; //Instance of Manager

    public DogPetter toaster;

    private int score;

    public Text ScoreText;
    public Text StrikeText;

    private int strikes;
    //Called Before Start
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        strikes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (strikes >= 3)
        {
            gameOver();
        }
        
        if (Input.GetKeyDown("r"))
        {
            reloadScene();;
        }
    }

    public void DogDestroy(int id, bool isRaccoon)
    {
        if (toaster.dogsIhavePet.ContainsKey(id))
        {
            if (!toaster.dogsIhavePet[id])
            {
                if (isRaccoon)
                {
                    score += 100;
                    ScoreText.text = "Score: " + score;
                }
                else
                {
                    strikes++;
                    StrikeText.text += "X";
                }
            }   
        }
        else if (isRaccoon)
        {
            score += 100;
            ScoreText.text = "Score: " + score;
        }
        else
        {
            strikes++;
            StrikeText.text += "X";
        }
        toaster.dogsIhavePet.Remove(id);
    }

    public void DogPetComplete(bool isRaccoon)
    {
        if (isRaccoon)
        {
            strikes++;
            StrikeText.text += "X";
        }
        else
        {
            score += 100;
            ScoreText.text = "Score: " + score;
        }
    }

    private void gameOver()
    {
        reloadScene();
    }

    private void reloadScene()
    {
        SceneManager.LoadScene("Robots");
    }
}
