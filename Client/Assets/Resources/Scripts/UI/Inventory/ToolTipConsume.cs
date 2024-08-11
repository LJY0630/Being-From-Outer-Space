using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipConsume : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI seeTxt;
    public TextMeshProUGUI buyTxt;
    public TextMeshProUGUI comsumeTxt;

    public Camera camera;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetToolTip(string name, int sell, int buy, int healing)
    {
        nameTxt.text = name;
        seeTxt.text = "�Ǹ� ���� : " + sell;
        buyTxt.text = "���� ���� : " + buy;

        if (name.Contains("Health"))
        {
            comsumeTxt.text = "ü�� ġ���� : " + healing;
        }
        else 
        {
            comsumeTxt.text = "ü�� ġ���� : " + healing;
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
