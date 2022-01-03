using Assets.Scripts.MonoBehaviors.Weapons.Other;
using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using IgnitedBox.Utilities;
using Scripts.Explosion;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Props;
using UnityEngine;

public class Landmine : OtherProjectile, ILevelProp
{
    public bool Enabled { get => active; set => active = value; }

    public FireworkExplosionHandler explosion;

    public SpriteRenderer teamColor;
    public SpriteRenderer activeLight;

    public bool Consumed { get; private set; }

    protected override void OnStart()
    {
        base.OnStart();
        explosion.Angle = 360;
        explosion.Speed = 20;
        explosion.Intensity = 1;
        explosion.Range = 10;
    }

    public override void Activate(float damage, float force, BaseController owner)
    {
        explosion.damage = damage;
        explosion.Intensity = force;
        this.owner = owner;
        active = true;

        if (owner)
        {
            explosion.Sender = owner;
            Physics2D.IgnoreCollision(BodyCollider, owner.Body.Collider);
            teamColor.color = owner.GetColor(0);
        }

        var tween = activeLight.Tween<SpriteRenderer, Color, SpriteRendererColorTween>
            (new Color(1, 66f/255, 66f/255, 1), 2, 0.2f);

        tween.loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ResetLoop;

        AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0),
            new Keyframe(0.10f, 1), new Keyframe(0.20f, 0), new Keyframe(0.30f, 1),
            new Keyframe(1, 0));
        for(int i = 0; i < curve.length; i++)
            curve.SmoothTangents(i, 0);
        tween.Curve = curve;

        Destroy(gameObject, 300f);
    }

    public override void OnCollide(Collider2D collision)
    {
        if (!active) return;

        var controller = collision.gameObject.GetComponent<BaseController>();

        if (!controller || controller.Team == IgnoreTeam) return;

        OnHit(null);

        Destroy();
    }

    protected override void OnHit(BaseController victim)
    {
        explosion.Play();
    }

    protected override void Destroy()
    {
        Consumed = true;

        if (GameModes.GameMode is CustomLevel level)
            level.ObjectiveEvents.Invoke(typeof(IOnPropActivation), (ILevelProp)this, gameObject);

        base.Destroy();

        Destroy(teamColor.gameObject);
        Destroy(activeLight.gameObject);
        Destroy(gameObject, explosion.Duration + 0.5f);
    }

    public void LoadParameters(object[] param)
    {
        int team = param.ParamAs<int>(0);
        explosion.IgnoreTeam = team;
        IgnoreTeam = team;

        teamColor.color = GameModes.GameMode?.teamColors[team] ?? Color.red;

        float damage = param.ParamAs<float>(1, 10);
        float force = param.ParamAs<float>(2, 10);

        Activate(damage, force, null);
    }
}
