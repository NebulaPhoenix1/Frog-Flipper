using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class PlayerPrefsEditor :  EditorWindow
{ 
    float highscore = 0f;


    [MenuItem("Edit/Player Preferences")]
    public static void openWindow()
    {
        PlayerPrefsEditor window = (PlayerPrefsEditor)EditorWindow.GetWindow(typeof(PlayerPrefsEditor));
        window.titleContent = new GUIContent("Player Preferences");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Player Preferences Editor", EditorStyles.boldLabel);
        if (GUILayout.Button("Clear All Player Preferences"))
        {
            if (EditorUtility.DisplayDialog("Confirm Clear", "Are you sure you want to clear all Player Preferences?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("All Player Preferences have been cleared.");
            }
        }

        highscore = EditorGUILayout.FloatField("Highscore", highscore);
        if (GUILayout.Button("Save Highscore"))
        {
            PlayerPrefs.SetFloat("Highscore", highscore);
        }
    }

    void OnEnable()
    {
        highscore = PlayerPrefs.GetFloat("Highscore", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
