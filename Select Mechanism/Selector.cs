using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Selector : Stats
{
    #region Serialized Fields
    public new string name;
    public Sprite picture;
    public string extraInfo;
    [SerializeField] float autoSelectDistance = 3f;
    #endregion

    #region Polymorphic Fields
    protected Player player;
    #endregion

    #region Constants & Statics
    public static float lastSelectDistance = 100f;
    #endregion

    #region Cached Fields
    protected SelectTracer tracer;
    SphereCollider autoSelectCollider;
    bool isSelected;
    #endregion
    public override void Start()
    {
        base.Start();
        player = Player.Instance;
        tracer = SelectTracer.Instance;
        autoSelectCollider = GetComponent<SphereCollider>();
        autoSelectCollider.radius = autoSelectDistance;
        lastSelectDistance = float.MaxValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerStay(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Player>(out player))
        {
            float distance = Helper.CalculateDistance(player.transform.position, transform.position);

            if ((tracer.enabled && isSelected == false) && (distance < lastSelectDistance))
                Select(distance);
            else if (!tracer.enabled)
                Select(distance);

        }
    }

    public void Select(float distance)
    {
        FindObjectsOfType<Selector>().Where(s => s.isSelected).ToList().ForEach(s => s.Deselect());
        lastSelectDistance = distance;
        isSelected = true;
        tracer.Load(this);
    }

    public void Deselect()
    {
        isSelected = false;
        lastSelectDistance = float.MaxValue;
        tracer.Unload();
    }
}