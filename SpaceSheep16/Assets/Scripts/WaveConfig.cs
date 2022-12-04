using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{

    [SerializeField] public GameObject enemyPrefab01;
    [SerializeField] public GameObject enemyPrefab02;
    [SerializeField] public GameObject enemyPrefab03;
    [SerializeField] public GameObject enemyPrefab04;
    [SerializeField] public GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    public GameObject GetEnemyPrefab(EnemyPrefabType enemyPrefabType)
    {
        switch (enemyPrefabType)
        {
            case EnemyPrefabType.WhiteSquare:
                return enemyPrefab01;
            break;
            case EnemyPrefabType.ColoredSquare:
                return enemyPrefab02;
            break;
            case EnemyPrefabType.BasicSprite:
                return enemyPrefab03;
            break;
            case EnemyPrefabType.PersonalizedSprites:
                return enemyPrefab04;
            break;
            default:
                return enemyPrefab04;
            break;
        }
        
    }

    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }

        return waveWaypoints;
    }

    public float GetTimeBetweenSpawns()    { return timeBetweenSpawns; }

    public float GetSpawnRandomFactor() { return spawnRandomFactor; }

    public int GetNumberOfEnemies()     { return numberOfEnemies; }

    public float GetMoveSpeed()         { return moveSpeed; }

}

public enum EnemyPrefabType
{
    WhiteSquare,
    ColoredSquare,
    BasicSprite,
    PersonalizedSprites
}
