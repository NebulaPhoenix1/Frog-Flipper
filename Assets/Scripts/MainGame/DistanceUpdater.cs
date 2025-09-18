using UnityEngine;
using TMPro;
using System.Collections;
public class DistanceUpdater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameObject player;
    private TMP_Text text;
    private float oldDist = 0.0f;
    void Start()
    {
        player = GameObject.Find("Player");
        text = GetComponent<TMP_Text>();
    }


    public void UpdateDistance()
    {
        // Count up from old distance to player's current height
        float dist = Mathf.Round(player.transform.position.y);
        StartCoroutine(countUp(dist));

    }

    private IEnumerator countUp(float target)
    {
        while (oldDist < target)
        {
            oldDist += 1.0f;
            text.text = oldDist.ToString() + "m";
            yield return new WaitForSeconds(0.05f);
        }
    }

}
