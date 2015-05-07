using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Action = System.Action;

public class FacebookMenu : MonoBehaviour
{
	public GameObject facebookPanel;
	public Image facebookProfilePicture;
	public Image facebookLoginButton;
	public Text facebookUsername;

	public static event Action OnLoggedIn;

	private Dictionary<string, string> profile = null;

	void Awake()
	{
		
	}

	void Start()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}

	void Update()
	{

	}

	public void OnInitComplete()
	{
		enabled = true;
		Debug.Log("Init Completed Successfully! Using App ID: " + FB.AppId + " Is ready? " + FB.IsInitialized + " is Logged in? " + FB.IsLoggedIn);
	}

	public void FacebookLogin()
	{
		FB.Login("user_friends, email, publish_actions, public_profile", LoginCallback);
	}

	private void LoginCallback(FBResult result)
	{
		if (!string.IsNullOrEmpty(result.Error)) 
		{
			Debug.Log("Error: " + result.Error);
		}
		else if (FB.IsLoggedIn)
		{
			Debug.Log("Login Successful");
			facebookLoginButton.enabled = false;
			facebookPanel.SetActive(true);
			FB.API("/v2.3/me?fields=id, first_name, last_name", HttpMethod.GET, APICallBack);
			if (OnLoggedIn != null)
			{
				OnLoggedIn();
			}
		}
	}

	private void APICallBack(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log("Facebook API Call failed! " + result.Error);
			return;
		}
		Debug.Log(result.Text);
	}

	private IEnumerator GetProfilePicture()
	{
		yield return null;
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}
}
