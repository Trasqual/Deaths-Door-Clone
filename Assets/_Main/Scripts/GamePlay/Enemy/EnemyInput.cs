using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.Player;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInput : InputBase
{
    [SerializeField] private EnemyTriggerDetector detecterPrefab;
    private EnemyTriggerDetector detector;
    private NavMeshAgent agent;
    private Player player;

    private Vector3 startPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if(detector == null)
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
            return player.transform.position;
        }
        else
        {
            if (Vector3.Distance(transform.position,startPos) > agent.stoppingDistance)
            {
                return startPos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    public override Vector3 GetMovementInput()
    {
        if (player != null)
        {
            return player.transform.position;
        }
        else
        {
            if(Vector3.Distance(transform.position, startPos) > agent.stoppingDistance)
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
        if(player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            {
                OnAttackActionStarted?.Invoke();
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
