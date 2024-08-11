using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    static ItemDataBase s_instance;
    public static ItemDataBase instance { get { return s_instance; } }

    [SerializeField]
    private FieldItem drop;

    public List<Item> itemDB = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        s_instance = this;

        GameObject single = GameObject.Find("ItemDataBase");

        if (single == null)
        {
            single = new GameObject { name = "ItemDataBase" };
            single.AddComponent<ItemDataBase>();
        }

        DontDestroyOnLoad(single);
        s_instance = single.GetComponent<ItemDataBase>();

        for(int i=0;i<itemDB.Count;i++)
        {
            itemDB[i].itemcode = i;
        }
    }

    public Item FindItem(int itemcode)
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemcode == itemDB[i].itemcode)
            {
                Item returnitem = itemDB[i];
                return returnitem;
            }
        }
        return null;
    }

    public string FindItemName(int itemcode)
    {
        Debug.Log("FindItemName 들어옴!");
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemcode == itemDB[i].itemcode)
            {
                string returnitemname = itemDB[i].itemName;
                Debug.Log($"아이템찾음! 이름 : {returnitemname}");
                return returnitemname;
            }
        }
        return null;
    }
    // Update is called once per frame
    public void GetItem(Vector3 enemyPos)
    {
        FieldItem dropping = Instantiate(drop, enemyPos + new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2)), Quaternion.identity);
        dropping.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0, 2)]);
    }
}
