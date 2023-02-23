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
    [SerializeField] public GameObject enemyPrefab05;
    [SerializeField] public GameObject enemyPrefab06;
    [SerializeField] public GameObject enemyPrefab07;
    [SerializeField] public GameObject enemyPrefab08;
    [SerializeField] public GameObject enemyPrefab09;
    [SerializeField] public GameObject enemyPrefab10;
    [SerializeField] public GameObject enemyPrefab11;
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
            case EnemyPrefabType.ScaleUpOnDestroy:
                return enemyPrefab04;
                break;
            case EnemyPrefabType.ChangeColorOnHitEnemy:
                return enemyPrefab05;
                break;
            case EnemyPrefabType.BounceOnHit:
                return enemyPrefab06;
                break;
            case EnemyPrefabType.ShowVFX:
                return enemyPrefab07;
                break;
            case EnemyPrefabType.ShakeCamera:
                return enemyPrefab08;
                break;
            case EnemyPrefabType.DrySquare:
                return enemyPrefab09;
            break;
            case EnemyPrefabType.JuicySquare:
                return enemyPrefab10;
            break;
            case EnemyPrefabType.PersonalizedSprites:
                return enemyPrefab11;
            default:
                return enemyPrefab11;
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
    ScaleUpOnDestroy,
    ChangeColorOnHitEnemy,
    BounceOnHit,
    ShowVFX,
    ShakeCamera,
    DrySquare,
    JuicySquare,
    PersonalizedSprites
}
