using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstTry : MonoBehaviour
{
	public Transform button;
	public TMP_Text panelText;

	public LeanTweenType easeType;
	private void Awake()
	{
		transform.LeanScale(Vector3.one, 1f).setEase(easeType);

		LeanTween.value(gameObject, 0f, 1f, 1f).setOnUpdate((float val) =>
		{
			Image img = gameObject.GetComponent<Image>();
			Color c = img.color;
			c.a = val;
			img.color = c;
		}).setEase(easeType);

		panelText.LeanAlphaText( 1f, 0.6f).setEase(LeanTweenType.easeInCirc).delay = 2f;
		button.LeanScale(Vector3.one, 0.6f).setEaseOutBounce().setOnComplete(() => { button.gameObject.GetComponent<Button>().interactable = true; }).delay = 2f;
	}

	public void Close()
	{
		panelText.LeanAlphaText(0f, 0.6f).setEase(LeanTweenType.easeInCirc);
		button.LeanScale(Vector3.zero, 0.6f).setEaseOutBounce().setOnComplete(() => { button.gameObject.GetComponent<Button>().interactable = false; });

		transform.LeanScale(Vector3.zero, 1f).setEase(easeType).delay = 0.3f;

		LeanTween.value(gameObject, 1f, 0f, 1f).setOnUpdate((float val) =>
		{
			Image img = gameObject.GetComponent<Image>();
			Color c = img.color;
			c.a = val;
			img.color = c;
		}).setEase(easeType);
	}
}
