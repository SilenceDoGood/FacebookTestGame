using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FriendPicker : MonoBehaviour
{
	public Canvas parentCanvas;
	public ScrollRect scrollRect;
	public RectTransform viewAreaRect;
	public int columnCount;

	public Text titleText;
	public Button selectAllButton;
	public Button sendButton;

	public GameObject pillPrefab;

	private enum SelectState
	{
		AllOn,
		AllOff
	}

	private SelectState selectState = SelectState.AllOff;
	private bool isSelectStateLocked = false;

	private RectTransform contentContainerRect;
	private List<Friend> completeFriendList;

	private List<FriendPill> friendPills = null;
	private float rowHeight;
	private int maxSelections = 0;
	private int numSelected = 0;
	private bool isInitialized = false;

	public delegate void APIMethodCallback(List<Friend> returnedFriends);
	private APIMethodCallback callBack = null;
	private List<Friend> returnFriendsList = null;

	void Start()
	{

	}

	public void Initialize(List<Friend> friendsList, APIMethodCallback callback, string title = "", int maxSelections = 0) 
	{
		if (!isInitialized)
		{
			this.completeFriendList = friendsList;
			this.callBack = callback;

			if (title.Length > 0)
			{
				titleText.text = title;
			}

			if (maxSelections > 0)
			{
				this.maxSelections = maxSelections;
				selectState = SelectState.AllOn;
				selectAllButton.GetComponentInChildren<Text>().text = "Deselect All";
				isSelectStateLocked = true;
			}
			scrollRect.onValueChanged.AddListener(OnScrollValueChange);
			CreateContentContainer();
			PopulateFriendPicker();
			isInitialized = true;
		}
	}

	public void Send()
	{
		returnFriendsList = new List<Friend>();

		for (int i = 0; i < friendPills.Count; ++i)
		{
			if (friendPills[i].toggle.isOn)
			{
				returnFriendsList.Add(friendPills[i].myFriend);
			}
		}

		if (callBack != null)
		{
			callBack(returnFriendsList);
		}
	}

	public void Close()
	{
		Destroy(parentCanvas.gameObject);
	}

	public void SelectAll(Text buttonText)
	{
		if (!isSelectStateLocked && selectState == SelectState.AllOff)
		{
			selectState = SelectState.AllOn;
			buttonText.text = "Deselect All";
			for(int i = 0; i < friendPills.Count; ++i)
			{
				friendPills[i].toggle.isOn = true;
			}
		}
		else
		{
			if (!isSelectStateLocked)
			{
				selectState = SelectState.AllOff;
				buttonText.text = "Select All";
			}
			for (int i = 0; i < friendPills.Count; ++i)
			{
				friendPills[i].toggle.isOn = false;
			}
		}
	}

	private void CreateContentContainer()
	{
		contentContainerRect = new GameObject("contentContainer", new System.Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
		contentContainerRect.SetParent(viewAreaRect);
		scrollRect.content = contentContainerRect;
		contentContainerRect.anchorMin = Vector2.zero;
		contentContainerRect.anchorMax = Vector2.one;
		rowHeight = viewAreaRect.rect.height / 3f;
		contentContainerRect.offsetMin = new Vector2(0f, viewAreaRect.rect.height - (rowHeight * (completeFriendList.Count / columnCount)));
		contentContainerRect.offsetMax = new Vector2(0f, 0f);
	}

	private void OnScrollValueChange(Vector2 scrollPosition)
	{
		//length plus 1 over 2 vertical number of rows (int math)
		//take scrollPosition * (the above) returns position in pill space
		Debug.Log(scrollPosition.y);
	}

	private void OnToggleValueChange(bool toggleValue)
	{
		if (toggleValue)
		{
			numSelected++;
		}
		else
		{
			numSelected--;
		}
		if (numSelected == 0)
		{
			sendButton.interactable = false;
		}
		else if (!sendButton.interactable && numSelected > 0)
		{
			sendButton.interactable = true;
		}

		SetToggleAvailableState(maxSelections == 0 || numSelected < maxSelections);
	}

	private void SetToggleAvailableState(bool state)
	{
		if (!state)
		{
			for (int i = 0; i < friendPills.Count; ++i)
			{
				if (!friendPills[i].toggle.isOn)
				{
					friendPills[i].toggle.interactable = false;
				}
			}
		}
		else
		{
			for (int i = 0; i < friendPills.Count; ++i)
			{
				friendPills[i].toggle.interactable = true;
			}
		}
	}

	private void PopulateFriendPicker()
	{
		RectTransform currentItem;
		float x, y;
		friendPills = new List<FriendPill>();
		for (int i = 0; i < completeFriendList.Count; ++i)
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

			var currentPill = currentItem.GetComponent<FriendPill>();
			friendPills.Add(currentPill);
			currentPill.Initialize(completeFriendList[i]);
			currentPill.toggle.onValueChanged.AddListener(OnToggleValueChange);
		}
	} 
}
