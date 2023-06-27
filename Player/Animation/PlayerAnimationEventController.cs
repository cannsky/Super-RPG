using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PlayerAnimationEventController : MonoBehaviour
{
    #region Necessary Components
    public Transform currentWeapon;//That must be assigned in the start and updated upon weapon change

    Player player;
    PlayerMovement movement;
    PlayerStateController stateController;
    PlayerAttack playerAttack;
    static PlayerAnimationEventController instance;
    public static PlayerAnimationEventController Instance { get=> instance; }

    public GameObject swordSlashParent;
    public GameObject skillVFXParent;
    private List<GameObject> slashes = new List<GameObject>();
    private List<GameObject> vfxs = new List<GameObject>();
    #endregion

    #region Configuration Fields
    Transform currentSwordSlashTransform;
    Transform currentSkillVFXTransform;
    ParticleSystem currentSwordSlash;
    ParticleSystem currentSkillVfx;
    ParticleSystem.EmissionModule emission;

    int slashCount = 0;
    int lastSlashIndex = 0;
    int currentSlashIndex = 0;

    [System.NonSerialized] public bool currentAttackDamageDone = false;
    [System.NonSerialized] public bool currentDamageHalfway = false;
    [System.NonSerialized] public bool currentStandupHalfway = false;
    [System.NonSerialized] public bool currentDodgeHalfway = false;

    [System.NonSerialized] public Skill currentSkill;

    #endregion

    #region Slash Config Fields
    Vector3 pos;
    Vector3 lastPos;
    Vector3 rotation;
    Vector3 lastRotation;
    #endregion

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this, 0.01f);
    }

    private void Start()
    {
        

        player = Player.Instance;
        movement = PlayerMovement.Instance;
        stateController = PlayerStateController.Instance;
        playerAttack = PlayerAttack.Instance;
        lastSlashIndex = slashCount;

        foreach (Transform swordSlash in swordSlashParent.transform) slashes.Add(swordSlash.gameObject);

        foreach (Transform vfx in skillVFXParent.transform) vfxs.Add(vfx.gameObject);

        foreach (GameObject slash in slashes)
        {
            foreach(Transform slashType in slash.transform)
            {
                var particles = slashType.GetComponent<ParticleSystem>();
                particles.Stop();
                var emis = particles.emission;
                emis.enabled = false;
            }
        }

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
    public void PlaySwordSlashes(int attackValue)
    {
        if (gameObject.tag == "Animator2")
            return;

        movement.isRotating = true;
        if(attackValue != 0) currentSwordSlashTransform = Instantiate(slashes[attackValue - 1].transform.GetChild((int)player.element), slashes[attackValue - 1].transform.GetChild((int)player.element).position, slashes[attackValue - 1].transform.GetChild((int)player.element).rotation);
        if (!currentSwordSlashTransform) return;
        currentSwordSlash = currentSwordSlashTransform.GetComponent<ParticleSystem>();
        currentSwordSlash.Play();
        emission = currentSwordSlash.emission;
        emission.enabled = true;
    }

    public void PlaySkillVFX(int skillValue)
    {
        if (gameObject.tag == "Animator2")
            return;

        if (skillValue != 0) currentSkillVFXTransform = Instantiate(vfxs[skillValue - 1].transform.GetChild((int)player.element), vfxs[skillValue - 1].transform.GetChild((int)player.element).position, vfxs[skillValue - 1].transform.GetChild((int)player.element).rotation);
        currentSkillVfx = currentSkillVFXTransform.GetComponent<ParticleSystem>();
        currentSkillVfx.Play();
        emission = currentSkillVfx.emission;
        emission.enabled = true;
    }

    public void StopSwordSlashes()
    {
        if (gameObject.tag == "Animator2")
            return;
        if (!currentSwordSlash)
            return;
        emission = currentSwordSlash.emission;
        emission.enabled = false;
        Destroy(currentSwordSlash.gameObject);
    }

    public void StopSkillVFX()
    {
        if (gameObject.tag == "Animator2")
            return;
        if (!currentSkillVfx)
            return;
        emission = currentSkillVfx.emission;
        emission.enabled = false;
        Destroy(currentSkillVfx.gameObject);
    }

    public void MeleeAttack()
    {
        if (gameObject.tag == "Animator2")
            return;
        playerAttack.Attack(stateController.combo, stateController.runningAttack);
        currentAttackDamageDone = true;
    }

    public void EndAttack()
    {
        if (gameObject.tag == "Animator2")
            return;
        stateController.attack = false;
        currentAttackDamageDone = false;

        playerAttack.TryDropFromQueue();
    }

    public void SkillAttack()
    {
        if (gameObject.tag == "Animator2")
            return;
        playerAttack.SkillAttack();
        currentAttackDamageDone = true;
    }

    public void EndSkillAttack()
    {
        if (gameObject.tag == "Animator2")
            return;
        stateController.attack = false;
        currentSkill = null;
        currentAttackDamageDone = false;
    }

    public void DamageHalfWay()
    {
        if (gameObject.tag == "Animator2")
            return;
        currentDamageHalfway = true;
    }

    public void EndDamage()
    {
        if (gameObject.tag == "Animator2")
            return;
        currentDamageHalfway = false;
        stateController.damage = false;
    }

    public void StandupHalfWay()
    {
        if (gameObject.tag == "Animator2")
            return;
        currentStandupHalfway = true;
    }

    public void EndStandup()
    {
        if (gameObject.tag == "Animator2")
            return;
        currentStandupHalfway = false;
        stateController.knockDownStandup = false;
    }

    public void DodgeHalfway()
    {
        if (gameObject.tag == "Animator2")
            return;
        currentDodgeHalfway = true;
    }

    public void EndDodge()
    {
        if (gameObject.tag == "Animator2")
            return;
        currentDodgeHalfway = false;
        stateController.dodge = false;
    }
}