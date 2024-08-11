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

		if ( transform.parent == canvas )
		{
			transform.SetParent(previousParent);
			transform.SetAsFirstSibling();
			transform.position = previousParent.GetComponent<RectTransform>().position;
			Debug.Log("드래그에이블 1");
		}

		// 알파값을 1로 설정하고, 광선 충돌처리가 되도록 한다
		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}
}

