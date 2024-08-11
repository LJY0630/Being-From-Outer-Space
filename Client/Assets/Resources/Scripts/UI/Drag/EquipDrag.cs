using UnityEngine;
using UnityEngine.EventSystems;

public class EquipDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;

    public Transform previousParent;
    public Item previousItem;
    private CanvasGroup canvasGroup;

    public int itemcode;

    [SerializeField]
    private ChangItem changItem;

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
        // 드래그 직전에 소속되어 있던 부모 Transform 정보 저장
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
                ItemEffectEquipment preeft = (ItemEffectEquipment)previousItem.efts[0];
                if (preeft.UnEquip(transform.root))
                {
                    Debug.Log("이큅드래그 1");
                    // 장비 해제시
                    C_UnEquipped c_UnEquipped = new C_UnEquipped();
                    c_UnEquipped.itemId = previousItem.itemcode.ToString();
                    NetPlayerManager.Instance.Session.Send(c_UnEquipped.Write());

                    transform.root.GetChild(1).GetComponent<Inventory>().UndateServer();

                    previousParent.GetComponent<Slot>().RemoveSlot();
                    changItem.NoEquip();
                    previousItem = null;
                }
            }
            else if (!previousItem.itemName.Equals(previousParent.GetComponent<Slot>().item.itemName))
            {
                ItemEffectEquipment preeft = (ItemEffectEquipment)previousItem.efts[0];
                if (preeft.UnEquip(transform.root))
                {

                    C_UnEquipped c_UnEquipped = new C_UnEquipped();
                    c_UnEquipped.itemId = previousItem.itemcode.ToString();
                    NetPlayerManager.Instance.Session.Send(c_UnEquipped.Write());

                    transform.root.GetChild(1).GetComponent<Inventory>().UndateServer();

                    previousParent.GetComponent<Slot>().UpdateSlotUI();
                    preeft = (ItemEffectEquipment)previousParent.GetComponent<Slot>().item.efts[0];
                    if (preeft.ExecuteRole(transform.root))
                    {

                        Debug.Log("이큅드래그 2");
                        C_Equipped c_Equipped = new C_Equipped();
                        c_Equipped.itemId = previousParent.GetComponent<Slot>().item.itemcode.ToString();
                        NetPlayerManager.Instance.Session.Send(c_Equipped.Write());

                        transform.root.GetChild(1).GetComponent<Inventory>().UndateServer();

                        previousItem = previousParent.GetComponent<Slot>().item;
                        changItem.SetItem(previousParent.GetComponent<Slot>().item.itemName);
                    }
                    Debug.Log("이큅드래그 3");
                }
            }
        }

        // 알파값을 1로 설정하고, 광선 충돌처리가 되도록 한다
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}