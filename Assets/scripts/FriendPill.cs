using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FriendPill : MonoBehaviour
{
	public Image friendImage;
	public Image loadingImage;
	public Text friendName;
	public Toggle toggle;

	public Rect spriteRect;

	public Friend myFriend;

	private bool isLoading = false;

	void Start()
	{
		loadingImage.enabled = false;
	}

	public void Initialize(Friend friend)
	{
		this.myFriend = friend;
		SetFriendName();
		StartCoroutine(SetFriendImage(myFriend.pictureURL));
	}

	private void SetFriendName()
	{
		friendName.text = myFriend.name;
	}

	private IEnumerator SetFriendImage(string url)
	{
		isLoading = true;
		StartCoroutine(LoadingAnimation());
		WWW www = new WWW(url);
		yield return www;
		if (www.texture != null)
		{
			CreateFriendSprite(www.texture);
			isLoading = false;
		}
	}

	private void CreateFriendSprite(Texture2D texture)
	{
		var friendSprite = Sprite.Create(texture, spriteRect, Vector2.zero);
		friendImage.sprite = friendSprite;
	}

	private IEnumerator LoadingAnimation()
	{
		loadingImage.enabled = true;
		var loadingRect = loadingImage.rectTransform;
		while (isLoading)
		{
			var newAngles = loadingRect.eulerAngles;
			newAngles.z += Time.deltaTime;
			loadingImage.rectTransform.eulerAngles = newAngles;
			yield return null;
		}
		loadingImage.enabled = false;
	}
}
