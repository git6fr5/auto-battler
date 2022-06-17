/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///<summary>
[DefaultExecutionOrder(1000)]
public class Arena : MonoBehaviour {

    public enum Team {
        Good, Bad, Neutral, None
    }

    public bool paused;
    public bool playing => !paused;
    public Team winner => GetWinner();
    public bool finished => winner != Team.None;

    public int roundNumber = 0;

    public Vector2 size;

    private Dictionary<Controller, Vector3> controllerCache = new Dictionary<Controller, Vector3>();

    void Start() {
        Pause();
    }

    void Update() {
        if (playing && finished) {
            Pause();
        }
    }

    public void Pause() {
        paused = true;

        if (winner == Team.Good) {
            NextRound();
        }

        ResetControllers();
        DestroyAllProjectiles();
        GameRules.Shop.Enable();

    }

    public void Play() {
        paused = false;
        CacheControllers();
        RunControllers();
        GameRules.Shop.Disable();

    }

    private void ResetControllers() {
        foreach (KeyValuePair<Controller, Vector3> kv in controllerCache) {
            if (kv.Key != null) {
                kv.Key.Reset(kv.Value);
            }
        }
    }

    public void NextRound() {
        roundNumber += 1;
        GameRules.Shop.AddGold(5);
    }

    private void CacheControllers() {
        controllerCache = new Dictionary<Controller, Vector3>();

        Controller[] controllers = GetAllBox<Controller>(transform.position, size, GameRules.CharacterCollisionLayer);
        for (int i = 0; i < controllers.Length; i++) {
            controllerCache.Add(controllers[i], controllers[i].transform.position);
        }
    }

    private void RunControllers() {
        foreach (KeyValuePair<Controller, Vector3> kv in controllerCache) {
            if (kv.Key != null) {
                kv.Key.Run();
            }
        }
    }

    private Team GetWinner() {
        Unit[] units = GetAllBox<Unit>(transform.position, size + new Vector2(1f, 1f), GameRules.CharacterCollisionLayer);

        // If all units are dead.
        if (units.Length == 0) {
            return Team.Neutral;
        }
        
        // If there is only one unit left.
        Team firstUnitTeam = units[0].team;
        if (units.Length == 1) {
            return firstUnitTeam;
        }

        // If there is more than one unit.
        for (int i = 1; i < units.Length; i++) {
            if (units[i].team != firstUnitTeam) {
                return Team.None;
            }
        }
        return firstUnitTeam;

    }

    public static void DestroyAllProjectiles() {
        Projectile[] projectiles = (Projectile[])GameObject.FindObjectsOfType(typeof(Projectile));
        for (int i = 0; i < projectiles.Length; i++) {
            Destroy(projectiles[i].gameObject);
        }
    }

    public static T[] GetAllBox<T>(Vector2 center, Vector2 size, LayerMask layer) {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, layer);
        List<T> listT = new List<T>();
        for (int i = 0; i < colliders.Length; i++) {
            T t = colliders[i].GetComponent<T>();
            if (t != null) {
                listT.Add(t);
            }
        }
        return listT.ToArray();
    }

    public static T[] GetAllCircle<T>(Vector3 position, float radius, LayerMask layer) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, layer);
        List<T> listT = new List<T>();
        for (int i = 0; i < colliders.Length; i++) {
            T t = colliders[i].GetComponent<T>();
            if (t != null) { 
                listT.Add(t);
            }
        }
        return listT.ToArray();
    }

    public static Unit[] Filter(Unit[] units, Team team) {
        List<Unit> filteredUnits = new List<Unit>();
        for (int i = 0; i < units.Length; i++) {
            if (units[i].team == team) { 
                filteredUnits.Add(units[i]);
            }
        }
        return filteredUnits.ToArray();
    }

    public static Team EnemyTeam(Team team) {
        if (team == Team.Good) { return Team.Bad; }
        if (team == Team.Bad) { return Team.Good; }
        if (team == Team.Neutral) { return Team.None; }
        return Team.None;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, size);
    }

}