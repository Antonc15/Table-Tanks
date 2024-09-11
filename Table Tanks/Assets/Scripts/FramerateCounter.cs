using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FramerateCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI framerateText;

    private float timer = 0f;
    private int frames = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        frames++;

        if(timer >= 1f)
        {
            framerateText.text = "" + frames;

            timer = 0f;
            frames = 0;
        }
    }
}
