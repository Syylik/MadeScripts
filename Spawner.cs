using System.Collections;
using UnityEngine;
using UnityEditor;

public class Spawner : MonoBehaviour
{
    public enum SpawnType
    {
        [InspectorName("Спавн в ранд. точке")] Point,
        [InspectorName("Спавн в области")] Area
    }
    public SpawnType spawnType;
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Vector2 spawnSize;
    [SerializeField] private float minSpawnTime = 0.8f, maxSpawnTime = 1.8f;
    public bool canSpawn = false;
    private void Start() => Spawn();
    public void Spawn() => StartCoroutine(SpawnCoroutine()); 
    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        var objectToSpawn = GetRandInArray(objectsToSpawn);
        var spawnPoint = transform.position;
        if(spawnType == SpawnType.Point)
        {
            spawnPoint = GetRandInArray(spawnPoints).position;
        }
        else
        { 
            var xSpawnSize = transform.position.x + Random.Range(-spawnSize.x, spawnSize.x);
            var ySpawnSize = transform.position.y + Random.Range(-spawnSize.y, spawnSize.y);
            spawnPoint = new Vector2(xSpawnSize, ySpawnSize);
        }
        if(canSpawn) Instantiate(objectToSpawn, spawnPoint, objectToSpawn.transform.rotation);
        StartCoroutine(SpawnCoroutine());
    }
    public static T GetRandInArray<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
    public void OnDrawGizmosSelected()
    {
        if(spawnType == SpawnType.Area) Gizmos.DrawWireCube(transform.position, spawnSize);
    }
}
[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Made by Syylik, suck my cock!", MessageType.Warning);
        serializedObject.Update();

        var canSpawn = serializedObject.FindProperty("canSpawn");
        EditorGUILayout.PropertyField(canSpawn, new GUIContent("Спавнить?"));
        if(canSpawn.boolValue)
        {
            var spawnType = serializedObject.FindProperty("spawnType");
            EditorGUILayout.PropertyField(spawnType, new GUIContent("Вид спавна"));

            var objectsToSpawn = serializedObject.FindProperty("objectsToSpawn");
            EditorGUILayout.PropertyField(objectsToSpawn, new GUIContent("Объекты для спавна"));

            switch(spawnType.enumValueIndex)
            {
                case 0:
                    if(!spawnType.hasMultipleDifferentValues)
                    {
                        var spawnPoints = serializedObject.FindProperty("spawnPoints");
                        EditorGUILayout.PropertyField(spawnPoints, new GUIContent("Точки спавна", "Просто перетяни объекты, в которых должны спавниться"));
                    }
                    break;
                case 1:
                    if(!spawnType.hasMultipleDifferentValues)
                    {
                        var spawnSize = serializedObject.FindProperty("spawnSize");
                        EditorGUILayout.PropertyField(spawnSize, new GUIContent("Размер спавна", "Укажи размер квадрата, в ктором будут спавниться объекты"));
                    }
                    break;
            }
            var minSpawnTime = serializedObject.FindProperty("minSpawnTime");
            EditorGUILayout.PropertyField(minSpawnTime, new GUIContent("Мин. время спавна"));

            var maxSpawnTime = serializedObject.FindProperty("maxSpawnTime");
            EditorGUILayout.PropertyField(maxSpawnTime, new GUIContent("Макс. время спавна"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}