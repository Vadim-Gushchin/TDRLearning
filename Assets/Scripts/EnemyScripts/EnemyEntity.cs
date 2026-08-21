using System;
using UnityEngine;


[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
   

    private int _currentHealth;
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;   
    private EnemyAI _enemyAI;

    public event EventHandler OnSkeletonDead;
    public event EventHandler OnGotDamage;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>(); 
    }

    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    private void Update()
    {
        DetectDeath();
    }

  

    public void TakeDamage(int damage)
    {
        OnGotDamage?.Invoke(this,EventArgs.Empty);
        _currentHealth -= damage;

    }
    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            OnSkeletonDead?.Invoke(this, EventArgs.Empty);

            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            _enemyAI.enabled = false;
            _enemyAI.SetDeathState();
        }
    }
}

