using UnityEngine;

public class MeleeEnemyInput : EnemyInputBase
{
    private int _selectedAttackNo = 0;

    protected override void Update()
    {
        if (_target != null)
        {
            float distance = Vector3.Distance(transform.position, _target.GetTransform().position);

            SetCurrentAttack(distance);

            if (distance <= agent.stoppingDistance)
            {
                OnAttackActionStarted?.Invoke();
            }
        }
    }

    private void SetCurrentAttack(float distance)
    {
        var curLowest = Mathf.Infinity;
        int selectedAttackNo = 0;
        for (int i = 0; i < attackController.GetMeleeAttacks().Count; i++)
        {
            var meleeAttack = (MeleeAttackBase)attackController.GetMeleeAttacks()[i];
            var dif = Mathf.Abs(meleeAttack.CurrentComboDamageData.attackRange - distance);
            if (dif < curLowest)
            {
                curLowest = dif;
                if (!meleeAttack.IsOnCooldown)
                    selectedAttackNo = i;
            }
        }
        _selectedAttackNo = selectedAttackNo;
        OnMeleeWeaponSwitchedWithID?.Invoke(_selectedAttackNo);
    }

    public override Vector3 GetMovementInput()
    {
        if (_target != null)
        {
            agent.stoppingDistance = attackController.SelectedMeleeAttack.CurrentComboDamageData.attackRange;
            if (Vector3.Distance(transform.position, _target.GetTransform().position) > agent.stoppingDistance)
            {
                return _target.GetTransform().position;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            agent.stoppingDistance = 0.5f;
            var returnPos = shouldPatrol ? GetPatrolPosition() : startPos;
            if (Vector3.Distance(transform.position, returnPos) > agent.stoppingDistance)
            {
                return returnPos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
