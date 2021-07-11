using Scripts.OOP.EnemyBehaviors;

public class EnemyController : BaseController
{
    public EnemySettings settings;

    internal EnemyBehavior Behavior { get; private set; }

    internal BaseController target;

    public void Set(int level)
    {
        this.level = level;
    }

    public override void OnStart()
    {
        Behavior = settings.SetSettings(this);
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
