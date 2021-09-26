using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	// Properties
	[SerializeField] [Range(0f, 1f)] private float alphaOffset = 0.4f;

	// Other staff
	public ItemSlot currSlot = null;
	[SerializeField] private Transform startParent;
	[HideInInspector] public Transform lastParent;
	[SerializeField] private bool returnObjectToStartPoint = false;
	private CanvasGroup canvasGroup;
	private Vector3 startPos;
	public string selfName;
	public int quantity = 1;
	public TextMeshProUGUI quantityText;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if (transform.parent.GetComponent<ItemSlot>() != null) currSlot = transform.parent.GetComponent<ItemSlot>();
		if (currSlot == null) ChangeCountText(true);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		startPos = transform.position;
		transform.SetParent(startParent);

		if (currSlot != null) currSlot.RemoveItemFromSlot(this);
		if (eventData.button == PointerEventData.InputButton.Right && quantity > 1)
		{
			DragAndDropItem halfItem = Instantiate(gameObject, transform.position, transform.rotation, lastParent).GetComponent<DragAndDropItem>();
			halfItem.quantity = quantity/2;
			quantity = Mathf.CeilToInt((float)quantity/2);
			currSlot.PlaceItemInSlot(halfItem);
		}

		canvasGroup.alpha -= alphaOffset;
		canvasGroup.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		mousePos.z = transform.position.z;
		transform.position = mousePos;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GameObject raycastedGameObject = eventData.pointerCurrentRaycast.gameObject;
		ItemSlot itemInSlot = null;
		ItemSlot slot = null;

		if (raycastedGameObject != null)
		{
			slot = raycastedGameObject.GetComponent<ItemSlot>();

			DragAndDropItem raycastedItem = raycastedGameObject.GetComponent<DragAndDropItem>();
			if (raycastedItem != null) itemInSlot = raycastedItem.GetComponent<DragAndDropItem>().currSlot;
		}

		if (slot == null)
		{
			if ((returnObjectToStartPoint && currSlot == null) || itemInSlot != null && !itemInSlot.PlaceItemInSlot(this)) ReturnObjectToLastPos();
			if (itemInSlot == null) currSlot = null;
		}
		else if (!slot.PlaceItemInSlot(this)) ReturnObjectToLastPos();

		canvasGroup.alpha += alphaOffset;
		canvasGroup.blocksRaycasts = true;
	}

	private void ReturnObjectToLastPos()
	{
		transform.position = startPos;
		if (currSlot != null) currSlot.PlaceItemInSlot(this);
		transform.SetParent(lastParent);
	}

	public void ChangeCountText(bool active)
	{
		if (quantity > 1)
		{
			quantityText.text = quantity.ToString();
			quantityText.enabled = active;
		}
		else quantityText.enabled = false;
	}
}
