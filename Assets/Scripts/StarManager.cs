using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public int counter = 0;
    public GameObject[] stars = new GameObject [20];
    public GameObject[] lines = new GameObject [19];
    public int max;

    public void Update()
    {
        if (stars[counter].isStatic && stars[counter + 1].isStatic)
        {
            lines[counter].SetActive(true);
            stars[counter+1].SetActive(false);
            counter++;
        }

        for (int check = counter+2; check < stars.Length-1; check++)
        {
            if (stars[counter].isStatic && stars[check].isStatic && !stars[counter+1].isStatic)
            {
                //play wrong noise
                stars[check].isStatic = false;
            }
        }

        if (stars[max].isStatic)
        {
            //load ui informational screen with next level button
        }
    }
}
