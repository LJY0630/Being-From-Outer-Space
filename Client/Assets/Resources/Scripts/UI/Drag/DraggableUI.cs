using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private	Transform canvas;

	public	Transform previousParent;
	private Item previousItem;
	private	CanvasGroup	canvasGroup;		

	private void Start()
	{
		canvas = transform.root.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
		canvasGroup	= GetComponent<CanvasGroup>();
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

		if ( transform.parent == canvas )
		{
			transform.SetParent(previousParent);
			transform.SetAsFirstSibling();
			transform.position = previousParent.GetComponent<RectTransform>().position;
			Debug.Log("�巡�׿��̺� 1");
		}

		// ���İ��� 1�� �����ϰ�, ���� �浹ó���� �ǵ��� �Ѵ�
		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}
}

