using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointList : Singleton<CheckpointList>
{
    public List<Transform> CheckpointListTransform = new();
}
