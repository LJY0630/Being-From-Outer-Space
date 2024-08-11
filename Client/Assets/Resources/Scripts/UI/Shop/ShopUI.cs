using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider player;

    [SerializeField]
    private GameObject Panel;

    [SerializeField]
    private GameObject CallText;

    [SerializeField]
    private GameObject Shop;

    [SerializeField]
    private InventotyUI inven;

    public bool isShopOn = false;

    void Start()
    {
        Panel.SetActive(false);
        isShopOn = false;
    }

    // Update is called once per frame
    public void QuitButton()
    {
        isShopOn = false;
        inven.ShopClose();
        inven.RedrawSlotUI();
        Panel.SetActive(false);
    }

    public void SetShop(bool active)
    {
        Shop.SetActive(active);
        inven.ShopOpen(active);
        CallText.SetActive(false);
        isShopOn = active;
    }

    public void SetText(bool active) 
    {
        Panel.SetActive(active);
        Shop.SetActive(false);
        CallText.SetActive(active);
    }

}
