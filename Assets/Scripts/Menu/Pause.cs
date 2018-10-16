using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
	// ++ means plus 1
	bool paused;

	[Header("Panels")]
	public GameObject controls;
	public GameObject volume, graphics;
	public GameObject pausePanel;
	public GameObject pauseButtons;
	public GameObject optionsPanel;

	void Start()
	{
		pausePanel.SetActive(false);
		Time.timeScale = 1;
		paused = false;
		optionsPanel.SetActive(false);
	}
	
	void Update()
	{
		PauseInfo();
	}
	public void RestartGame()
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
	public void ResumeGame()
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		pausePanel.SetActive(false);
		Cursor.visible = false;
		paused = false;
		// setting the timeScale to 1 so game is unpaused
		Time.timeScale = 1;
	}
	public void BackToMenu()
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}
	void PauseInfo()
	{
		if (Input.GetKeyDown(KeyCode.P) && !paused || Input.GetKeyDown(KeyCode.Escape) && !paused)
		{
			pausePanel.SetActive(true);
			paused = true;
			Cursor.visible = true;
			// setting the timeScale to 0 so game is paused
			Time.timeScale = 0;
		}
		else if (Input.GetKeyDown(KeyCode.P) && paused || Input.GetKeyDown(KeyCode.Escape) && paused)
		{
			pausePanel.SetActive(false);
			paused = false;
			Cursor.visible = false;
			// setting the timeScale to 1 so game is unpaused
			Time.timeScale = 1;
		}

	}
	public void Options()
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		optionsPanel.SetActive(true);
		pauseButtons.SetActive(false);
	}
	public void Back()
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		optionsPanel.SetActive(false);
		pauseButtons.SetActive(true);
		controls.SetActive(false);
		volume.SetActive(false);
		graphics.SetActive(false);
	}
}
