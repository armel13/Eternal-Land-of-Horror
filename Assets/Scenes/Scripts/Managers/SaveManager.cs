using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private int _savedDuration = 60;
    [SerializeField] private float _savedVolumeValue = 0.5f;
    [SerializeField] private bool _isLevelStartTransitionOn = true;

    public float SavedVolumeValue
    {
        get { return _savedVolumeValue; }
        set
        {
            if (value > 1) { value = 1; }
            if (value < 0) { value = 0; }

            _savedVolumeValue = value;
        }
    }
    public int SavedDuration => _savedDuration;
    public bool IsLevelStartTransitionOn => _isLevelStartTransitionOn;

    public void UpdateSavedDuration(int newValue)
    {
        _savedDuration = newValue;
    }

    public void ChangeIsLevelStartTransitionOnValue(bool newValue)
    {
        _isLevelStartTransitionOn = newValue;
    }
}
