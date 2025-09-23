using UnityEngine;
using System.Collections;

//Code from: https://gist.github.com/mstevenson/5103365 as of 23/9/2025

public class Fps : MonoBehaviour
{
    private float count;
    private float width = 120.0f;
    private float height = 30.0f;
    float x = (Screen.width / 2f) + (Screen.width / 4f);
    float y = 10.0f;

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(x, y, width, height), "FPS: " + Mathf.Round(count));
    }
}