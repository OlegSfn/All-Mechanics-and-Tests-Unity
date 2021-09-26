using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour 
{
	public DragAndDropItem slotItem = null;
	public int itemsCount = 0;
	public TextMeshProUGUI itemsCountText;

	public bool PlaceItemInSlot(DragAndDropItem item)
	{
		if (slotItem != null && item.currSlot != null && item.selfName != slotItem.selfName)
		{
			ItemSlot slotOfItem = item.currSlot;
			slotOfItem.PlaceItemInSlot(slotItem);
			RemoveItemFromSlot(slotItem);
		}

		if (slotItem == null || item.selfName == slotItem.selfName)
		{
			itemsCount += item.quantity;
			ChangeCountText();
			if (itemsCount > 1 && slotItem != null)
			{
				Destroy(item.gameObject);
				return true;
			}

			item.transform.position = transform.position;
			item.transform.SetParent(transform);
			item.transform.SetAsFirstSibling();
			item.lastParent = item.transform.parent;
			item.currSlot = this;
			item.ChangeCountText(false);
			slotItem = item;

			return true;
		}
		return false;
	}

	public void RemoveItemFromSlot(DragAndDropItem item)
	{
		//if (itemsCount > 1) Instantiate(item.gameObject, transform.position, Quaternion.identity, transform);
		//else item.currSlot.slotItem = null;
		slotItem = null;
		item.quantity = itemsCount;
		itemsCount = 0;
		item.ChangeCountText(true);
		ChangeCountText();
	}

	public void ChangeCountText()
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
