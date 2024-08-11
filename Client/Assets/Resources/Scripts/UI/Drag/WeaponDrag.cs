using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;

    public Transform previousParent;
    public Item previousItem;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private WeaponChanger changItem;

    private void Awake()
    {
        previousItem = null;
    }

    private void Start()
    {
        canvas = transform.root.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        canvasGroup = GetComponent<CanvasGroup>();
        previousItem = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = transform.parent;
        previousItem = previousParent.GetComponent<Slot>().item;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            transform.SetAsFirstSibling();
            transform.position = previousParent.GetComponent<RectTransform>().position;

            if (previousParent.GetComponent<Slot>().item == null)
            {
                ItemEffectWeapon preeft = (ItemEffectWeapon)previousItem.efts[0];
                if (preeft.UnEquip(transform.root))
                {
                    // �������̴� ���⸦ �������� ����? 
                    Debug.Log("������");
                    Debug.Log(previousItem.itemcode.ToString());
                    C_UnEquipped c_UnEquipped = new C_UnEquipped();
                    c_UnEquipped.itemId = previousItem.itemcode.ToString();
                    NetPlayerManager.Instance.Session.Send(c_UnEquipped.Write());

                    transform.root.GetChild(1).GetComponent<Inventory>().UndateServer();

                    previousParent.GetComponent<Slot>().RemoveSlot();
                    changItem.SetNothing();
                    previousItem = null;
                }
                Debug.Log("weapondrag if");
                // ���� ������
            }
            else if (!previousItem.itemName.Equals(previousParent.GetComponent<Slot>().item.itemName))
            {
                ItemEffectWeapon preeft = (ItemEffectWeapon)previousItem.efts[0];
                if (preeft.UnEquip(transform.root))
                {
                    C_UnEquipped c_UnEquipped = new C_UnEquipped();
                    c_UnEquipped.itemId = previousItem.itemcode.ToString();
                    NetPlayerManager.Instance.Session.Send(c_UnEquipped.Write());

                    transform.root.GetChild(1).GetComponent<Inventory>().UndateServer();

                    previousParent.GetComponent<Slot>().UpdateSlotUI();
                    preeft = (ItemEffectWeapon)previousParent.GetComponent<Slot>().item.efts[0];
                    if (preeft.ExecuteRole(transform.root))
                    {

                        C_Equipped c_Equipped = new C_Equipped();
                        c_Equipped.itemId = previousParent.GetComponent<Slot>().item.itemcode.ToString();
                        NetPlayerManager.Instance.Session.Send(c_Equipped.Write());

                        transform.root.GetChild(1).GetComponent<Inventory>().UndateServer();

                        previousItem = previousParent.GetComponent<Slot>().item;
                        changItem.WeaponChange(previousParent.GetComponent<Slot>().item.itemName);
                    }
                }
                Debug.Log("weapondrag else if ");
            }
        }

        // ���İ��� 1�� �����ϰ�, ���� �浹ó���� �ǵ��� �Ѵ�
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}