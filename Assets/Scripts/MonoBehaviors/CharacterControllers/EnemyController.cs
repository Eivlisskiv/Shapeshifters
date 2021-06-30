using Scripts.OOP.EnemyBehaviors;

public class EnemyController : BaseController
{
    public EnemySettings settings;

    private EnemyBehavior behavior;

    internal BaseController target;

    public void Set(int level)
    {
        this.level = level;
    }

    public override void OnStart()
    {
        behavior = settings.SetSettings(this);
        settings = null;
        target = behavior.Target(this);
    }

    public override void OnUpdate()
    {
        if(!target) target = behavior.Target(this);
    }

    public override bool IsFiring(out float angle)
        => behavior.Fire(this, out angle);
}
