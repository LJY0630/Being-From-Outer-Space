using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUICall : MonoBehaviour
{
    [SerializeField]
    private ShopUI shop;

    private bool canShopOpen = false;

    private void Update()
    {
        if (canShopOpen && Input.GetKeyDown(KeyCode.F))
        {
            if(!shop.isShopOn)
            {
                shop.SetShop(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Shop")
        {
            if (!shop.isShopOn)
            {
                canShopOpen = true;
                shop.SetText(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Shop")
        {
            canShopOpen = false;
            shop.QuitButton();
        }
    }
}
