using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Manager/BossSelector")]
public class BossSelectorData : ScriptableObject
{
    [SerializeField]
    private List<SceneAsset> _sceneList = new List<SceneAsset>();
    public  List<SceneAsset> SceneList
    {
        get { return _sceneList; }
    }
}
