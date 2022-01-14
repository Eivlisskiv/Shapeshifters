using IgnitedBox.Utilities;
using Scripts.OOP.EnemyBehaviors;
using Scripts.OOP.Game_Modes.CustomLevels;

public class EnemyController : BaseController, ILevelProp
{
    public EnemySettings settings;

    internal EnemyBehavior Behavior { get; private set; }

    internal BaseController target;

    public override string Name 
    {
        get => settings?.name ?? base.Name; 
        set => base.Name = value; 
    }
    public bool Enabled 
    {
        get => enabled && ControllerEnabled; 
        set 
        {
            enabled = value;
            DisableController(!value);
            gameObject.SetActive(value);
        } 
    }

    public bool Consumed => Dying.HasValue;

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

    public void LoadParameters(object[] param)
    {
        Team = param.ParamAs(0, 1);
        Level = param.ParamAs(1, 0);
    }
}
