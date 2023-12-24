using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	public void LoadPreviousScene()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		if (currentSceneIndex > 0)
		{
			SceneManager.LoadScene(currentSceneIndex - 1);
		}
	}
}