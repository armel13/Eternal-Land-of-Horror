using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    [SerializeField] private TextMeshProUGUI _timerText;

    public bool stopTimer = false;
    private float _totalDurations;
    [SerializeField] private float _countdownTime = 360;
    [SerializeField] private bool _isGetDurationFromSaveManager = false;

    public float CountdownTime => _countdownTime;

    public void StartTimer()
    {
        stopTimer = false;
        UpdateTimerText();
    }

    private void Start()
    {
        _totalDurations = _countdownTime;
        //stopTimer = false;
        if (_isGetDurationFromSaveManager) _countdownTime = ((SaveManager.GetInstance() != null) ? SaveManager.GetInstance().SavedDuration : 60);
        UpdateTimerText();
    }

    public void OnResetTimer()
    {
        _countdownTime = _totalDurations;
        stopTimer = false;
        UpdateTimerText();
    }

    void Update()
    {
        if (stopTimer) return;

        if (_countdownTime > 0)
        {
            _countdownTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            UIManager.instance.ShowLosePanel();
            Debug.Log("Timer expired!");
            _countdownTime = 0;
        }
    }

    public void OnSetStopTimerValue(bool value)
    {
        stopTimer = value;
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_countdownTime / 60);
        int seconds = Mathf.FloorToInt(_countdownTime % 60);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

        _timerText.SetText(timerString);
    }
}
