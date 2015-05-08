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

	private Dictionary<string, object> profile = null;
	private Sprite profileSprite = null;

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

	public void Friends()
	{
		if(FB.IsLoggedIn)
		{
			FB.API("/v2.3/me?fields=friends.fields(id,first_name,last_name,picture.width(128))", HttpMethod.GET, FriendsCallback);
		}
	}

	private void FriendsCallback(FBResult result)
	{
		if(!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log("Error during Inviteable_friends call! " + result.Error);
		}
		else if (FB.IsLoggedIn)
		{
			Debug.Log(result.Text);
		}
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
			FB.API("/v2.3/me?fields=id,first_name,last_name,picture.width(865)", HttpMethod.GET, APIProfileCallBack);
			if (OnLoggedIn != null)
			{
				OnLoggedIn();
			}
		}
	}

	private void APIProfileCallBack(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log("Facebook API Call failed! " + result.Error);
			return;
		}
		Debug.Log (result.Text);
		profile = Util.DeserializeJSONProfile(result.Text);
		InitFaceBookProfile();
	}

	private delegate void LoadPictureCallback (Texture2D texture);

	private void InitFaceBookProfile()
	{
		facebookUsername.text = "" + profile["first_name"] + " " + profile["last_name"];

		var pictureData = (Dictionary<string, object>)profile["picture"];
		var pictureInfo = (Dictionary<string, object>)pictureData["data"];


		StartCoroutine(GetProfilePicture(pictureInfo["url"].ToString(), LoadPicture));
	}

	private void LoadPicture(Texture2D texture)
	{
		var picture = Sprite.Create(texture, new Rect(0, 0, 865f, 865f), Vector2.zero);
		facebookProfilePicture.sprite = picture;
	}

	private IEnumerator GetProfilePicture(string url, LoadPictureCallback callback = null)
	{
		WWW www = new WWW(url);
		yield return www;
		if(callback != null)
		{
			callback(www.texture);
		}
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
