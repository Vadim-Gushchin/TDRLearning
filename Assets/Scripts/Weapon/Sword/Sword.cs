using System;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Sword : MonoBehaviour
{
    [SerializeField] private int _swordDamage = 5;

    public event EventHandler OnSwordSwing;

    public PolygonCollider2D _polygonColider2D;

    private void Awake()
    {
        _polygonColider2D = GetComponent<PolygonCollider2D>();
    }


    private void Start()
    {
        AttackColliderTurnOff();
    }

    public void Attack()
    {
        AttackColliderTurner();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
            enemyEntity.TakeDamage(_swordDamage);
    }

    public void AttackColliderTurner()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }

    public void AttackColliderTurnOff()
    {
        _polygonColider2D.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        _polygonColider2D.enabled = true;
    }
}

