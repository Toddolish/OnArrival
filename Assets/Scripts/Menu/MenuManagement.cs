using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManagement : MonoBehaviour
{
	// ++ means plus 1
	[Header("Camera")]
	public CameraSwap cameraSwap;

	[Header("Panels")]
	public GameObject controls;
	public GameObject volume, graphics;

	[Header("Audio")]
	public AudioMixer masterMixer;
	public Slider masterSlider;

	[Header("Graphics")]
	Resolution[] resolutions;
	public Dropdown resolutionDropdown;
	void Start()
	{
		// Load the volume
		masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 40);

		// Setting timescale to 1 in this script and when backToMenu is pressed in pause script
		Time.timeScale = 1;
		// Accesing the reference to the cameraSwap script component
		
		controls.SetActive(false);
		volume.SetActive(false);
		graphics.SetActive(false);

		resolutions = Screen.resolutions;
		int currentResIndex = 0;
		List<string> options = new List<string>();
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				currentResIndex = i;
			}
		}
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResIndex;
		resolutionDropdown.RefreshShownValue();
	}
	public void PlayGame()
	{
		FindObjectOfType<AudioManager>().Play("swarm");
		// Load game scene
		SceneManager.LoadScene(1);

		// Also using this button to restart game.. since there is only one level...
	}
	public void OptionsMenu()
	{
		cameraSwap = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSwap>();
		FindObjectOfType<AudioManager>().Play("Click2");
		cameraSwap.swapCamera();
	}
	public void Back()
	{
		cameraSwap = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSwap>();
		FindObjectOfType<AudioManager>().Play("Click2");
		cameraSwap.swapCamera();
		controls.SetActive(false);
		volume.SetActive(false);
		graphics.SetActive(false);
	}
	public void ExitGame()// never press this fking button.... na it's ok i understand
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		Application.Quit();
	}
	public void Controls()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		controls.SetActive(true);
		volume.SetActive(false);
		graphics.SetActive(false);
	}
	public void Volume()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		controls.SetActive(false);
		volume.SetActive(true);
		graphics.SetActive(false);
	}
	public void Graphics()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		controls.SetActive(false);
		volume.SetActive(false);
		graphics.SetActive(true);
	}
	public void SetVolume(float volume)
	{
		masterMixer.SetFloat("volume", volume);
	}
	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}
	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}
	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
	public void SaveData()
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
	}
}
