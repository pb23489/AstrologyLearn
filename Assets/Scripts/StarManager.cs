﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    private int counter = 0;
    private int max;
    public GameObject[] stars = new GameObject [0];
    public GameObject[] lines = new GameObject [0];
    public GameObject endScreen;
    public GameObject continueButton;

    public void Update()
    {
       max = stars.Length-1;
        if (counter == max)
        {
            endScreen.SetActive(true);
            if (continueButton.CompareTag("Clicked"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        else if (stars[counter].CompareTag("Clicked") && stars[counter + 1].CompareTag("Clicked"))
        {
            lines[counter].SetActive(true);
            stars[counter + 1].SetActive(false);
            Instantiate(Resources.Load("Correct SFX"));
            counter++;
        }

        for (int check = counter + 2; check < stars.Length; check++)
        {
            if (stars[counter].CompareTag("Clicked") && stars[check].CompareTag("Clicked") && !stars[counter + 1].CompareTag("Clicked"))
            {
                Instantiate(Resources.Load("Error SFX"));
                stars[check].tag = "Untagged";
            }
        }
    }
}