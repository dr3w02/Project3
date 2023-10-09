    using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;
using Unity.Collections;
using UnityEditor.SearchService;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Mole> moles;

    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private List<TextMeshProUGUI> scoreText = new List<TextMeshProUGUI>();


    //This determins the amount of time in game change depending on time we want for our minigame
    private float startingTime = 60f;

    //this tracks how much time is remaining 
    private float timeRemaining;
    //this tracks the amount of moles
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    // this controls how were doing in corilation to the moles left
    private int scoreP1;
    private int scoreP2;
    private bool playing = false;
    public int playerIndex;
    public Mole currentMole;


    //this is how the game starts setting everything up 

    public void StartGame()
    {
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);
        nextButton.SetActive(false);
        //hide all the visable moles
        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }

        //remove any old game stats 
        currentMoles.Clear();
        // START WITH 30 SECONDS 
        timeRemaining = startingTime;
        scoreP1 = 0;
        scoreP2 = 0;
        scoreText[0].text = "0";
        scoreText[1].text = "0";
        playing = true;

    }

    public void GameOver(int type)
    {
        if (type == 0)
        {
            outOfTimeText.SetActive(true);

        }
        else
        {
            bombText.SetActive(true);
        }

        //Hide all the moles 
        foreach (Mole mole in moles)
        {
            mole.StopGame();

        }
        playing = false;
    }
    //ending the game no more moles left to click


    void Update()
    {
        if (playing)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
                nextButton.SetActive(true);
            }

            //int score = scoreP1 > scoreP2 ? scoreP1 : scoreP2; fancy way
            int score = 0;
            if (scoreP1 > scoreP2)
            {
                score = scoreP1;
            }
            else
            {
                score = scoreP2;
            }

            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";

            //15 min in video he explains this part, this is bascially saying once 10 moles have been hit the level diffculty will increase
            //once 10 moles are hit odwn more molews will apprear
            if (currentMoles.Count <= (score / 10))
            {
                //chose a random mole
                int index = Random.Range(0, moles.Count);

                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
                else
                {
                    moles[index].Activate(score / 10);
                }

                currentMole = moles[index];
            }

            if(currentMole == null)
            {
                //chose a random mole
                int index = Random.Range(0, moles.Count);

                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
                else
                {
                    moles[index].Activate(score / 10);
                }

                currentMole = moles[index];
            }
        }

    }
    public void AddScore(int moleIndex, int playerIndex) //for when we've sucessfully clicked the mole
    {
        if(playerIndex == 0)
        {
            //add and update score
            scoreP1 += 1;
            scoreText[playerIndex].text = $"{scoreP1}";
        }
        else
        {
            //add and update score
            scoreP2 += 1;
            scoreText[playerIndex].text = $"{scoreP2}";
        }


        currentMoles.Remove(moles[moleIndex]);
        currentMole = null;

    }

    public void NextMole()
    {
        //chose a random mole
        int index = Random.Range(0, moles.Count);
        int score = 0;
        if (scoreP1 > scoreP2)
        {
            score = scoreP1;
        }
        else
        {
            score = scoreP2;
        }
        if (!currentMoles.Contains(moles[index]))
        {
            currentMoles.Add(moles[index]);
            moles[index].Activate(score / 10);
        }
        else
        {
            moles[index].Activate(score / 10);
        }

        currentMole = moles[index];
    }

}

