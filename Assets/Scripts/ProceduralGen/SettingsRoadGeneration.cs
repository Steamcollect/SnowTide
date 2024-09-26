using UnityEngine;

[CreateAssetMenu(fileName = "SCO_SettingsRoadGen", menuName = "SCO/SettingsRoadGeneration")]
public class SettingsRoadGeneration : ScriptableObject
{
    [field:Header("Chunks Road")]
    [field: SerializeField][field: Min(1)]public int chunksVisibe { get; private set; } = 1;
}
