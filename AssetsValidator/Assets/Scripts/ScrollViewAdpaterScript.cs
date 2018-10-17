using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class ScrollViewAdpaterScript : MonoBehaviour 
{

	public AssetModelList assetList;
	public RectTransform prefab;
	public ScrollRect scrollView;
	public RectTransform content;
	public GameObject gObject;
	public AssetBundle bundle;
	public GameObject panelDetail;
	public GameObject dialog;
	public string SERVER_URL;

	List<ItemView> views = new List<ItemView>();
	// Use this for initialization
	void Start () 
	{
		UpdateItems ();
	}
	
	// Update is called once per frame
	public void UpdateItems () 
	{
		StartCoroutine (GetJSONBundle (SERVER_URL, results => OnReceivedNewModels(results)));
	}

	void OnReceivedNewModels(AssetModelList assetList)
	{
		foreach (Transform child in content.transform)
			Destroy (child.gameObject);

		views.Clear ();

		foreach (AssetModel model in assetList.assets)
		{
			GameObject instance = GameObject.Instantiate (prefab.gameObject) as GameObject;
			instance.transform.SetParent (content, false);
			var view = InitializeItemView (instance, model);
			views.Add (view);
		}
	}

	IEnumerator GetJSONBundle(string url, Action<AssetModelList> onDone) 
	{
		Debug.Log(url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		} else {
			// Show results as text
			// Debug.Log(www.downloadHandler.text);
			assetList = JsonUtility.FromJson<AssetModelList>(www.downloadHandler.text);

			onDone (assetList);
		}
	}

	ItemView InitializeItemView(GameObject viewGameObject, AssetModel model)
	{
		ItemView view = new ItemView (viewGameObject.transform);
		view.title.text = model.name;
		view.toggle.isOn = model.check;
		StartCoroutine (loadSpriteImageFromUrl (model.url, view.imageIcon));
		if (!model.check) {
			view.toggle.onValueChanged.AddListener (delegate {
				TaskForModelToggle (model, view.toggle);
			});
		} else
			view.toggle.interactable = false;
		viewGameObject.GetComponent<Button>().onClick.AddListener (delegate {
			TaskForModel(model);
		});
		return view;
	}

	void TaskForModelToggle(AssetModel model, Toggle toggle)
	{
		Debug.Log(model.name);
		//hacer yo el manejo del dialog y paso de parametros con un gameobject dialod
		dialog.GetComponent<DialogScript> ().showMessage(model, toggle);
	}

	void TaskForModel(AssetModel model)
	{
		Debug.Log(model.name);
		StartCoroutine (GetAssetBundle (model.name, model.url));
	}

	public class ItemView
	{
		public Text title;
		public RawImage imageIcon;
		public Toggle toggle;

		public ItemView(Transform rootView)
		{
			title = rootView.Find("TitleText").GetComponent<Text>();
			imageIcon = rootView.Find("ImageIcon").GetComponent<RawImage>();
			toggle = rootView.Find("Toggle").GetComponent<Toggle>();
		}
	}

	IEnumerator GetAssetBundle(string name, string url) 
	{
		if (gObject != null) {
			bundle.Unload (true);
			Destroy (gObject);
		}

		UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			bundle = DownloadHandlerAssetBundle.GetContent(www);
			gObject = Instantiate (bundle.LoadAsset(name)) as GameObject;
			gObject.AddComponent<RotationAssetsScript> ().startPosition = GameObject.FindGameObjectWithTag("StartPosition");
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraZoomScript> ().resetCamera();
		}
	}

	IEnumerator loadSpriteImageFromUrl(string URL, RawImage imageIcon)
	{
		WWW www = new WWW(URL);
		yield return www;

		Texture2D text2d = www.texture;
		imageIcon.texture = text2d;
	}
}
