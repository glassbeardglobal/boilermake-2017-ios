using System; 
using UnityEngine;
using System.Collections; 
using System.Collections.Generic; 

class Server : MonoBehaviour {   
	//This is the URL we're making requests to.
	private const string url = "http://bmback.us-east-2.elasticbeanstalk.com/api";

	//Examples of GET and POST methods.
	public void Start(){
		//Call method, easy
		/*Debug.Log (GET ("flags"));

		//Set up the POST parameters
		Dictionary<string, string> param = new Dictionary<string, string>();
		param.Add ("flag[claimed]", "1");
		//param.Add ("flag[y]", "100");*/
		/*//Call post method w/ params
		Debug.Log (POST ("flags", param));*/
	}

	//Submits a GET request with the given method.
	//To GET w/ parameters, add 'em yourself with the optional parameter parameter.
	public static string GET(string method, string parameters = "")
	{
		WWW www = new WWW (url + method + parameters);

		//Basically wait
		try { while (!www.isDone) {} }
		catch (UnityException u) {}

		//Return when done
		return www.text; 
	}

	//Submits a POST request with the given Dictionary full of parameter data.
	public static string POST(string method, Dictionary<string,string> post)
	{
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<String,String> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url + method + form);

		//Wait again
		try { while (!www.isDone) {} }
		catch (UnityException u) {}

		return www.text; 
	}
}