//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2019 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;

public class RCC_LogitechInitLoad : MonoBehaviour {

	[InitializeOnLoad]
	public class InitOnLoad {

		static InitOnLoad(){
			
			RCC_SetScriptingSymbol.SetEnabled("RCC_LOGITECH", true);

			if(!EditorPrefs.HasKey("RCC_LOGITECH" + RCC_Version.version.ToString())){
				
				EditorPrefs.SetInt("RCC_LOGITECH" + RCC_Version.version.ToString(), 1);

				if(EditorUtility.DisplayDialog("Logitech Gaming SDK For Realistic Car Controller", "Be sure you have imported latest Logitech Gaming SDK to your project.", "Download", "Close"))
					Application.OpenURL (RCC_AssetPaths.logitech);

			}

		}

	}

}
