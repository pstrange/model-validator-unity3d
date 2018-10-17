using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeValidatorScript : MonoBehaviour {

	public InputField inputCode;
	public GameObject panel;
	public GameObject panelLogin;

	public void validateCode(){

		if(inputCode.text.Length > 0){
			string code = inputCode.text;
			if (code.Equals ("ABC123")) {
				panel.SetActive (true);
				panelLogin.SetActive (false);
			}
		}

	}
}
