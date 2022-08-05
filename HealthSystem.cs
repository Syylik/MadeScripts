using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

namespace HealthSys
{
    [AddComponentMenu("HealthSys/HealthSystem")]
    public class HealthSystem : MonoBehaviour
    {
        public bool isImmortal = false;
        
        public float health = 10;
        public float maxHealth = 10;
        
        [Tooltip("Уменьшение урона в процентах")] [Range(0f, 100f)] public float shield;

        public bool hasHealthBar = true;
        public Image healthBar;
        
        public float invinsibleTime = 0.15f;
        private bool canTakeHit = true;


        [System.Serializable]
        public struct Events
        {
            public UnityEvent OnTakeHit;
            public UnityEvent OnHeal;
            public UnityEvent OnDie;
            public UnityEvent OnReduceMaxHealth;
            public UnityEvent OnAddMaxHealth;
        }    
        public Events events;
        private void Start() => UpdateHealthBar();
        public void TakeHit(float damage)
        {
            if(!isImmortal && canTakeHit)
            {
                var shieldedDamage = (damage * shield) / 100;
                health -= damage - shieldedDamage;
                StartCoroutine(InvinsibleTime());
                health = Mathf.Clamp(health, 0, maxHealth);
                UpdateHealthBar();
                events.OnTakeHit?.Invoke();
                if(health <= 0) Die();
            }
        }
        private void OnValidate() => UpdateHealthBar();
        public void UpdateHealthBar()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            maxHealth = Mathf.Clamp(maxHealth, 0, float.MaxValue);
            if(hasHealthBar && healthBar != null)
            { 
                healthBar.fillAmount = health / maxHealth;
            }
        }
        public void Heal(float healPoint)
        {
            health += healPoint;
            UpdateHealthBar();
            events.OnHeal?.Invoke();
            health = Mathf.Clamp(health, 0, maxHealth);
        }
        public void AddMaxHealth(float addValue)
        {
            maxHealth += addValue;
            maxHealth = Mathf.Clamp(maxHealth, 0, float.MaxValue);
            UpdateHealthBar();
            events.OnAddMaxHealth?.Invoke();
        }
        public void ReduceMaxHealth(float reduceValue)
        {
            maxHealth += reduceValue;
            maxHealth = Mathf.Clamp(maxHealth, 0, float.MaxValue);
            UpdateHealthBar();
            events.OnReduceMaxHealth?.Invoke();
        }
        public void Die()
        {
            events.OnDie?.Invoke();
            Destroy(gameObject);
        }
        private IEnumerator InvinsibleTime()
        {
            canTakeHit = false;
            yield return new WaitForSeconds(invinsibleTime);
            canTakeHit = true;
        }
    }

    [CustomEditor(typeof(HealthSystem)), CanEditMultipleObjects]
    public class HealthSystemEditor : Editor
    {
        public SerializedProperty isImmortal;
        public SerializedProperty health;
        public SerializedProperty maxHealth;
        public SerializedProperty shield;
        public SerializedProperty hasHealthBar;
        public SerializedProperty healthBar;
        public SerializedProperty invinsibleTime;
        public SerializedProperty events;
        private void OnEnable()
        {
            isImmortal = serializedObject.FindProperty(nameof(isImmortal));
            health = serializedObject.FindProperty(nameof(health));
            maxHealth = serializedObject.FindProperty(nameof(maxHealth));
            shield = serializedObject.FindProperty(nameof(shield));
            hasHealthBar = serializedObject.FindProperty(nameof(hasHealthBar));
            healthBar = serializedObject.FindProperty(nameof(healthBar));
            invinsibleTime = serializedObject.FindProperty(nameof(invinsibleTime));
            events = serializedObject.FindProperty(nameof(events));
        }
        public override void OnInspectorGUI()
        {
            HealthSystem healthSystem = (HealthSystem)target;
            serializedObject.Update();

            DrawVariables();

            serializedObject.ApplyModifiedProperties();
        }
        private void DrawVariables()
        {
            EditorGUILayout.PropertyField(isImmortal, new GUIContent("Бессмертен?"));
            if(!isImmortal.boolValue)
            {
                EditorGUILayout.PropertyField(health, new GUIContent("Здоровье"));
                EditorGUILayout.PropertyField(maxHealth, new GUIContent("Макс. здоровье"));
                EditorGUILayout.PropertyField(shield, new GUIContent("Щит%"));
                EditorGUILayout.PropertyField(hasHealthBar, new GUIContent("Есть полоска хп?"));
                if(hasHealthBar.boolValue) EditorGUILayout.PropertyField(healthBar, new GUIContent("Полоска хп"));
                EditorGUILayout.PropertyField(invinsibleTime, new GUIContent("Время бессмертия"));
            }
            EditorGUILayout.PropertyField(events, new GUIContent("События"));
        }
    }
}