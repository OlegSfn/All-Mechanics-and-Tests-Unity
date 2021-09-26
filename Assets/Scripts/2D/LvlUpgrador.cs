using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class LvlUpgrador : MonoBehaviour
{
	[Header("Variables")]
	private int score = 0, level = 1;
	private float sliderProgress;
	[SerializeField] private int scoreToReachNewLevel;

	[Header("Slider")]
	[SerializeField] private Slider slider;
	[SerializeField] private float sliderSpeed;

	[Header("Particles")]
	[SerializeField] private ParticleSystem sliderParticles;
	[SerializeField] private ParticleSystem newLevelParticles;

	[Header("Texts")]
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI lastLevelText;
	[SerializeField] private TextMeshProUGUI nextLevelText;

	public void AddScore(int addScore)
	{
		score += addScore;
		StartCoroutine(ApplyScoreToSlider(addScore));
		scoreText.text = score.ToString();
	}

	private void UpdateLevel()
	{
		sliderProgress -= 1;
		slider.value = 0;

		level += 1;
		lastLevelText.text = level.ToString();
		nextLevelText.text = (level+1).ToString();

		newLevelParticles.Play();
	}

	public IEnumerator ApplyScoreToSlider(float addScore) {
		sliderProgress = slider.value + (addScore/scoreToReachNewLevel);
		sliderParticles.Play();

		while (slider.value < sliderProgress)
		{
			slider.value += Time.deltaTime * sliderSpeed;
			if (slider.value == 1) UpdateLevel();
			yield return null;
		}

		slider.value = sliderProgress;
		sliderParticles.Stop();
		yield break;
	}
}
