using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChest : MonoBehaviour
{
    public GameObject[] Chest;

    void Start()
    {
        for (int i = 0; i < Chest.Length; i++) 
        {
            Chest[i].SetActive(false);
        }
    }

    public void SetChestActive(bool check) 
    {
        for (int i = 0; i < Chest.Length; i++)
        {
            Chest[i].SetActive(check);
        }
    }

    public void SetMarker(bool check) 
    {
        for (int i = 0; i < Chest.Length; i++)
        {
            Chest[i].transform.GetChild(1).GetChild(0).gameObject.SetActive(check);
            Chest[i].transform.GetChild(1).GetChild(1).gameObject.SetActive(check);
        }
    }
}
