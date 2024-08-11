using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangItem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] HealEquip;

    [SerializeField]
    private GameObject[] PowerEquip;

    [SerializeField]
    private GameObject[] MagicEquip;

    [SerializeField]
    private GameObject[] WithOutEquip;

    [SerializeField]
    private Material[] HealMaterial;

    [SerializeField]
    private Material[] MagicMaterial;

    [SerializeField]
    private Material[] PowerMaterial;

    void Awake() 
    {
        NoEquip();
    }

    public void SetItem(string itemName) 
    {
        ClearEquip();
        //NoEquip();
        if (itemName.Contains("Heal")) 
        {
            if (itemName.Contains("Lv1"))
            {
                for (int i = 0; i < HealEquip.Length; i++)
                {
                    HealEquip[i].SetActive(true);
                    HealEquip[i].GetComponent<SkinnedMeshRenderer>().material = HealMaterial[0];
                }
            }
            else if (itemName.Contains("Lv2")) 
            {
                for (int i = 0; i < HealEquip.Length; i++)
                {
                    HealEquip[i].SetActive(true);
                    HealEquip[i].GetComponent<SkinnedMeshRenderer>().material = HealMaterial[1];
                }
            }
            else if (itemName.Contains("Lv3"))
            {
                for (int i = 0; i < HealEquip.Length; i++)
                {
                    HealEquip[i].SetActive(true);
                    HealEquip[i].GetComponent<SkinnedMeshRenderer>().material = HealMaterial[2];
                }
            }
            else if (itemName.Contains("Lv4"))
            {
                for (int i = 0; i < HealEquip.Length; i++)
                {
                    HealEquip[i].SetActive(true);
                    HealEquip[i].GetComponent<SkinnedMeshRenderer>().material = HealMaterial[3];
                }
            }
            else if (itemName.Contains("Lv5"))
            {
                for (int i = 0; i < HealEquip.Length; i++)
                {
                    HealEquip[i].SetActive(true);
                    HealEquip[i].GetComponent<SkinnedMeshRenderer>().material = HealMaterial[4];
                }
            }
        }
        else if (itemName.Contains("Warrior"))
        {
            if (itemName.Contains("Lv1"))
            {
                for (int i = 0; i < PowerEquip.Length; i++)
                {
                    PowerEquip[i].SetActive(true);
                    PowerEquip[i].GetComponent<SkinnedMeshRenderer>().material = PowerMaterial[0];
                }
            }
            else if (itemName.Contains("Lv2"))
            {
                for (int i = 0; i < PowerEquip.Length; i++)
                {
                    PowerEquip[i].SetActive(true);
                    PowerEquip[i].GetComponent<SkinnedMeshRenderer>().material = PowerMaterial[1];
                }
            }
            else if (itemName.Contains("Lv3"))
            {
                for (int i = 0; i < PowerEquip.Length; i++)
                {
                    PowerEquip[i].SetActive(true);
                    PowerEquip[i].GetComponent<SkinnedMeshRenderer>().material = PowerMaterial[2];
                }
            }
            else if (itemName.Contains("Lv4"))
            {
                for (int i = 0; i < PowerEquip.Length; i++)
                {
                    PowerEquip[i].SetActive(true);
                    PowerEquip[i].GetComponent<SkinnedMeshRenderer>().material = PowerMaterial[3];
                }
            }
            else if (itemName.Contains("Lv5"))
            {
                for (int i = 0; i < PowerEquip.Length; i++)
                {
                    PowerEquip[i].SetActive(true);
                    PowerEquip[i].GetComponent<SkinnedMeshRenderer>().material = PowerMaterial[4];
                }
            }
        }
        else if (itemName.Contains("Wizard"))
        {
            if (itemName.Contains("Lv1"))
            {
                for (int i = 0; i < MagicEquip.Length; i++)
                {
                    MagicEquip[i].SetActive(true);
                    MagicEquip[i].GetComponent<SkinnedMeshRenderer>().material = MagicMaterial[0];
                }
            }
            else if (itemName.Contains("Lv2"))
            {
                for (int i = 0; i < MagicEquip.Length; i++)
                {
                    MagicEquip[i].SetActive(true);
                    MagicEquip[i].GetComponent<SkinnedMeshRenderer>().material = MagicMaterial[1];
                }
            }
            else if (itemName.Contains("Lv3"))
            {
                for (int i = 0; i < MagicEquip.Length; i++)
                {
                    MagicEquip[i].SetActive(true);
                    MagicEquip[i].GetComponent<SkinnedMeshRenderer>().material = MagicMaterial[2];
                }
            }
            else if (itemName.Contains("Lv4"))
            {
                for (int i = 0; i < MagicEquip.Length; i++)
                {
                    MagicEquip[i].SetActive(true);
                    MagicEquip[i].GetComponent<SkinnedMeshRenderer>().material = MagicMaterial[3];
                }
            }
            else if (itemName.Contains("Lv5"))
            {
                for (int i = 0; i < MagicEquip.Length; i++)
                {
                    MagicEquip[i].SetActive(true);
                    MagicEquip[i].GetComponent<SkinnedMeshRenderer>().material = MagicMaterial[4];
                }
            }
        }
    }

    private void ClearEquip() 
    {
        for (int i = 0; i < PowerEquip.Length; i++) 
        {
            HealEquip[i].SetActive(false);
            PowerEquip[i].SetActive(false);
            MagicEquip[i].SetActive(false);
        }

        for (int i = 0; i < WithOutEquip.Length; i++) 
        {
            WithOutEquip[i].SetActive(false);
        }
    }

    public void NoEquip() 
    {
        ClearEquip();
        for (int i = 0; i < WithOutEquip.Length; i++)
        {
            WithOutEquip[i].SetActive(true);
        }
    }
}
