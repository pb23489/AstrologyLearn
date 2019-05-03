using System.Collections;
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

    public void Update()
    {
       max = stars.Length-1;
        if (counter == max)
        {
            //display UI Screen
        }
        else if (stars[counter].isStatic && stars[counter + 1].isStatic)
        {
            lines[counter].SetActive(true);
            stars[counter + 1].SetActive(false);
            counter++;
        }

        for (int check = counter + 2; check < stars.Length; check++)
        {
            if (stars[counter].isStatic && stars[check].isStatic && !stars[counter + 1].isStatic)
            {
                //play wrong noise
                stars[check].isStatic = false;
            }
        }
    }
}