using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Healing _healing;

    private void Start()
    {
        _healing.HealthChanged += SliderUpdate;
    }

    public void SliderUpdate(float current, float maximum)
    {
        _slider.maxValue = maximum;
        _slider.value = current;
    }
}
