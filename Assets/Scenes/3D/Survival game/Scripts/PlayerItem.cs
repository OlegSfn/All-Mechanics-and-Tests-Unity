using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Slider cooldownBar;
    [SerializeField] private float cooldownTime;
    [SerializeField] private Transform itemPos;

    private bool cooldown = false;
    private int itemIndex, itemCount;
    private ItemInfo item;

	private void Awake()
	{
        itemCount = itemPos.childCount;
        FindActiveItem();
	}

	void Update()
    {
        if (cooldown) Cooldown();

        if (Input.GetMouseButton(0) && cooldownBar.value == cooldownBar.maxValue)
		{
            anim.SetBool("isDigging", true);
            cooldownBar.value = 0;
            cooldown = true;
		}
		else anim.SetBool("isDigging", false);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
            itemIndex += 1;
            if (itemIndex >= itemCount) itemIndex = 0;
            ChangeActiveItem();
		}
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
            itemIndex -= 1;
            if (itemIndex < 0) itemIndex = itemCount - 1;
            ChangeActiveItem();
		}
    }

    private void Cooldown()
	{
        cooldownBar.value += 1 / cooldownTime * Time.deltaTime;
        if (cooldownBar.value == cooldownBar.maxValue)
            cooldown = false;
    }

    private void FindActiveItem()
	{
        for (int i = 0; i < itemCount; i++)
        {
            GameObject child = itemPos.GetChild(i).gameObject;
            if (child.activeSelf)
            {
                ItemInfo itemInfo = child.GetComponent<ItemInfo>();
                if (itemInfo != null)
                {
                    itemIndex = i;
                    item = itemInfo;
                    cooldownTime = item.cooldown;
                    anim.runtimeAnimatorController = item.animController;
                    return;
                }
            }
        }
    }

    private void ChangeActiveItem()
	{
        //Last item
        item.gameObject.SetActive(false);

        // Next item
        item = itemPos.GetChild(itemIndex).GetComponent<ItemInfo>();
        item.gameObject.SetActive(true);
        cooldownTime = item.cooldown;
        anim.runtimeAnimatorController = item.animController;
    }
}
