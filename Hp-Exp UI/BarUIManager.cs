using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BarUIManager : MonoBehaviour,IUIHider,IPointerEnterHandler,IPointerExitHandler
{
    public enum UILayouts
    {
        Layout1=0,
        Layout2
    }

    //Config Params
    [SerializeField] UIToggle layout1;
    [SerializeField] UIToggle layout2;
    [SerializeField] Image healthFill1;
    [SerializeField] Image healthFill2;
    [SerializeField] Image energyFill1;
    [SerializeField] Image energyFill2;
    [SerializeField] Image expFill1_1;
    [SerializeField] Image expFill1_2;
    [SerializeField] Image expFill2_1;
    [SerializeField] Image expFill2_2;
    [SerializeField] Image expFill2_3;
    [SerializeField] Image expFill2_4;

    //Cached Fields
    private UILayouts currentUILayout=UILayouts.Layout2;
    private PlayerLevelController levelController;
    private Player stats;
    private AnimationManager manager;
    private const float oneEleventh = 1f / 11;
    private const float oneThird = 1f / 3;
    private float ratio = 0;
    private float expRatio = 0;
    [NonSerialized] public int expPeriod = 0;
    private int oneThirdCount = 0;
    [NonSerialized] public bool isStarted = false;

    private static BarUIManager instance;
    public static BarUIManager Instance { get => instance; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this, 0.01f);
    }

    public void Start()
    {
        manager = FindObjectOfType<AnimationManager>();
        manager.Start();
        levelController = PlayerLevelController.Instance;
        levelController.gainExp += UpdateLevelBars;
        stats = Player.Instance;
        stats.Start();
        stats.healthChange += UpdateHealthBar;
        Load();
        isStarted = true;
    }

    public void Load()
    {
        UpdateHealthBar(stats.allStats.health, stats.allStats.currentHealth);
        UpdateEnergyBar(stats.allStats.energy, stats.allStats.currentEnergy);
        UpdateLevelBars(1f, GetCurrentExpValues());
    }

    public void LevelUpLoad()
    {
        UpdateHealthBar(stats.allStats.health, stats.allStats.currentHealth);
        UpdateEnergyBar(stats.allStats.energy, stats.allStats.currentEnergy);
        UpdateLevelBars(1f, 1f);
    }

    public void ChangeLayout()
    {
        float hpRatio;
        float energyRatio;
        float expRatio;
        if(currentUILayout==UILayouts.Layout1)
        {            
            hpRatio = healthFill1.fillAmount;
            energyRatio = energyFill1.fillAmount;
            expRatio = expFill1_1.fillAmount;

            layout1.Toggle();
            layout2.Toggle();
            currentUILayout = UILayouts.Layout2;
        }
        else
        {
            hpRatio = healthFill2.fillAmount;
            energyRatio = energyFill2.fillAmount;
            expRatio = expFill2_4.fillAmount;

            layout2.Toggle();
            layout1.Toggle();
            currentUILayout = UILayouts.Layout1;
        }

        UpdateHealthBar(1, hpRatio);
        UpdateEnergyBar(1, energyRatio);
        UpdateLevelBars(1, expRatio);
    }

    private void UpdateHealthBar(float totalHp,float currentHp)
    {
        ratio = currentHp / totalHp;

        if (currentUILayout == UILayouts.Layout1)
            healthFill1.fillAmount = ratio;
        else
            healthFill2.fillAmount = ratio;
    }

    private void UpdateEnergyBar(float totalEnergy,float currentEnergy)
    {
        ratio = currentEnergy / totalEnergy;

        if (currentUILayout == UILayouts.Layout1)
            energyFill1.fillAmount = ratio;
        else
            energyFill2.fillAmount = ratio;
    }

    private void UpdateLevelBars(float totalExpNeeded,float currentExp)
    {
        expRatio = currentExp / totalExpNeeded;

        if (currentUILayout==UILayouts.Layout1)
        {
            if (expRatio == 1f)            
                expRatio = GetCurrentExpValues();
                          
            expFill1_1.fillAmount = expRatio;
            expFill1_2.fillAmount = (expRatio % oneEleventh) / oneEleventh;

            expPeriod = CalculateExpPeriod();
        }
        else
        {            
            expFill2_4.fillAmount = expRatio;
            expFill2_1.fillAmount = (expRatio / oneThird >= 1) ? 1 : (expRatio % oneThird) / oneThird;
            expFill2_2.fillAmount = (expRatio / (2 * oneThird) >= 1) ? 1 : ((expRatio - oneThird) % oneThird) / oneThird;
            expFill2_3.fillAmount = ((expRatio - (2 * oneThird)) % oneThird) / oneThird;

            oneThirdCount = Convert.ToInt32(expRatio/ oneThird)-expPeriod;
            
            manager.PlayOneThirdComplete(oneThirdCount);

            if(expRatio==1f)
            {
                expPeriod = 0;
                manager.PlayLevelUp();                
            }
            else
            {
                expPeriod = CalculateExpPeriod();
            }
        }
        
        int CalculateExpPeriod()
        {
            return (expRatio < oneThird) ? 0 :
                   (expRatio < 2 * oneThird) ? 1 : 2;
        }

    }

    public float GetCurrentExpValues()
    {
        return levelController.currentExp / (float)levelController.totalXpRequiredForLevelup;
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveBarUI(this);
    }

    public void HideUI()
    {
        if (currentUILayout == UILayouts.Layout1)
            layout1.Toggle();
        else
            layout2.Toggle();
    }

    public void UnhideUI()
    {
        HideUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
