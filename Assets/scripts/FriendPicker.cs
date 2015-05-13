using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FriendPicker : MonoBehaviour
{
	public ScrollRect scrollRect;
	public RectTransform viewAreaRect;
	public int columnCount;


	public GameObject pillPrefab;

	private RectTransform contentContainerRect;
	private List<Friend> friendsList;
	private float rowHeight;

	void Start()
	{
		//DEBUG LAUNCHER
		//List<Friend> newList = new List<Friend>();
		//for (int i = 0; i < 50; ++i)
		//{
		//	newList.Add(new Friend(i.ToString(), i.ToString(), i.ToString()));
		//}
		//Initialize(newList);
	}

	public void Initialize(List<Friend> friendsList, Transform parent)
	{
		this.friendsList = friendsList;
		GetComponent<RectTransform>().SetParent(parent);
		CreateContentContainer();
		PopulateFriendPicker();
	}

	private void CreateContentContainer()
	{
		contentContainerRect = new GameObject("contentContainer", new System.Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
		contentContainerRect.SetParent(viewAreaRect);
		scrollRect.content = contentContainerRect;
		contentContainerRect.anchorMin = Vector2.zero;
		contentContainerRect.anchorMax = Vector2.one;
		rowHeight = viewAreaRect.rect.height / 3f;
		contentContainerRect.offsetMin = new Vector2(0f, viewAreaRect.rect.height - (rowHeight * (friendsList.Count / columnCount)));
		contentContainerRect.offsetMax = new Vector2(0f, 0f);
	}

	private void PopulateFriendPicker()
	{
		RectTransform currentItem;
		float x, y;
		for (int i = 0; i < friendsList.Count; ++i)
		{
			currentItem = ((GameObject)Instantiate(pillPrefab)).GetComponent<RectTransform>();
			currentItem.SetParent(contentContainerRect);
			currentItem.anchorMin = new Vector2(0f, 1f);
			currentItem.anchorMax = new Vector2(0f, 1f);
			
			x = (i % columnCount) * contentContainerRect.rect.width / columnCount;
			y = -((i / columnCount + 1) * rowHeight);
			currentItem.offsetMin = new Vector2(x, y);

			x = x + contentContainerRect.rect.width / columnCount;
			y = y + rowHeight;
			currentItem.offsetMax = new Vector2(x, y);

			currentItem.GetComponent<FriendPill>().Initialize(friendsList[i]);
		}
	} 
}
