using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Prefab Category", menuName = "Prefab Library/Prefab Category")]
public class PrefabCategory : ScriptableObject
{
    public List<GameObject> prefabs = new List<GameObject>();
    public Texture2D categoryImage;
    public string categoryImageTitle; // New field for the title
}
