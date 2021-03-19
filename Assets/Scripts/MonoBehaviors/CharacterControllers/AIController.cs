using Scripts.OOP.Utils;
using UnityEngine;
using Scripts.OOP.TileMaps;
using Scripts.OOP.Perks;
using UnityEngine.U2D;

public class AIController : BaseController
{
    public static AIController Spawn(GameObject mob, string name, int level)
    {
        AIController ai = mob.AddComponent<AIController>();
        mob.name = name;
        ai.Color = Color.red;
        ai.Initialize(level);
        mob.GetComponent<SpriteShapeRenderer>().color = Color.red;
        return ai;
    }

    BaseController target;

    public void Initialize(int level)
    {
        this.level = level;
        stats = new Scripts.OOP.Character.Stats.Stats(50 + (level / 5));
        Perk perk = PerksHandler.Random();
        perk.LevelUp(level);
        if (perks == null) perks = new PerksHandler();
        perks.Add(perk);
    }

    public override void OnStart()
    {
        GenerateBuild();
        target = GetNearestPlayer();
    }

    public void GenerateBuild()
    {
        xp = 1;
        body.corners = Random.Range(3, 10);
    }

    public override void OnUpdate()
    {
        if (!target) target = GetNearestPlayer();
    }

    public override bool IsFiring(out float angle)
    {
        angle = 0;
        if(FireReady && weapon)
        {
            if (!target || !ShootTarget(out angle))
            {
                if (Randomf.Chance(50)) return false;
                angle = Random.Range(0, 360);
            }
            return true;
        }
        return false;
    }

    public override void OnHealthChange() { }

    public override void OnXPChange(bool isUp)
    {
        if(isUp)
        {
            Perk perk = PerksHandler.Random();
            perk.LevelUp();
            perks.Add(perk);
        }
    }

    public override bool OnDeath()
    {
        WaveData.Wave.RemoveMob(this);
        return true;
    }

    public override void OnDeathEnded()
    {

    }

    private bool ShootTarget(out float angle)
    {
        angle = 0;
        Vector2 vt = target.transform.position;
        Vector2 pos = transform.position;
        Vector2 direction = vt - pos;
        float distance = direction.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(pos + (direction.normalized * (body.radius + 0.1f)), direction);
        if(hit && hit.transform == target.transform)
        {
            angle = Vectors2.TrueAngle(Vector2.right, pos - vt);
            if (distance <= weapon.Range) angle += 180;
            return true;
        }

        return false;
    }

    private PlayerController GetNearestPlayer()
    {
        if (PlayerController.players.Count == 0) return null;
        //Temporary
        return PlayerController.players[0] == null ? 
            null : PlayerController.players[0];
        /*
        PlayerController nearest = players[0];
        float distance = (nearest.transform.position - transform.position).magnitude;
        for(int i = 1; i < players.Length; i++)
        {
            PlayerController compare = players[i];
            float d = (compare.transform.position - transform.position).magnitude;
            if(d < distance)
            {
                distance = d;
                nearest = compare;
            }
        }

        return nearest;*/
    }
}
