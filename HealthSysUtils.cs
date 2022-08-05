using UnityEngine;

namespace HealthSys
{
    [AddComponentMenu("HealthSys/HealthUtils"), RequireComponent(typeof(HealthSys.HealthSystem))]
    public class HealthSysUtils : MonoBehaviour
    {
        private Animator anim;
        private void Awake()
        {
            if(TryGetComponent<Animator>(out Animator animator)) anim = animator;
        }
        public void PlayAnim(string triggerName) => anim.SetTrigger(triggerName);
        public void InstantiateEffect(GameObject effect)
        {
            var newEffect = Instantiate(effect, transform.position, effect.transform.rotation);
            Destroy(newEffect, 10f);
        }
        public void PlaySound(AudioSource sound) => sound.Play();
    }
}