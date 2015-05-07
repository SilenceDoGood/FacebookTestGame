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
		FB.Init(SetInit);
	}

	void Start()
	{

	}

	void Update()
	{

	}

	public void SetInit()
	{
		enabled = true;
	}

	public void FacebookLogin()
	{
		FB.Login("public_profile,user_friends,email,publish_actions", LoginCallback);
	}

	public void LoginCallback(FBResult result)
	{
		if (FB.IsLoggedIn)
		{
			facebookLoginButton.enabled = false;
			facebookPanel.SetActive(true);
			FB.API("/v2.3/me?fields=id, first_name, last_name", HttpMethod.GET, APICallBack);
		}
		if (OnLoggedIn != null)
		{
			OnLoggedIn();
		}
	}

	private void APICallBack(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log("Facebook API Call failed!");
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
