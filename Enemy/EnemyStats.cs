using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : Selector
{
    public StatFields stats;
    public EnemyAI self;
    [SerializeField] float minTreshhold = -3f;
    [SerializeField] float maxTreshold = 3f;
    [SerializeField] int frameCount = 60;
    [SerializeField] float deathHeight = 3f;
    float treshold;
    public override float MovementMultiplier
    {
        get => base.MovementMultiplier;
        set
        {
            base.MovementMultiplier = value;
            self.SpeedMultiplier = value;
        }
    }

    public override void Start()
    {
        if(self == null) self = GetComponent<GolemAI>();
        allStats = stats;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        StartCoroutine(DeathAnimation());
    }

    protected override IEnumerator DeathAnimation()
    {
        gameObject.GetComponent<ExperienceGiver>().GiveExperience();
        self.animator.SetBool("Death", true);
        self.animator.SetBool("Walk", false);
        self.animator.SetBool("Attack", false);
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<ExperienceGiver>().enabled = false;
        foreach (var collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
        GetComponent<NavMeshAgent>().enabled = false;
        self.animator.SetBool("Walk Backwards", false);
        tracer.UnloadCompletely();
        yield return new WaitForSeconds(deathTime);
        StartCoroutine(DeathEffect());
    }

    protected override IEnumerator DeathEffect()
    {
        meshRenderer.materials = new Material[] { deathMaterial };
        treshold = maxTreshold;
        float decrementAmount = (maxTreshold - minTreshhold)/(float)frameCount;
        float deltaSeconds = deathEffectTime / (float)frameCount;
        float dropAmount = deathHeight / (float)frameCount;
        Vector3 drop = new Vector3(0, -dropAmount, 0);
        
        for (int i = 0; i < frameCount; i++)
        {
            transform.position += drop;
            treshold -= decrementAmount;
            deathMaterial.SetFloat("_threshold", treshold);
            yield return new WaitForSeconds(deltaSeconds);
        }
        Destruction();
    }

    protected override void Destruction()
    {
        Destroy(gameObject);
    }
}
