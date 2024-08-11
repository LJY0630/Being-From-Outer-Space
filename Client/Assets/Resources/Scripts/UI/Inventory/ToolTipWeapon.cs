using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipWeapon : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI seeTxt;
    public TextMeshProUGUI buyTxt;
    public TextMeshProUGUI weaponTxt;

    public Camera camera;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetToolTip(string name, int sell, int buy, int power, int magic, int heal)
    {
        nameTxt.text = name;
        seeTxt.text = "판매 가격 : " + sell;
        buyTxt.text = "구매 가격 : " + buy;

        if (power != 0)
        {
            weaponTxt.text = "힘 : " + power;
        }
        else if (magic != 0)
        {
            weaponTxt.text = "마력 : " + magic;
        }
        else if (heal != 0)
        {
            weaponTxt.text = "치유력 : " + magic;
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
            Debug.Log("Block");
        }
    }

}
