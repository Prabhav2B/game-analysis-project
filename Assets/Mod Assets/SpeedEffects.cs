﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using KartGame.KartSystems;
using KartGame;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;


public class SpeedEffects : MonoBehaviour
{

    [SerializeField] private Rigidbody playerRb;
    [Space(10)]
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float maxFOV;
    [SerializeField] private GameObject speedlines;
    [SerializeField] private PostProcessVolume volume;

    private float minFOV;
    private float currFOV;
    private float maxSpeed;
    private float speed;
    bool zoomedOut;
    ChromaticAberration ca;

    private void Start()
    {
        maxSpeed = playerRb.gameObject.GetComponent<ArcadeKart>().baseStats.TopSpeed;
        minFOV = vCam.m_Lens.FieldOfView;
        zoomedOut = false;
        speedlines.SetActive(false);


        if (volume.profile.TryGetSettings<ChromaticAberration>(out ca))
        {
            ca.intensity.overrideState = true;
            ca.intensity.value = 0f;
        }
    }

    private void Update()
    {
        if (playerRb.velocity.magnitude > .9 * maxSpeed && !zoomedOut)
        {
            ZoomOut();
            zoomedOut = true;
            
        }
        else if (playerRb.velocity.magnitude < .75 * maxSpeed && zoomedOut)
        {
            ZoomIn();
            zoomedOut = false;
        }
    }

    void ZoomOut()
    {
        currFOV = vCam.m_Lens.FieldOfView;

       DOTween.To(() => vCam.m_Lens.FieldOfView, x => vCam.m_Lens.FieldOfView = x, maxFOV, 1f);
        speedlines.SetActive(true);

        DOTween.To(() => ca.intensity.value, x => ca.intensity.value = x, 1f, 1f);
    }

    void ZoomIn()
    {
        currFOV = vCam.m_Lens.FieldOfView;

        DOTween.To(() => vCam.m_Lens.FieldOfView, x => vCam.m_Lens.FieldOfView = x, minFOV, 1f);
        speedlines.SetActive(false);

        DOTween.To(() => ca.intensity.value, x => ca.intensity.value = x, 0f, 1f);

    }



}