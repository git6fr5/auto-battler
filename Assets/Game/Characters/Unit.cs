/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Defintions --- */
using Team = Arena.Team;

public class Unit : Input {

    /* --- Variables --- */
    #region Variables
    
    // Settings.
    [SerializeField] public Team team;
    [SerializeField] public int cost = 1;
    public Controller controller => GetComponent<Controller>();

    [SerializeField] private float visionRadius = 1f;
    [SerializeField] private float attackMaxRadius = 1f;
    [SerializeField] private float attackMinRadius = 1f;
    [SerializeField] private float dangerRadius = 1f;
    [SerializeField] private float thinkInterval = 1f;

    // AI.
    [SerializeField, ReadOnly] private float ticks = 0f;
    [SerializeField, ReadOnly] private Unit targetUnit;
    [HideInInspector] private Vector2 randomDirection = new Vector2(0f, 0f);
    
    #endregion

    public override void Get() {
        if (targetUnit != null) {

            Vector2 targetPosition = targetUnit.transform.position;
            Vector2 position = transform.position;

            Vector2 attackPosition = targetPosition + attackMinRadius * (position - targetPosition).normalized;
            Vector2 towards = attackPosition - position;
            Vector2 around = Quaternion.Euler(0f, 0f, 90f) * towards.normalized;
            
            movement = towards.sqrMagnitude < GameRules.MovementPrecision ? around : towards.normalized;
            attack = (targetPosition - position).magnitude < attackMaxRadius ? true : false;
            target = (targetPosition - position).normalized;

        }
        else {
            movement = randomDirection;
            attack = false;
            target = randomDirection;
        }
        
    }

    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        ticks -= deltaTime;
        if (ticks < 0f) {
            Think();
            ticks += thinkInterval;
        }
    }

    private void Think() {
        Unit[] units = Arena.GetAllCircle<Unit>(transform.position, visionRadius, GameRules.CharacterCollisionLayer);
        Unit[] enemies = Arena.Filter(units, Arena.EnemyTeam(team));
        targetUnit = GetClosest(enemies);
        randomDirection = Random.insideUnitCircle.normalized;
    }

    private Unit GetClosest(Unit[] units) {
        float minSqrDistance = Mathf.Infinity;
        Unit closestUnit = null;
        for (int i = 0; i < units.Length; i++) {
            float sqrDistance = (transform.position - units[i].transform.position).sqrMagnitude;
            if (sqrDistance < minSqrDistance) {
                closestUnit = units[i];
                minSqrDistance = sqrDistance;
            }
        }
        return closestUnit;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackMinRadius);
        Gizmos.DrawWireSphere(transform.position, attackMaxRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerRadius);
    }

}
