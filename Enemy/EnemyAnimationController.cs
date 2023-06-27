using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public EnemyAI enemy;
    public Animator animator;
    public List<ColliderBasedAttack> colliderBasedAttacks = new List<ColliderBasedAttack>();
    public float stopTime = 5f;

    public GameObject skillVFXParent;
    private List<GameObject> vfxs = new List<GameObject>();
    Transform currentSkillVFXTransform;
    ParticleSystem currentSkillVfx;
    ParticleSystem.EmissionModule emission;
    ParticleSystem usedVFX;
    EnemyStats stats;

    private IEnumerator coroutine;

    private CinemachineImpulseSource impulseSource;
    private void Start()
    {
        stats = transform.parent.GetComponent<EnemyStats>();
        impulseSource = transform.GetComponent<CinemachineImpulseSource>();
        foreach (Transform vfx in skillVFXParent.transform) vfxs.Add(vfx.gameObject);
        //if (colliderBasedAttacks != null && colliderBasedAttacks[0] != null) colliderBasedAttacks[0].stats = stats;
        foreach (GameObject vfx in vfxs)
        {
            foreach (Transform vfxType in vfx.transform)
            {
                var particles = vfxType.GetComponent<ParticleSystem>();
                particles.Stop();
                var emis = particles.emission;
                emis.enabled = false;
            }
        }
    }

    public void PlaySkillVFX(int skillValue)
    {
        currentSkillVFXTransform = Instantiate(vfxs[skillValue - 1].transform.GetChild((int)stats.element), vfxs[skillValue - 1].transform.GetChild((int)stats.element).position, vfxs[skillValue - 1].transform.GetChild((int)stats.element).rotation);
        currentSkillVfx = currentSkillVFXTransform.GetComponent<ParticleSystem>();
        currentSkillVfx.Play();
        emission = currentSkillVfx.emission;
        emission.enabled = true;
    }

    public void ThrowProjectile(int skillValue)
    {
        currentSkillVFXTransform = Instantiate(vfxs[skillValue - 1].transform.GetChild((int)stats.element), vfxs[skillValue - 1].transform.GetChild((int)stats.element).position, vfxs[skillValue - 1].transform.GetChild((int)stats.element).rotation);
        currentSkillVfx = currentSkillVFXTransform.GetComponent<ParticleSystem>();
        currentSkillVfx.Play();
        emission = currentSkillVfx.emission;
        emission.enabled = true;
        coroutine = StopProjectile(currentSkillVfx);
        StartCoroutine(coroutine);
    }

    private void CameraShake()
    {
        impulseSource.GenerateImpulse();
    }

    private void AttackWithCameraShake(int attackType)
    {
        impulseSource.GenerateImpulse();
        if (attackType >= 1) ColliderAttack(attackType);
        else stats.TryHit(Player.Instance);
    }

    public void StopSkillVFX()
    {
        if (!currentSkillVfx)
            return;

        Debug.Log(currentSkillVfx);
        emission = currentSkillVfx.emission;
        emission.enabled = false;
        Destroy(currentSkillVfx.gameObject);
    }

    private IEnumerator StopProjectile(ParticleSystem usedVFX)
    {
        yield return new WaitForSeconds(stopTime);
        if (usedVFX)
        {
            emission = usedVFX.emission;
            emission.enabled = false;
            Destroy(usedVFX.gameObject);
        }
    }

    private void Attack(int attackType)
    {
        if (attackType==1) ColliderAttack(attackType);
        else stats.TryHit(Player.Instance);
    }

    private void SecondaryAttack(int attackType)
    {
        //this might include projectiles,so get required data from enemy
        if (attackType==1) ColliderAttack(attackType);
        else stats.TryHit(Player.Instance);
    }

    private void ColliderAttack(int attackType)
    {
        colliderBasedAttacks[attackType - 1].stats = stats;
        colliderBasedAttacks[attackType - 1].Attack();
    }

    private void EndAttack()
    {
        enemy.ResetAnimation();
        Invoke(nameof(ResetAttack), enemy.attackResetTime);
    }

    private void ResetAttack()
    {
        enemy.enemyState.alreadyAttacked = false;
        enemy.enemyState.isAttacking = false;
    }
}