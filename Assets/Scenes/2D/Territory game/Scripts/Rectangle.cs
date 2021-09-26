using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rectangle : MonoBehaviour
{
	public Vector2Int size = new Vector2Int(1, 1);
	[SerializeField] private TextMeshProUGUI square;
	[SerializeField] private Image rectImg;
	[SerializeField] private RectTransform rt;
	private Color defaultColor;

	private void Awake()
	{
		defaultColor = rectImg.color;
	}

	public void FitScaleToSize()
	{
		rt.sizeDelta = size * 100;
	}
	public Vector2 GetOffsetValue()
	{
		return new Vector2((float)(size.x - 1) / 2, (float)(size.y - 1) / 2);
	}

	// Set values
	public void SetSize(Vector2Int newSize)
	{
		size = newSize;
		FitScaleToSize();
		UpdateSquareText();
	}
	public void SetParent(Transform parent)
	{
		transform.SetParent(parent);
	}
	public void SetTransparentColor(bool available)
	{
		if (available)
			rectImg.color = Color.green;
		else
			rectImg.color = Color.red;
	}
	public void SetNormalColor()
	{
		rectImg.color = defaultColor;
	}
	
	
	private void UpdateSquareText()
	{
		RectTransform textRT = square.gameObject.GetComponent<RectTransform>();
		textRT.sizeDelta = new Vector2(size.x*100, 40+(size.x*size.y)*5);
		square.text = (size.x*size.y).ToString();
	}
}
