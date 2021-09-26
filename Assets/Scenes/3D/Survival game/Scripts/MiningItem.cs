using UnityEngine;
using UnityEngine.UI;
public class MiningItem : MonoBehaviour
{
	[SerializeField] private int maxHealth;
	[SerializeField] private GameObject dropItem;
	[SerializeField] private Color partSysColor;
	[SerializeField] private Slider healthBar;

	private ParticleSystem partSys;
	private float health;

	private void Awake()
	{
		health = maxHealth;
		healthBar.value = 1;

		partSys = transform.GetChild(0).GetComponent<ParticleSystem>();
		ParticleSystem.MainModule settings = partSys.main;
		settings.startColor = partSysColor;
	}

	private void ChangeSlider() => healthBar.value = health/maxHealth;


	private void OnTriggerEnter(Collider col)
	{
		ItemInfo item = col.GetComponent<ItemInfo>();
		if (item != null)
		{
			health -= item.damage;
			partSys.Play();
			if (health <= 0)
			{
				Instantiate(dropItem, transform.position, dropItem.transform.rotation);
				Destroy(gameObject);
			}
			ChangeSlider();
		}
	}
}
