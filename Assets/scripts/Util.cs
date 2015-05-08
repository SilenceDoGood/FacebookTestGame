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
}
