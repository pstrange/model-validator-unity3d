using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetsBundleScript : Editor 
{

	[MenuItem("Assets/ Build AssetBundles")]
	static void BuildAssetBundles()
	{
		BuildPipeline.BuildAssetBundles (@"C:\AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
	}

}
