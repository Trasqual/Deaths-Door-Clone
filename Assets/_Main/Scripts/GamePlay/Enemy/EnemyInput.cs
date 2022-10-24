using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInput : InputBase
{
    [SerializeField] private EnemyTriggerDetector detecterPrefab;
    private EnemyBehaviour enemyBehaviour;
    private EnemyTriggerDetector detector;
    private NavMeshAgent agent;

    private Player player;
    private Vector3 startPos;
    private bool isAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();

        if (detector == null)
        {
            detector = Instantiate(detecterPrefab, transform.position, Quaternion.identity, transform);
            detector.OnTargetFound += OnTargetDetectedCallback;
            detector.OnTargetLost += OnTargetLostCallback;
        }

        startPos = transform.position;
    }

    public override Vector3 GetLookInput()
    {
        if (player != null)
        {
            return (player.transform.position - transform.position).normalized;
        }
        else
        {
            return (startPos - transform.position).normalized;
        }
    }

    public override Vector3 GetMovementInput()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
            {
                return player.transform.position;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) > agent.stoppingDistance)
            {
                return startPos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    OnAttackActionStarted?.Invoke();
                    var curAttack = (MeleeAttackAnimationData)enemyBehaviour.AttackController.SelectedMeleeAttack.CurrentAttackAnimationData;
                    DOVirtual.DelayedCall(curAttack.attackCD + 0.5f, () =>
                    {
                        isAttacking = false;
                    });
                }
            }
        }
    }

    private void OnTargetDetectedCallback(Player target)
    {
        player = target;
    }

    private void OnTargetLostCallback()
    {
        player = null;
    }
}
