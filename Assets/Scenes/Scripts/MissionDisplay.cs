using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionDisplay : Singleton<MissionDisplay>
{
    [SerializeField] private TextMeshProUGUI _missionText;
    [SerializeField] private List<string> _missions = new List<string>();
    private int selectedMissionIndex = 0;

    private void Start()
    {
        UpdateMissionText();
    }

    public void SetMissionIndex(int index)
    {
        if (index >= 0 && index < _missions.Count)
        {
            selectedMissionIndex = index;
            UpdateMissionText();
        }
        else
        {
            Debug.LogWarning("Invalid mission index selected!");
        }
    }

    private void UpdateMissionText()
    {
        if (_missions.Count > 0 && selectedMissionIndex < _missions.Count)
        {
            _missionText.SetText(_missions[selectedMissionIndex]);
        }
        else
        {
            _missionText.SetText("No mission available");

        }
    }

    public void OnOpenPopup()
    {
        SoundsManager.GetInstance().PlayClip(SoundsManager.Sound.PopUpInteract);
    }
}
