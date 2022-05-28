using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "d13/PrefabCollection", order = 1)]
public class PrefabCollection : ScriptableObject
{
    [SerializeField] private GameObject[] prefabs;

    #region Public
    
    public GameObject GetRandom()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    #endregion
}
