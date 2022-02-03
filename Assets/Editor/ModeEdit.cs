using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ModeEdit : EditorWindow
{
    private static ModeEdit window;

    [MenuItem("Window/Mode Editor")]
    private static void OpenWindow()
    {
        window = (ModeEdit)GetWindow(typeof(ModeEdit));
        window.minSize = new Vector2Int(300, 300);
        window.maxSize = new Vector2Int(1920, 1080);
        window.Show();
    }
    private void OnGUI()
    {
        var setting = new Rect(0, 0, window.position.width, 100);
        GUI.DrawTexture(setting, new Texture2D(1, 1));
        setting = new Rect(window.position.width/2 - 50, 0, 100, 100);
        EditorGUI.DrawRect(setting, Color.red);
    }
    private Button CreateButton(string name)
    {
        var button = new Button();
        button.text = name;
        return button;
    }
}