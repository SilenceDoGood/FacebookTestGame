using UnityEngine;

public class Friend
{
	public string inviteToken;
	public string name;
	public string pictureURL;

	public Friend(string token, string name, string url)
	{
		this.inviteToken = token;
		this.name = name;
		this.pictureURL = url;
	}
}
