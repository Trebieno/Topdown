using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildVIew : MonoBehaviour
{
    [SerializeField] private Image _indicator;
    [SerializeField] private Image _indicatorChange;
    [SerializeField] private Builder _builder;

    private void Start()
    {
        OffUpdate();
        _builder.OnConstructionStart += PlayUpdate;
        _builder.OnConstructionStop += OffUpdate;
    }

    private void OnDestroy()
    {
        _builder.OnConstructionStart -= PlayUpdate;
        _builder.OnConstructionStop -= OffUpdate;
    }

    public void PlayUpdate(float time)
    {
        if(!_indicator.gameObject.activeSelf)
            _indicator.gameObject.SetActive(true);
        
        _indicatorChange.fillAmount = time / 100;
    }

    public void OffUpdate()
    {
        if(_indicator.gameObject.activeSelf)
            _indicator.gameObject.SetActive(false);
    }
}
