using System.Linq;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    //Config params
    [SerializeField] float maxRayDistance=100f;
    [SerializeField] Camera thirdPersonCamera;
    
    Player player;
    SelectTracer tracer;
    Selector currentSelector;

    static Clicker instance;
    public static Clicker Instance { get => instance; }
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
        tracer = SelectTracer.Instance;
    }

    public void ClickOnEnemy()
    {
        bool deselect = true;
        RaycastHit raycastHit;
        Ray ray = thirdPersonCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit, maxRayDistance, GetLayerMask(), QueryTriggerInteraction.Ignore) && raycastHit.transform != null)
        {
            Selector selector;
            raycastHit.collider.TryGetComponent<Selector>(out selector);

            if (selector != null)
            {
                float distance = Helper.CalculateDistance(player.transform.position, selector.transform.position);
                if (distance <= tracer.maxSelectDistance)
                {
                    deselect = false;

                    if (currentSelector == selector)
                        player.AttackRequest();
                    else
                    {
                        currentSelector = selector;
                        player.autoChase = false;
                        selector.Select(distance);
                    }
                }
            }
        }

        if (deselect)
        {
            currentSelector = null;
            tracer?.UnloadCompletely();
        }
    }

    public void ClickAnywhere()
    {
        var selectors = Physics.OverlapSphere(player.transform.position, tracer.maxSelectDistance, GetLayerMask());

        if (selectors.Length==0) return;

        RequestAttack(selectors[0].GetComponent<Selector>());
    }

    void RequestAttack(Selector selector)
    {
        bool deselect = true;

        if (selector != null)
        {
            float distance = Helper.CalculateDistance(player.transform.position, selector.transform.position);
            if (distance <= tracer.maxSelectDistance)
            {
                deselect = false;

                if (currentSelector == selector)
                    player.AttackRequest();
                else
                {
                    currentSelector = selector;
                    player.autoChase = false;
                    selector.Select(distance);
                }
            }
        }

        if (deselect)
        {
            currentSelector = null;
            tracer?.UnloadCompletely();
        }
    }

    private int GetLayerMask()
    {
        return LayerMask.GetMask("Enemy", "NPC");
    }
}
