
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/New Level", order = 1)]
public class Level : ScriptableObject
{
    [Header("Total box count/gray box")]
    public int boxTotalCount;
    [Header("level prefab")]
    public GameObject prefab;
    [Header("platform's corner material")]
    public Material platformBlock;
    [Header("platform base material")]
    public Material platform;
    [Header("levelid")]
    public int levelId;
}
