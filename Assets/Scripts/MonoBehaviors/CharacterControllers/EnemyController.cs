using Scripts.OOP.EnemyBehaviors;
using UnityEngine.UI;

public class EnemyController : BaseController
{
    public EnemySettings settings;

    internal EnemyBehavior Behavior { get; private set; }

    internal BaseController target;

    public override string Name 
    {
        get => settings?.name ?? base.Name; 
        set => base.Name = value; 
    }

    public void Set(int level)
    {
        this.level = level;
    }

    public override void OnStart()
    {
        Behavior = settings.SetSettings(this);
        Name = settings.name;
        settings = null;
        target = Behavior.Target(this);
    }

    public override void OnUpdate()
    {
        if(!target) target = Behavior.Target(this);
        Behavior.Ability(this);
    }

    public override bool IsFiring(out float angle)
        => Behavior.Fire(this, out angle);
}
