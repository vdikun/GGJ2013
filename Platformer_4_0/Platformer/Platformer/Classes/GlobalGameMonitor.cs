using System;
using System.Net;
using System.Threading;

public static class GlobalGameMonitor
{
	public static string DefaultUserName
	{
		get
		{
			return System.Environment.UserName;
		}
	}
	
	public static void SubmitResult(string gamename)
	{
		SubmitResultToServer(gamename, string.Empty, DefaultUserName, 0);
	}
	
	public static void SubmitResult(string gamename, int score)
	{
		SubmitResultToServer(gamename, string.Empty, DefaultUserName, score);
	}
	
	public static void SubmitResult(string gamename, string username)
	{
		SubmitResultToServer(gamename, string.Empty, username, 0);
	}
	
	public static void SubmitResult(string gamename, string username, int score)
	{
		SubmitResultToServer(gamename, string.Empty, username, score);
	}
	
	public static void SubmitResult(string gamename, string levelname, string username, int score)
	{
		SubmitResultToServer(gamename, levelname, username, score);
	}

    public delegate void RequestDelegate(string url);
	
	private static void SubmitResultToServer(string gamename, string levelname, string username, int score)
	{
        string url = "http://globalgameleaders.com/api/reportscore.php";
        url = string.Format("{0}?game={1}&level={2}&user={3}&score={4}", url, gamename, levelname, username, score);

        RequestDelegate caller = new RequestDelegate(SendRequest);
        caller.BeginInvoke(url, null, null);
    }

    public static void SendRequest(string url)
    {
        HttpWebRequest objRequest = (HttpWebRequest) WebRequest.Create(url);
        objRequest.Method = "GET";
        objRequest.GetResponse();
    }
}
