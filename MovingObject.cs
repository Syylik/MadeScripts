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
public class MovingObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MovingObject movingObject = (MovingObject)target;
        EditorGUILayout.HelpBox("Made by Syylik, suck my cock!", MessageType.Warning);
        movingObject.canMove = EditorGUILayout.Toggle("����� ���������?", movingObject.canMove);
        if(movingObject.canMove)
        {
            movingObject.moveSpeed = EditorGUILayout.FloatField("��������", movingObject.moveSpeed);
            movingObject.moveDirection = EditorGUILayout.Vector2Field("�����������", movingObject.moveDirection);
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("��������������� �� ��������?");
        movingObject.canBeDestroyByTime = EditorGUILayout.Toggle(movingObject.canBeDestroyByTime);
        EditorGUILayout.EndHorizontal();
        if(movingObject.canBeDestroyByTime)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("����� �� ���������������");
            movingObject.timeToDestroy = EditorGUILayout.FloatField(movingObject.timeToDestroy);
            EditorGUILayout.EndHorizontal();
        }
    }
}
