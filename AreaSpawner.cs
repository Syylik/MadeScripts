using System.Collections;
using UnityEngine;
using UnityEditor;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Vector2 spawnSize;
    [SerializeField] private float minSpawnTime = 0.8f, maxSpawnTime = 1.8f;
    public bool spawnInfinity = true;
    public bool canSpawn = true;
    private void Start()
    {
        if(spawnInfinity) Spawn();
    }
    public void Spawn() => StartCoroutine(SpawnCoroutine()); 
    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        var objectToSpawn = GetRandInArray(objectsToSpawn);
        var xSpawnSize = transform.position.x + Random.Range(-spawnSize.x, spawnSize.x);
        var ySpawnSize = transform.position.y + Random.Range(-spawnSize.y, spawnSize.y);
        var spawnPoint = new Vector2(xSpawnSize, ySpawnSize);
        if(canSpawn) Instantiate(objectToSpawn, spawnPoint, objectToSpawn.transform.rotation);
        StartCoroutine(SpawnCoroutine());
    }
    public static T GetRandInArray<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }
}
[CustomEditor(typeof(AreaSpawner))]
public class AreaSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AreaSpawner spawner = (AreaSpawner)target;
        EditorGUILayout.HelpBox("Made by Syylik, suck my cock!", MessageType.Warning);
        base.OnInspectorGUI();
    }
}