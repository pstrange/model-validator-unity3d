using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DialogScript : MonoBehaviour {

	AssetModel model;
	Toggle toggle;

	public void showMessage(AssetModel model, Toggle toggle){
		this.model = model;
		this.toggle = toggle;
		gameObject.SetActive (true);
	}

	public void eventYes(){
		StartCoroutine (requestCheckModel(model.name));
	}

	public void eventNo(){
		gameObject.SetActive (false);
		toggle.isOn = false;
		toggle.interactable = true;
	}

	IEnumerator requestCheckModel(string name)
	{
		WWWForm form = new WWWForm();
		using (UnityWebRequest www = UnityWebRequest.Post("https://assets-validator.herokuapp.com/models/"+name+"/check", form))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("post complete!");
				gameObject.SetActive (false);
				toggle.interactable = false;
			}
		}
	}
}
