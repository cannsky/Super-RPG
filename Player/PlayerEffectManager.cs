using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : EffectManager
{
    Player playerStats;
    public GameObject playerVfxs;
    List<GameObject> vfxs = new List<GameObject>();
    [System.NonSerialized] public ParticleSystem currentVFX;
    [System.NonSerialized] public ParticleSystem usedVFX;
    Queue usedVfxs = new Queue();
    [System.NonSerialized] public Transform currentVFXTransform;
    ParticleSystem.EmissionModule emission;
    IEnumerator coroutine;

    static PlayerEffectManager instance;
    public static PlayerEffectManager Instance { get => instance; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    protected override void Start()
    {
        base.Start();
        foreach (Transform vfx in playerVfxs.transform) vfxs.Add(vfx.gameObject);

        foreach (GameObject vfx in vfxs)
        {
            foreach (Transform vfxEffect in vfx.transform)
            {
                var particles = vfxEffect.GetComponent<ParticleSystem>();
                particles.Stop();
                var emis = particles.emission;
                emis.enabled = false;
            }
        }
    }

    public void PlayEffectVFX(int vfxValue, float stopTime)
    {
        if (vfxValue < 4) currentVFXTransform = Instantiate(vfxs[vfxValue].transform.GetChild(0), vfxs[vfxValue].transform.GetChild(0).position, vfxs[vfxValue].transform.GetChild(0).rotation);
        else return;
        currentVFX = currentVFXTransform.GetComponent<ParticleSystem>();
        currentVFX.Play();
        emission = currentVFX.emission;
        emission.enabled = true;
        usedVfxs.Enqueue(currentVFX);
        coroutine = StopEffectVFX(stopTime);
        StartCoroutine(coroutine);
    }

    public IEnumerator StopEffectVFX(float stopTime)
    {
        yield return new WaitForSeconds(stopTime);
        if(usedVfxs.Count != 0)
        {
            usedVFX = (ParticleSystem)(usedVfxs.Dequeue());
            emission = usedVFX.emission;
            emission.enabled = false;
            Destroy(usedVFX.gameObject);
        }
    }

    public override void AddEffect(string skillName, BuffDebuffStatTypes buffDebuffType, int change, float time)
    {
        //base.AddEffect(skillName, buffDebuffType, change, time);
        PlayEffectVFX(0, 2f);
    }

    public override void AddEffect(string skillName, DamageStatTypes damageType, int damage, int time)
    {
        base.AddEffect(skillName, damageType, damage, time);
    }

    public override void AddEffect(string skillName, float movementChange, float time)
    {
        //base.AddEffect(skillName, movementChange, time);
        PlayEffectVFX(2, 2f);
    }

    public override void AddEffect(string skillName, RegenerationStatTypes regenType, int regeneration, int time)
    {
        base.AddEffect(skillName, regenType, regeneration, time);
    }

    public override void Heal(int regen)
    {
        PlayEffectVFX(1, 2f);
        base.Heal(regen);
    }

    public override void HealEnergy(int regen)
    {
        PlayEffectVFX(3, 2f);
        base.Heal(regen);
    }

}
