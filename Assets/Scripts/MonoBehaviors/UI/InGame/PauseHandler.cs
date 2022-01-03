using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Utils;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    private const float anim_speed = 0.1f;

    public static void ForcePause(bool pause)
    { 
        if (!Instance) return;

        Instance.paused = pause;
        Instance.UpdatePaused();
    }

    public static void SetControl(bool control)
    {
        Instance.canPause = control;
        Instance.button.gameObject.SetActive(control);

        ForcePause(false);
    }

    private static PauseHandler Instance;

    public Image grey;
    public Text text;

    public Button button;

    public GeneralButton exit;

    private RectTransform left;
    private Vector3 lpos;
    private RectTransform right;
    private Vector3 rpos;

    private bool canPause = false;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        left = button.transform.GetChild(0).GetComponent<RectTransform>();
        lpos = left.localPosition;
        right = button.transform.GetChild(1).GetComponent<RectTransform>();
        rpos = right.localPosition;
    }

    private void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.P))
            TogglePaused();
    }

    public void TogglePaused()
    {
        paused = !paused;

        UpdatePaused();
    }

    public void ExitGame()
    {
        exit.ChangeFocus(false);
        exit.ChangeSelect(false);

        if (GameModes.GameMode == null) return;

        TogglePaused();
        GameModes.GameMode.GameOver();
    }

    private void UpdatePaused()
    {
        if (paused)
        {
            TimeHandler.Instance.Pause();
            ToPlay();
        }
        else
        {
            TimeHandler.Instance.Resume();
            ToPause();
        }

        GameModes.GameMode?.PauseControllers(paused);

        grey.enabled = paused;
        text.enabled = paused;
        exit.gameObject.SetActive(paused);
    }

    private void ToPlay()
    {
        left.Tween<Transform, Vector3, PositionTween>
            (lpos + new Vector3(22, 13.9f, 0), anim_speed)
            .scaledTime = false;

        left.Tween<Transform, Vector3, VectorRotationTween>
            (new Vector3(0, 0, 65), anim_speed)
            .scaledTime = false;


        right.Tween<Transform, Vector3, PositionTween>
            (rpos + new Vector3(-22, -13.9f, 0), anim_speed)
            .scaledTime = false;

        right.Tween<Transform, Vector3, VectorRotationTween>
            (new Vector3(0, 0, -65), anim_speed)
            .scaledTime = false;
    }

    private void ToPause()
    {
        left.Tween<Transform, Vector3, PositionTween>
            (lpos, anim_speed).scaledTime = false;

        left.Tween<Transform, Vector3, VectorRotationTween>
            (new Vector3(0, 0, 0), anim_speed)
            .scaledTime = false;


        right.Tween<Transform, Vector3, PositionTween>
            (rpos, anim_speed).scaledTime = false;

        right.Tween<Transform, Vector3, VectorRotationTween>
            (new Vector3(0, 0, 360), anim_speed, callback: 
            () => right.rotation = Quaternion.Euler(0, 0, 0))
            .scaledTime = false;
    }
}
