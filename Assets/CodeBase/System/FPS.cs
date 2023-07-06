using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    [SerializeField] private TMP_Text _textFPS;
    [SerializeField] private int _maxFps;

    private float _deltaFPS;
    private void Start()
    {
        Application.targetFrameRate = _maxFps;
    }

    private void OnValidate()
    {
        Application.targetFrameRate = _maxFps;
    }

    private void OnGUI()
    {
        _deltaFPS += (Time.deltaTime - _deltaFPS) * 0.1f;
        float fps = 1.0f / _deltaFPS;
        _textFPS.text = Mathf.Ceil (fps).ToString();
    }
}