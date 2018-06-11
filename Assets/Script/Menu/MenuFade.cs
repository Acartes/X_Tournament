using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.SceneManagement;

public class MenuFade : MonoBehaviour
{

	public static MenuFade Instance;

	Image MenuFadeBlack;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
		MenuFadeBlack = GameObject.Find("MenuFadeBlack").GetComponent<Image>();
	}

	public IEnumerator FadeIn(string mode)
	{
		for (float i = 1f; i < 105f; i += 3f)
		{
			yield return new WaitForSeconds(0.01f);
			MenuFadeBlack.color = new Color(0, 0, 0, i / 100f);
		}

		if (mode == "Play")
			LobbyManager.Instance.StartHost();

		if (mode == "MenuThenPlay")
			SceneManager.LoadScene("Lobby réseau", LoadSceneMode.Single);

		if (mode == "Menu")
		{
			SceneManager.LoadScene("Lobby réseau", LoadSceneMode.Single);
			StartCoroutine(FadeOut());
		}
	}

	public IEnumerator FadeOut()
	{
		for (float i = 100f; i > -5f; i -= 3f)
		{
			yield return new WaitForSeconds(0.01f);
			MenuFadeBlack.color = new Color(0, 0, 0, i / 100f);
		}
	}
}
