using Scripts.OOP.EnemyBehaviors;

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
        this.Level = level;
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
        //Behavior.AbilityUpdate(this);
    }

    public override bool IsFiring(out float angle)
        => Behavior.Fire(this, out angle);
}
