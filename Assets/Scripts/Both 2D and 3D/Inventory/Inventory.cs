using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    private Slot[] slots;
	[SerializeField] private Transform player;

	[Header("Inventory buttons")]
	[SerializeField] private Color normalColor;
	[SerializeField] private Color highlightedColor;
	[SerializeField] private Color pressedColor;
	[SerializeField] [Range(1f, 5f)] private float colorMultiplier = 1f;
	[SerializeField] [Range(0f, 100f)] private float fadeDuration = 0.1f;


	private void Awake()
	{
		slots = new Slot[transform.childCount];

		for (int i = 0; i < slots.Length; i++)
		{
			GameObject slot = transform.GetChild(i).gameObject;
			slots[i] = slot.AddComponent<Slot>();
			slots[i].player = player;
			slots[i].inventory = this;
			slots[i].selfIndex = i;

			slots[i].buttonColors.normalColor = normalColor;
			slots[i].buttonColors.highlightedColor = highlightedColor;
			slots[i].buttonColors.pressedColor = pressedColor;
			slots[i].buttonColors.colorMultiplier = colorMultiplier;
			slots[i].buttonColors.fadeDuration = fadeDuration;
		}
	}

	public void AddItem(Item item)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			Slot slot = slots[i];
			if (slot.slotItem == null || (slot.slotItem.itemName == item.itemName && slot.slotItem.maxStackCount != slot.itemsCount))
			{
				item.countText.enabled = false;
				if (slot.itemsCount+item.quantity > item.maxStackCount)
				{
					GameObject itemGameObj = item.gameObject;
					Item newItem = Instantiate(itemGameObj, itemGameObj.transform.position, itemGameObj.transform.rotation).GetComponent<Item>();
					
					newItem.quantity = item.quantity - (item.maxStackCount - slot.itemsCount);
					item.quantity = item.maxStackCount - slot.itemsCount;

					newItem.ChangeCountText();
					slot.AddItem(item);
					AddItem(newItem);
				}
				else slot.AddItem(item);

				return;
			}
		}
	}

	public void UpdateInventory(int startIndex)
	{
		for (int i = startIndex; i < slots.Length-1; i++)
		{
			slots[i].slotItem = slots[i+1].slotItem;
			slots[i].itemsCount = slots[i+1].itemsCount;
			slots[i].UpdateItemsCountText();

			if (slots[i+1].slotItem == null) return;
			slots[i+1].slotItem.gameObject.transform.position = slots[i].transform.position;
			slots[i+1].slotItem.gameObject.transform.SetParent(slots[i].transform);
			if (i+1 == slots.Length-1)
			{
				slots[i+1].slotItem = null;
				slots[i+1].itemsCount = 0;
				slots[i+1].UpdateItemsCountText();
			}
		}
	}
}
