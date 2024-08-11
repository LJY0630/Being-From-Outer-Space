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
                ItemEffectWeapon preeft = (ItemEffectWeapon)previousItem.efts[0];
                if (preeft.UnEquip(transform.root))
                {
                    // 장착중이던 무기를 서버에서 해제? 
                    Debug.Log("해제요");
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
                // 무기 해제시
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

        // 알파값을 1로 설정하고, 광선 충돌처리가 되도록 한다
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}