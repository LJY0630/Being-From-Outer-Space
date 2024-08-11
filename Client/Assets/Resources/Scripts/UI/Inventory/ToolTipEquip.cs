using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipEquip : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI seeTxt;
    public TextMeshProUGUI buyTxt;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI manaTxt;
    public TextMeshProUGUI defendTxt;
    public TextMeshProUGUI weaponTxt;

    public Camera camera;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetToolTip(string name, int sell, int buy, float health, int mana, int defend, int power, int magic, int heal) 
    {
        nameTxt.text = name;
        seeTxt.text = "�Ǹ� ���� : " + sell;
        buyTxt.text = "���� ���� : " + buy;
        healthTxt.text = "ü�� : " + health;
        manaTxt.text = "���� : " + mana;
        defendTxt.text = "���� : " + defend;

        if (power != 0)
        {
            weaponTxt.text = "�� : " + power;
        }
        else if (magic != 0)
        {
            weaponTxt.text = "���� : " + magic;
        }
        else if (heal != 0) 
        {
            weaponTxt.text = "ġ���� : " + heal;
        }
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            gameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("Block");
        }
    }

}
