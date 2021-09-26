using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
	// Slot properties
	[HideInInspector] public Item slotItem = null;
	[HideInInspector] public int itemsCount;
	[HideInInspector] public int selfIndex;
	[HideInInspector] public TextMeshProUGUI itemsCountText;
	private Image slotImg;

	// Buttons
	[HideInInspector] public ColorBlock buttonColors;
	private Coroutine currentCoroutine = null;

	// Other staff
	[HideInInspector] public Transform player;
	[HideInInspector] public Inventory inventory;
	private Vector3 itemScaleBeforeTaking;

	private void Awake()
	{
		slotImg = GetComponent<Image>();
		itemsCountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(currentCoroutine != null) StopCoroutine(currentCoroutine);
		currentCoroutine = StartCoroutine(ChangeColor(buttonColors.highlightedColor));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (currentCoroutine != null) StopCoroutine(currentCoroutine);
		currentCoroutine = StartCoroutine(ChangeColor(buttonColors.normalColor));
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (currentCoroutine != null) StopCoroutine(currentCoroutine);
		if (eventData.pointerCurrentRaycast.gameObject == gameObject)
		{
			currentCoroutine = StartCoroutine(ChangeColor(buttonColors.highlightedColor));
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				LeftButtonAction();
			}
			else if (eventData.button == PointerEventData.InputButton.Right)
			{
				RightButtonAction();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (currentCoroutine != null) StopCoroutine(currentCoroutine);
		currentCoroutine = StartCoroutine(ChangeColor(buttonColors.pressedColor));
	}

	private void RightButtonAction()
	{
		//RemoveItem();
		RemoveAllItems();
	}

	private void LeftButtonAction()
	{
		UseItem();
	}

	private IEnumerator ChangeColor(Color color)
	{
		while (slotImg.color != color)
		{
			slotImg.color = Color.Lerp(slotImg.color, color, buttonColors.fadeDuration * Time.deltaTime * 100);
			yield return null;
		}
		yield break;
	}

	public void UseItem()
	{
		if (itemsCount > 0)
		{
			itemsCount--;
			if (itemsCount == 0)
			{
				Destroy(slotItem.gameObject);
				slotItem = null;
				inventory.UpdateInventory(selfIndex);
			}
			UpdateItemsCountText();
		}
	}

	public void AddItem(Item item)
	{
		if (itemsCount == 0)
		{
			itemScaleBeforeTaking = item.transform.localScale;
			item.transform.position = transform.position;
			item.transform.SetParent(transform);
			slotItem = item;
		}
		else
		{
			Destroy(item.gameObject);
		}
		itemsCount += item.quantity;
		UpdateItemsCountText();
	}

	public void RemoveItem()
	{
		if (itemsCount > 0)
		{
			slotItem.quantity = 1;
			if (itemsCount > 1)
			{
				Item item = Instantiate(slotItem.gameObject, player.position, Quaternion.identity).GetComponent<Item>();
				item.transform.localScale = itemScaleBeforeTaking;
				item.ChangeCountText();
				item.StartCoroutine(item.HandleDroppingItem());
			}
			else if (itemsCount == 1)
			{
				slotItem.transform.position = player.position;
				slotItem.transform.parent = null;
				slotItem.StartCoroutine(slotItem.HandleDroppingItem());
				slotItem = null;
				inventory.UpdateInventory(selfIndex);
			}
			itemsCount--;
			UpdateItemsCountText();
		}
	}

	public void RemoveAllItems()
	{
		if (itemsCount > 0)
		{
			slotItem.transform.position = player.position;
			slotItem.transform.parent = null;

			slotItem.StartCoroutine(slotItem.HandleDroppingItem());
			slotItem.quantity = itemsCount;
			slotItem.ChangeCountText();

			slotItem = null;
			itemsCount = 0;
			inventory.UpdateInventory(selfIndex);
			UpdateItemsCountText();
		}
	}

	public void UpdateItemsCountText()
	{
		if (itemsCount > 1)
		{
			itemsCountText.text = itemsCount.ToString();
			itemsCountText.enabled = true;
		}
		else
		{
			itemsCountText.enabled = false;
		}
	}
}
