using System.Threading;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;
    private Animator _animator;


    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLAYER = "ChasingSpeedMultiplayer";
    private const string IS_ATTACKING = "Attack";
    private const string IS_DEAD = "IsDead";
    private const string GOT_DAMAGE = "GotHit";

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnSkeletonDead += _enemyEntity_OnSkeletonDead;
        _enemyEntity.OnGotDamage += _enemyEntity_OnGotDamage;
    }


    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLAYER, _enemyAI.RoamingAnimationSpeed);
    }
    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        _enemyEntity.OnSkeletonDead -= _enemyEntity_OnSkeletonDead;
        _enemyEntity.OnGotDamage -= _enemyEntity_OnGotDamage;

    }

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IS_ATTACKING);

    }

    private void _enemyEntity_OnGotDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(GOT_DAMAGE);
    }

    private void _enemyEntity_OnSkeletonDead(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DEAD, true);
        _spriteRenderer.sortingOrder = -1;
        _enemyShadow.SetActive(false);

    }
}
