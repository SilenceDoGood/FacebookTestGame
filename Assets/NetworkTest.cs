using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Facebook.MiniJSON;

public class NetworkTest : MonoBehaviour
{
	public string accessToken;
	public Text text;
	private delegate void authRequestCallBack();

	void Start()
	{
		StartCoroutine(SendToAuthPoint(StartAPIRequest));
	}

	private IEnumerator SendToAuthPoint(authRequestCallBack callback)
	{
		var deviceID = "";
		for (int i = 0; i < 32; ++i)
		{
			deviceID += Random.Range(0, 9).ToString();
		}
		var postData = "FB_ACCESS_TOKEN=CAAE1YteNiZB8BALghJVRH9foHBwYD0cZC6lOliVZBmZAljkaP5ZBEXRjZA51sZBZCRVF87uoewWjN0RXRrsBa7qLeL99tRauqtlGeXFZCG1yACtX0FLfajUb9Rkjbs4COLl5Eh53bLnxAHfC4OZAZAqlkGcGXdLXU77eqB9pfm5ZCuhwyr9OBf7L2ZB7M0tKk92tuM4BIbU96Vih4IB1pa3rW1AqigXKEZCr2eDm4ZD";
		postData += "&DEVICE_ID=" + deviceID;
		var data = Encoding.ASCII.GetBytes(postData);

		WebAsync webAsync = new WebAsync();
		WebRequest authRequest = HttpWebRequest.Create("http://gemjunction.peterbue.com/api/v0.1/auth");
		authRequest.Method = "POST";
		authRequest.ContentType = "Application/x-www-form-urlencoded";
		authRequest.ContentLength = data.Length;

		using(var stream = authRequest.GetRequestStream())
		{
			stream.Write(data, 0, data.Length);
			stream.Close();
		}

		IEnumerator e = webAsync.GetResponse(authRequest);
		while(e.MoveNext()) { yield return e.Current; }

		using (System.IO.StreamReader stream = new System.IO.StreamReader(webAsync.requestState.webResponse.GetResponseStream()))
		{
			var dictionary = (Dictionary<string, object>)Json.Deserialize(stream.ReadToEnd());
			accessToken = dictionary["access_token"].ToString();
		}
		if(callback != null)
		{
			callback();
		}
	}

	private void StartAPIRequest() 
	{
		StartCoroutine(SentToPlayerInfo());
	}

	private IEnumerator SentToPlayerInfo() 
	{
		WebAsync webAsync = new WebAsync();
		WebRequest apiRequest = HttpWebRequest.Create("http://gemjunction.peterbue.com/api/v0.1/PlayerInfo?access_token=" + accessToken + "&numLives&numMaxLives&highestLevelCompleted");
		apiRequest.Method = "GET";

		IEnumerator e = webAsync.GetResponse(apiRequest);
		while (e.MoveNext()) { yield return e.Current;  }

		using (System.IO.StreamReader stream = new System.IO.StreamReader(webAsync.requestState.webResponse.GetResponseStream()))
		{
			text.text = stream.ReadToEnd();
		}
	}

	private IEnumerator sendDeleteRequest()
	{
		WebAsync webAsync = new WebAsync();
		bool areWe;
		//WebRequest requestAnyURL = HttpWebRequest.Create("http://www.example.com");
		WebRequest requestAnyURL = HttpWebRequest.Create("http://gemjunction.peterbue.com/request.php");
		
		requestAnyURL.Method = "LOLWALLHAX";
 
		IEnumerator e = webAsync.GetResponse(requestAnyURL);
		while (e.MoveNext()) { yield return e.Current; Debug.Log("waiting for response..."); }
 
		areWe = (webAsync.requestState.errorMessage == null);
 
		Debug.Log("Are we connected to the inter webs? "+ areWe);
		System.IO.StreamReader responseReader = new System.IO.StreamReader(webAsync.requestState.webResponse.GetResponseStream());
		print(responseReader.ReadToEnd());
		yield return null;
	}
}
