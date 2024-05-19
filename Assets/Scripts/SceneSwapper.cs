using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneSwapper : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject environment;
    bool curScene;

    public void SwitchScene()
    {
        curScene = !curScene;

        background.SetActive(curScene);
        environment.SetActive(!curScene);   

    }
}

[CustomEditor(typeof(SceneSwapper))]
public class SceneSwapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Switch Scene"))
        {
            (target as SceneSwapper).SwitchScene();
        }
    }
}