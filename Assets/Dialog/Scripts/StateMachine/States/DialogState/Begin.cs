using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Begin : MonoBehaviour, IState
{
    [Header("Owner Settings")]
    public Canvas Owner;
    public GameObject DialogPanel;
    public CanvasGroup CanvasGroup;        // CanvasGroup allow to fade in/out evething in the canvas

    [Header("Target Settings")]
    public Vector3 PlacementOffset; 
    public bool FollowTarget;

    [Header("Display Settings")]
    public float BubbleDisplayDelay;      // how long does it take to this bubble to show something
    public float BubbleDisplayTime;       // how long does the bubble show something
    public bool NeverEnd;                 // ignore the displaytime and never end showing the bubble
    public bool WaitForCommand;           // ignore the displaytime and end the bubble when it receive a command
    public KeyCode Command;               // the command that will change the bubble text or end it

    public bool EnableFadeIn;
    public bool EnableFadeOut;
    public float FadeInSpeed;
    public float FadeOutSpeed;

    // help with the parallax effect - Require FollowTarget
    [Header("Rotation Settings (Require Follow Target): TO DO")]
    public bool EnableRotation;         // allows the bubble to rotate wherever a gameobject is
    public GameObject RotateTowards;    // the gameObject the camera will follow
    public float RotateSpeed;

    [Header("Event Settings : TO DO")]
    // criar um evento pra quando terminar
    public Event FinishedFadeOut; // ?????????? se o fadeOut tiver setado -- se for fadeOut vai para um estado se nunca terminar ele vai pra ou outro estado diferente
    public Event FinishedNoEnd; // ?????????? se o neverEnd estiver setado

    // estados que ele iria quando entrasse nos eventos
    public IState Faded;
    public IState Neverend;

    [HideInInspector]
    public GameObject Target;

    public void Enter()
    {
        Owner.transform.position = Target.transform.position + PlacementOffset;

        // If followPlayer is true -. set the canvas as the target child, else apper in the target position only
        if (FollowTarget)
        {
            Owner.transform.parent = Target.transform;
        }

    }

    public void Execute()
    {
        // fade-In and fade-out
        waitTime(BubbleDisplayDelay, () =>
        {
            if (EnableFadeIn)
            {
                StartCoroutine("fadeIn");
            }
            else
            {
                CanvasGroup.alpha = 1;
            }

            waitTime(BubbleDisplayTime, () =>
            {
                if (!NeverEnd && !WaitForCommand)
                {
                    if (EnableFadeOut)
                    {
                        StartCoroutine("fadeOut");
                    }
                    else
                    {
                        CanvasGroup.alpha = 0;
                    }
                }

                if (WaitForCommand)
                {
                    waitCommand(() =>
                    {
                        if (EnableFadeOut)
                        {
                            StartCoroutine("fadeOut");
                        }
                        else
                        {
                            CanvasGroup.alpha = 0;
                            Exit();
                        }
                    });
                }
            });
        });

        // FALTA FAZER A ROTAÇÃO LEMBRANDO DE ATUALIZAR A CADA FRAME
    }

    private void waitTime(float seconds, Action action)
    {
        StartCoroutine(wait(seconds, action));
    }

    private void waitCommand(Action action)
    {
        StartCoroutine(CheckCommand(action));
    }


    public void Exit()
    {
        gameObject.transform.parent = null;
        ManageText mt = this.GetComponent<ManageText>();
        mt.endedBubble = true;
    }

    IEnumerator CheckCommand(Action callback)
    {
        while (!Input.GetKey(Command))
        {
            yield return new WaitForEndOfFrame();
        }
        callback();
    }

    IEnumerator wait(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }

    IEnumerator fadeIn()
    {
        float alpha = CanvasGroup.alpha;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / FadeInSpeed)
        {
            alpha = Mathf.Lerp(alpha, 1, t);
            CanvasGroup.alpha = alpha;
            yield return null;
        }
    }

    IEnumerator fadeOut()
    {
        float alpha = CanvasGroup.alpha;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / FadeOutSpeed)
        {
            alpha = Mathf.Lerp(alpha, 0, t);
            CanvasGroup.alpha = alpha;
            yield return null;
        }
        Exit();
    }
}
