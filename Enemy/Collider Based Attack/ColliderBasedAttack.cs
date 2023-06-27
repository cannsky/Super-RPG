using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBasedAttack : MonoBehaviour
{
    internal EnemyStats stats;
    [SerializeField]Collider attackCollider;

    private void Start()
    {
        attackCollider.enabled = false;
    }

    internal void Attack()
    {
        attackCollider.enabled = true;
        Invoke("StopCollider", 0.3f);
    }

    void StopCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Player playerTemp;
        collider.gameObject.TryGetComponent<Player>(out playerTemp);
        if (playerTemp == null) return;
        attackCollider.enabled = false;
        stats.TryHit(Player.Instance);
    }
}
