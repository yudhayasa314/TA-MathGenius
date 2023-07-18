using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStar : MonoBehaviour
{
    public GameObject[] stars;

    public void SetStar(int amount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(false);
        }

        switch(amount)
        {
            case 0:
                stars[0].gameObject.SetActive(true);
                break;

            case 1:
                stars[1].gameObject.SetActive(true);
                stars[2].gameObject.SetActive(true);
                break;

            case 2:
                stars[1].gameObject.SetActive(true);
                break;

            case 3:
                //stars[0].gameObject.SetActive(true);
                break;
        }
    }
}
