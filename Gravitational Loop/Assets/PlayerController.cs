﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;

    bool carpa;


    float currentTime;

    bool invincible;

    public GameObject fireShield;

    [SerializeField]
    AudioClip win, death, idestory, destory, bounce;


    public int currentObstacleNumber;
    public int totalObstacleNumber;


    public Image CircleTimer;
    public GameObject CircleTimerOBJ;
    public GameObject gameOverUI;
    public GameObject finishUI;

    public enum PlayerState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public PlayerState playerstate = PlayerState.Prepare;

    void Start()
    {

        totalObstacleNumber = FindObjectsOfType<ObstacleController>().Length;

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentObstacleNumber = 0;

    }


    void Update()
    {
        if (playerstate == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                impact = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                impact = false;
            }


            if (invincible)
            {
                currentTime -= Time.deltaTime * .35f;
                if (!fireShield.activeInHierarchy)
                {
                    fireShield.SetActive(true);
                }
            }
            else
            {
                if (fireShield.activeInHierarchy)
                {
                    fireShield.SetActive(false);
                }

                if (impact)
                {
                    currentTime += Time.deltaTime * 0.8f;
                }
                else
                {
                    currentTime -= Time.deltaTime * 0.5f;
                }
            }


            if (currentTime >= 0.15f || InvictableSlider.color == Color.red)
            {
                InvictableOBJ.SetActive(true);
            }
            else
            {
                InvictableOBJ.SetActive(false);
            }



            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
                Debug.Log("invincible");
                CircleTimer.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                Debug.Log("-----------");
                CircleTimer.color = Color.white;
            }

            if (CircleTimerOBJ.activeInHierarchy)
            {
                CircleTimer.fillAmount = currentTime / 1;
            }





        }



        if (playerstate == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<LevelSpawner>().NextLevel();
            }
        }





    }


    public void shatterObstacles()
    {


        if (invincible)
        {

            ScoreManager.Intance.addScore(1);
        }
        else
        {
            ScoreManager.Intance.addScore(3);
        }

    }



    private void FixedUpdate()
    {
        

        if (playerstate == PlayerState.Playing)
        {
            if (impact)
            {

                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);


            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!impact)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
        else
        {
            if (invincible)
            {
                if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "plane")
                {
                
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles();
                    SoundManager.instance.playSoundFX(idestory, 0.7f);
                    currentObstacleNumber();
                }

            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                 
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles();
                    SoundManager.instance.playSoundFX(destory, 0.7f);
                    currentObstacleNumber();

                }
                else if (collision.gameObject.tag == "plane")
                {
                    Debug.Log("GameOver");
                    gameOverUI.SetActive(true);
                    playerstate = PlayerState.Finish;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    ScoreManager.intance.ResetScore();
                    SoundManager.instance.playSoundFX(death, 0.7f);



                }
            }





        }


        FindObjectOfType<GameUIController>().LevelSliderFill(currentObstacleNumber / (float)totalObstacleNumber);


        if (collision.gameObject.tag == "Finish" && playerstate == PlayerState.Playing)
        {
            playerstate = PlayerState.Finish;
            SoundManager.instance.playSoundFX(win, 0.7f);
            finishUI.SetActive(true);
            finishUI.transform.GetChild(0).GetComponent<Text>().text = "Level" + PlayerPrefs.GetInt("Level", 1);
        }


    }


    private void OnCollisionStay(Collision collision)
    {
        if (!impact || collision.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.instance.playSoundFX(bounce, 0.7f);


        }
    }



}