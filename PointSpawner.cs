using System.Collections;
using UnityEngine;
using UnityEditor;

public class PointSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Transform[] spawnPoints;
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
        var spawnPoint = GetRandInArray(spawnPoints).position;
        if(canSpawn) Instantiate(objectToSpawn, spawnPoint, objectToSpawn.transform.rotation);
        StartCoroutine(SpawnCoroutine());
    }
    public static T GetRandInArray<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
[CustomEditor(typeof(PointSpawner))]
public class PointSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PointSpawner spawner = (PointSpawner)target;
        EditorGUILayout.HelpBox("Made by Syylik, suck my cock!", MessageType.Warning);
        base.OnInspectorGUI();
    }
}