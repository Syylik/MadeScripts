using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Collider2D))]
public class MovingObject : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Vector2 moveDirection;
    public bool canMove = false;
    public bool canBeDestroyByTime = false;
    public float timeToDestroy = 2f;
    private void Start()
    {
        if(canBeDestroyByTime) Destroy(gameObject, timeToDestroy);
    }
    private void Update()
    {
        if(canMove) transform.Translate(moveSpeed * moveDirection * Time.deltaTime);
    }

}

[CustomEditor(typeof(MovingObject))]
[CanEditMultipleObjects]
public class MovingObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var canMove = serializedObject.FindProperty("canMove");
        EditorGUILayout.PropertyField(canMove, new GUIContent("Может двигаться?"));
        if(canMove.boolValue)
        {
            var moveSpeed = serializedObject.FindProperty("moveSpeed");
            EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Скорость"));
            var moveDir = serializedObject.FindProperty("moveDirection");
            EditorGUILayout.PropertyField(moveDir, new GUIContent("Направление"));
        }
        EditorGUILayout.BeginHorizontal();
        var canBeDestroyByTime = serializedObject.FindProperty("canBeDestroyByTime");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(canBeDestroyByTime, new GUIContent("Уничтожится со временем?"));
        EditorGUILayout.Space(10);
        if(canBeDestroyByTime.boolValue)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            var destroyTime = serializedObject.FindProperty("timeToDestroy");
            EditorGUILayout.PropertyField(destroyTime, new GUIContent("Время до самоуничтожения"));
            EditorGUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }
}