using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "SceneInfo")]
public class SceneInfo : ScriptableObject
{
    public string Name;
    public string Description;
    public SceneType SceneType;
}
