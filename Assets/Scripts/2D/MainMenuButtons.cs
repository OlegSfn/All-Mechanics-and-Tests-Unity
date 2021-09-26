using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
	public LeanTweenType settingsAppearEase;

	public GameObject settingsPanel;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			settingsPanel.transform.LeanScale(Vector3.zero, 0.6f).setEase(settingsAppearEase);
		}
	}

	public void PlayButton(int buildIndex)
	{
		SceneManager.LoadScene(buildIndex);
	}

	public void SettingsButton(GameObject settingsPanel)
	{
		settingsPanel.transform.LeanScale(Vector3.one, 0.6f).setEase(settingsAppearEase);
	}

	public void ExitButton()
	{
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				 Application.Quit();
		#endif
	}

}
