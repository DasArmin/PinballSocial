﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour
{
    // The text component we will use to display the countdown.
    [SerializeField] private Text countDownText;
    // Amount of time we have until we can play.
    [SerializeField] private int time = 3;
    // The UI that we will disable once the countdown is finished.
    [SerializeField] private GameObject countDownUI;
    // The UI that we will enable once the countdown is finished.
    [SerializeField] private GameObject actionUI;
    // The background of our UI that we will smoothly fade.
    [SerializeField] private Image backgroundImage;
    // Wall that stops us from having a head start.
    [SerializeField] private SpringWall springWall;

    [SerializeField] private CameraFollow cameraFollow;
    private IEnumerator startCountDown;
    private IEnumerator fadeBackground;

    void Start()
    {
        countDownUI.SetActive(true);
        actionUI.SetActive(false);

        fadeBackground = FadeBackground(backgroundImage.color, 0.025f);
        startCountDown = StartCountDown();

        StartCoroutine(fadeBackground);
        StartCoroutine(startCountDown);
    }

    IEnumerator StartCountDown()
    {
        while (true)
        {
            time--;
            SetCountdownText();
            yield return new WaitForSeconds(1);
        }
    }

    void SetCountdownText()
    {
        switch(time)
        {
            case 2:
                StartCoroutine(springWall.lowerWall);
                countDownText.text = time.ToString();
                break;
            case 1:
                countDownText.text = time.ToString();
                cameraFollow.IsWaiting = false;
                break;
            case 0:
                countDownText.text = "Start!";
                actionUI.SetActive(true);
                break;
            case -1:
                StopCoroutine(startCountDown);
                countDownUI.SetActive(false);
                break;
            default:
                countDownText.text = time.ToString();
                break;
        }
    }

    IEnumerator FadeBackground(Color imageColor, float fadeTime)
    {
        while (true)
        {
            // Keep fading the image until the alpha is 0.
            if (imageColor.a >= 0)
            {
                imageColor.a -= fadeTime;
                backgroundImage.color = imageColor;
            }

            // Stop the function once we're done fading.
            else if (imageColor.a <= 0)
                StopCoroutine(fadeBackground);

            yield return new WaitForSeconds(0.05f);
        }
    }
}
