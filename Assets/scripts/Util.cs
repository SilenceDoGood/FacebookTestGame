using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public static class Util
{
	public static Dictionary<string, object> DeserializeJSONProfile(string response)
	{
		var responseObject = (Dictionary<string, object>)Json.Deserialize(response);
		return responseObject;
	}

	public static Dictionary<string, object> DeserializeJSONProfilePictureData(Dictionary<string, object> profile)
	{
		return (Dictionary<string, object>)((Dictionary<string, object>)profile["picture"])["data"];
	}

	public static List<Friend> DeserializeJSONFriendsList(string response)
	{
		var responseObject = (Dictionary<string, object>)Json.Deserialize(response);
		var objectList = (List<object>)responseObject["data"];
		List<Friend> friendsList = new List<Friend>();
		foreach (var o in objectList)
		{
			var tokenData = (Dictionary<string, object>)o;
			var pictureData = (Dictionary<string, object>)((Dictionary<string, object>)tokenData["picture"])["data"];
			var friend = new Friend(tokenData["id"].ToString(), tokenData["name"].ToString(), pictureData["url"].ToString());
			friendsList.Add(friend);
		}
		return friendsList;
	}
}
