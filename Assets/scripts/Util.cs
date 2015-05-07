using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public static class Util
{
	public static Dictionary<string, string> DeserializeJSONProfile(string response)
	{
		var responseObject = (Dictionary<string, object>)Json.Deserialize(response);
		object value;
		var profile = new Dictionary<string, string>();
		foreach (var key in responseObject.Keys)
		{
			if (responseObject.TryGetValue(key, out value))
			{
				profile[key] = (string)value;
			}
		}
		return profile;
	}
}
