using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public int counter = 0;
    public bool star1 = false;
    public bool star2 = false;
    public GameObject[] stars = new GameObject [20];
    public GameObject[] lines = new GameObject [19];
    public GameObject test;

    public void Update()
    {
        

        if(stars[counter].isStatic && stars[counter+1].isStatic)
        {
            lines[counter].SetActive(true);
            counter++;
            test.tag.Equals("Done");
        }
    }
}
