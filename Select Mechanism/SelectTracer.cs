using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SelectTracer : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] Canvas entityCanvas;
    [SerializeField] Image entitySprite;
    [SerializeField] TMP_Text header;
    [SerializeField] Image healthFill;
    [SerializeField] Sprite NPCHealthFillSprite;
    [SerializeField] Sprite mobHealthFillSprite;
    [SerializeField] TMP_Text levelInfo;
    [SerializeField] TMP_Text distanceInfo;
    public float maxSelectDistance = 15f;
    #endregion

    #region Cached Fields
    [NonSerialized] new public bool enabled;
    Selector currentSelector;
    Clicker clicker;
    Player player;
    bool isInDistance;
    #endregion

    static SelectTracer instance;
    public static SelectTracer Instance { get => instance; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    private void Start()
    {
        player = Player.Instance;
        clicker = Clicker.Instance;
        UnloadCompletely();
    }

    private void Update()
    {
        if (!currentSelector)
        {
            if(enabled)
                UnloadCompletely();
            return;
        }            

        //nonserialized public float;
        float distance = Helper.CalculateDistance(currentSelector.transform.position, player.transform.position);

        distanceInfo.text = (distance <= player.interactionDistance + Mathf.Epsilon) ? "Nearby" : System.Math.Round(distance, 2) + " meters";

        isInDistance = distance <= maxSelectDistance;

        Selector.lastSelectDistance = distance;

        if (!isInDistance)
            UnloadCompletely();
    }


    public void Load(Selector selector)
    {
        if (!enabled)
            EnableWindow();

        currentSelector = selector;
        header.text = selector.name;
        entitySprite.sprite = selector?.picture;

        player.targetStats = selector;

        if (selector is EnemyStats)
        {
            EnemyStats enemy = (EnemyStats)selector;
            enemy.healthChange += UpdateHealth;
            levelInfo.text = "Level " + selector.GetComponent<ExperienceGiver>().level;
            UpdateHealth(((float)enemy.allStats.health),((float)enemy.allStats.currentHealth));
            healthFill.sprite = mobHealthFillSprite;
        }
        else
        {
            levelInfo.text = selector.extraInfo;
            UpdateHealth(1,1);
            healthFill.sprite = NPCHealthFillSprite;
        }
    }

    private void UpdateHealth(float totalHealth,float currentHealth)
    {
        float ratio = 0;
        try
        {
            ratio = currentHealth / totalHealth;
        }
        catch (Exception)
        {

        }

        if(ratio>=0 && ratio<=1)
        healthFill.fillAmount = ratio;
    }

    void EnableWindow()
    {
        enabled = true;
        entityCanvas.enabled = true;
    }

    void DisableWindow()
    {
        enabled = false;
        entityCanvas.enabled = false;
    }

    public void UnloadCompletely()
    {
        if(currentSelector)
        {
            if (currentSelector is EnemyStats)
                ((EnemyStats)currentSelector).healthChange -= UpdateHealth;
            currentSelector.Deselect();
        }
        DisableWindow();
        currentSelector = null;
        player.targetStats = null;
    }

    public void Unload()
    {
        DisableWindow();
        currentSelector = null;
        player.targetStats = null;
    }
}
