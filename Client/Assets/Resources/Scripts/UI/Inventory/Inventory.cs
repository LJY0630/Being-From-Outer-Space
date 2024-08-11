using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<int> count = new List<int>();

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;
    public C_InvenInfo Inveninfo = new C_InvenInfo();

    int SlotCnt;

    [SerializeField]
    private ItemSound itemSound;

    private void Awake()
    {
        SlotCnt = 28;
    }

    public bool AddItem(Item _item)
    {
        if (items.Count < SlotCnt)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == _item.itemName && _item.itemType == ItemType.Consumables)
                {
                    count[i] += 1;
                    if (onChangeItem != null)
                    {
                        onChangeItem.Invoke();
                    }
                    int index = CheckInvenInfo(_item.itemcode);
                    Inveninfo.itemss[index].cnt = count[i];
                    NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
                    return true;
                }
            }
            items.Add(_item);
            count.Add(1);

            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }

            if (_item.itemType == ItemType.Consumables)
            {
                C_InvenInfo.Items serveritem = new C_InvenInfo.Items();
                serveritem.itemId = _item.itemcode.ToString();
                serveritem.cnt = 1;
                Inveninfo.itemss.Add(serveritem);
            }
            else
            {
                int index = CheckInvenInfo(_item.itemcode);
                if (index != -1)
                {
                    int count = 0;

                    for (int j = 0; j < items.Count; j++)
                    {
                        if (items[j].itemcode == _item.itemcode)
                        {
                            count++;
                        }
                    }
                    Inveninfo.itemss[index].cnt = count++;
                }
                else
                {
                    C_InvenInfo.Items serveritem = new C_InvenInfo.Items();
                    serveritem.itemId = _item.itemcode.ToString();
                    serveritem.cnt = 1;
                    Inveninfo.itemss.Add(serveritem);
                }
            }

            for (int i = 0; i < Inveninfo.itemss.Count; i++)
            {
                Debug.Log("상태 : " + Inveninfo.itemss[i].itemId + " " + Inveninfo.itemss[i].cnt);
            }

            NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
            return true;
        }
        return false;
        
    }

    public bool AddWithoutServerItem(Item _item)
    {
        if (items.Count < SlotCnt)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == _item.itemName && _item.itemType == ItemType.Consumables)
                {
                    count[i] += 1;
                    if (onChangeItem != null)
                    {
                        onChangeItem.Invoke();
                    }
                    return true;
                }
            }
            items.Add(_item);
            count.Add(1);

            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }

            if (_item.itemType == ItemType.Consumables)
            {
                C_InvenInfo.Items serveritem = new C_InvenInfo.Items();
                serveritem.itemId = _item.itemcode.ToString();
                serveritem.cnt = 1;
                Inveninfo.itemss.Add(serveritem);
            }
            else
            {
                int index = CheckInvenInfo(_item.itemcode);
                if (index != -1)
                {
                    int count = 0;

                    for (int j = 0; j < items.Count; j++)
                    {
                        if (items[j].itemcode == _item.itemcode)
                        {
                            count++;
                        }
                    }
                    Inveninfo.itemss[index].cnt = count++;
                }
                else
                {
                    C_InvenInfo.Items serveritem = new C_InvenInfo.Items();
                    serveritem.itemId = _item.itemcode.ToString();
                    serveritem.cnt = 1;
                    Inveninfo.itemss.Add(serveritem);
                }
            }

            for (int i = 0; i < Inveninfo.itemss.Count; i++)
            {
                Debug.Log("상태 : " + Inveninfo.itemss[i].itemId + " " + Inveninfo.itemss[i].cnt);
            }

            NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItem fieldItem = collision.GetComponent<FieldItem>();
            if (AddItem(fieldItem.GetItem()))
            {
                itemSound.GetSound();
                fieldItem.DestroyItem();
            }
        }
    }

    public void RemoveItem(int _index)
    {
        int index = CheckInvenInfo(items[_index].itemcode);
        items.RemoveAt(_index);
        count.RemoveAt(_index);
        Inveninfo.itemss[index].cnt--;
        NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
        if (Inveninfo.itemss[index].cnt == 0)
        {
            Inveninfo.itemss.RemoveAt(index);
        }
        onChangeItem.Invoke();
    }

    public void RemoveWithoutServerItem(int _index)
    {
        int index = CheckInvenInfo(items[_index].itemcode);
        int Itemcount = 0;

        items.RemoveAt(_index);
        count.RemoveAt(_index);

        for (int j = 0; j < items.Count; j++)
        {
            if (items[j].itemcode == int.Parse(Inveninfo.itemss[index].itemId))
            {
                Itemcount++;
            }
        }

        if (Itemcount == 0)
            Inveninfo.itemss[index].cnt = 0;

        Inveninfo.itemss[index].cnt = Itemcount;

        NetPlayerManager.Instance.Session.Send(Inveninfo.Write());

        if (Inveninfo.itemss[index].cnt == 0)
        {
            Inveninfo.itemss.RemoveAt(index);
        }


        onChangeItem.Invoke();
    }

    public void RemoveItemSell(int _index)
    {
        int index = CheckInvenInfo(items[_index].itemcode);
        int Itemcount = 0;

        if (items[_index].itemType != ItemType.Consumables)
        {
            for (int j = 0; j < items.Count; j++)
            {
                if (items[j].itemcode == int.Parse(Inveninfo.itemss[index].itemId))
                {
                    Itemcount++;
                }
            }

            items.RemoveAt(_index);
            count.RemoveAt(_index);

            Itemcount -= 1;

            if (Itemcount == 0)
                Inveninfo.itemss[index].cnt = 0;

            Debug.Log("으악 : " + Inveninfo.itemss[index].cnt);

            Inveninfo.itemss[index].cnt = Itemcount;

            NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
            if (Inveninfo.itemss[index].cnt == 0)
            {
                Inveninfo.itemss.RemoveAt(index);
            }
        }
        else
        {
            items.RemoveAt(_index);
            count.RemoveAt(_index);
            Inveninfo.itemss.RemoveAt(index);
            NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
        }
    }

    public int CheckInvenInfo(int itemcode)
    {
        for (int i = 0; i < Inveninfo.itemss.Count; i++)
        {
            if (int.Parse(Inveninfo.itemss[i].itemId) == itemcode)
            {
                return i;
            }
        }
        return -1;
    }

    public void UndateServer()
    {
        NetPlayerManager.Instance.Session.Send(Inveninfo.Write());
    }
}