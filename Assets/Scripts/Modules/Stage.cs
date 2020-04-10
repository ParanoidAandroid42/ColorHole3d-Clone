using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage/New Stage", order = 1)]
public class Stage : ScriptableObject
{
    [Header("level list")]
    public List<Level> levels;
    [Header("door metarial")]
    public Material doorMetarial;
    [Header("tube metarial")]
    public Material tubeMetarial;
}
