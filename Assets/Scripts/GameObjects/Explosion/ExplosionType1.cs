using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using IgnitedBox.UnityUtilities;
using Scripts.OOP.Utils;
using UnityEngine;
using UnityEngine.U2D;

public class ExplosionType1 : MonoBehaviour
{

    public float size;
    public float speed;

    public float minAngle;
    public float maxAngle;

    ExplosionSpike[] spikes;

    float totalTime;
    float time;

    public enum State { Stopped, Playing, Paused }
    public State state;
    private State internalState;

    // Start is called before the first frame update
    void Start() => PlayNew();

    private void PlayNew()
    {
        totalTime = size / speed;
        time = 0;
        SpawnSpikes();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != internalState)
        {
            if (state == State.Playing && 
                internalState == State.Stopped)
                PlayNew();

            internalState = state;
        }

        if (internalState != State.Playing) return;

        time += Time.deltaTime * speed;

        if (time < totalTime)
        {
            Explode();
            return;
        }

        state = (internalState = State.Stopped);
        Dissipate();
    }

    private void Explode()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].Expand(time);
        }
    }

    private void Dissipate()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].Fade();
        }
    }

    void SpawnSpikes()
    {
        spikes = new ExplosionSpike[Randomf.Int(4, 6)];
        for(int i = 0; i < spikes.Length; i++)
        {
            spikes[i] = new ExplosionSpike(gameObject.transform,
                size, Random.Range(minAngle, maxAngle));
        }
    }
}

public class ExplosionSpike
{
    readonly LineRenderer line;
    readonly Vector2 direction;
    readonly float offset;

    public ExplosionSpike(Transform parent, float size, float angle)
    {
        offset = Random.Range(1, 1.5f);

        direction = Vectors2.FromDegAngle(angle, size * 1.2f);

        line = Components.CreateGameObject
            <LineRenderer>("Spike", parent);
        Initialize();
    }

    private void Initialize()
    {
        line.transform.localPosition = Vector2.zero;
        line.positionCount = 2;
        SetPosition(0, Vector2.zero);
        SetPosition(1, Vector2.zero);

        line.startWidth = 0.5f;
        line.endWidth = 0;

        line.material = StaticStuff.UnlitSpriteMaterial;
    }

    private void SetPosition(int index, Vector2 position)
        => line.SetPosition(index, position
            + (Vector2)line.transform.position);

    public void Expand(float progress)
        => SetPosition(1, direction * (offset * progress));

    public void Fade()
    {
        line.Tween<LineRenderer, Color, LineRendererColorTween>
            (line.startColor - new Color(0, 0, 0, 1), 0.2f, 
            callback: () => Object.Destroy(line.gameObject));
    }
}

class ExplosionCore
{
    readonly SpriteShapeRenderer shape;
    readonly SpriteShapeController controller;

    public ExplosionCore(Transform parent, float size, float angle)
    {
        //shape =
    }
}

class LineRendererColorTween : ColorTweener<LineRenderer>
{
    public LineRendererColorTween() : base() { }

    public LineRendererColorTween(LineRenderer element, Color target, float time,
    float delay, System.Func<double, double> easing, System.Action callback)
    : base(element, target, time, delay, easing, callback) { }

    public override void Blend(TweenData<LineRenderer, Color> with)
    {
        throw new System.NotImplementedException();
    }

    public override Color GetTweenAt(float percent)
    {
        throw new System.NotImplementedException();
    }

    protected override Color GetStart()
        => Element.startColor;

    protected override Color GetTween()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnFinish()
    {
        Element.startColor = Target;
        Element.endColor = Target;
    }

    protected override void OnMove(Color current)
    {
        Element.startColor = current;
        Element.endColor = current;
    }
}
