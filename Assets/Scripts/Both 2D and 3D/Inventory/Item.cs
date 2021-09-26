using System.Collections;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [HideInInspector] public bool isPickable = true;

    public Sprite InvItemSprite;
    public int maxStackCount = 1;
    public int quantity = 1;
    public string itemName;
    public float unpickableTime = 1f;
    [HideInInspector] public TextMeshPro countText;

	private void Awake()
	{
        countText = transform.GetChild(0).GetComponent<TextMeshPro>();
        ChangeCountText();
	}

    public void ChangeCountText()
	{
        if (quantity > 1)
        {
            countText.enabled = true;
            countText.text = quantity.ToString();
        }
		else countText.enabled = false;
	}

	public IEnumerator HandleDroppingItem()
	{
        isPickable = false;
        float startTime = Time.time;

		while (Time.time - startTime < unpickableTime)
		{
            yield return new WaitForSeconds(unpickableTime/10);
		}

        isPickable = true;
        yield break;
	}
}
