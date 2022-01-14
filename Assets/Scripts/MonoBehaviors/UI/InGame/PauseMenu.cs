using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.UI.Menus.PauseMenu;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Utils;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : PauseHandler
{
    private RectTransform left;
    private Vector3 lpos;
    private RectTransform right;
    private Vector3 rpos;


    // Start is called before the first frame update
    protected override void OnStart()
    {
        if (!pauseButton) return;

        left = pauseButton.transform.GetChild(0).GetComponent<RectTransform>();
        lpos = left.localPosition;
        right = pauseButton.transform.GetChild(1).GetComponent<RectTransform>();
        rpos = right.localPosition;
    }

    public void ExitGame(GeneralButton exit)
    {
        exit.ChangeFocus(false);
        exit.ChangeSelect(false);

        if (GameModes.GameMode == null) return;

        TogglePaused();
        GameModes.GameMode.GameOver();
    }

    protected override void OnPauseUpdated()
    {
        GameModes.GameMode?.PauseControllers(Paused);
    }

    protected override void OnPlay()
    {
        //TimeHandler.Instance.Pause();

        if (left)
        {
            left.Tween<Transform, Vector3, PositionTween>
                (lpos + new Vector3(22, 13.9f, 0), containerFadeTime)
                .scaledTime = false;

            left.Tween<Transform, Vector3, VectorRotationTween>
                (new Vector3(0, 0, 65), containerFadeTime)
                .scaledTime = false;
        }

        if (right)
        {
            right.Tween<Transform, Vector3, PositionTween>
                (rpos + new Vector3(-22, -13.9f, 0), containerFadeTime)
                .scaledTime = false;

            right.Tween<Transform, Vector3, VectorRotationTween>
                (new Vector3(0, 0, -65), containerFadeTime)
                .scaledTime = false;
        }
    }

    protected override void OnPause()
    {
        //TimeHandler.Instance.Resume();

        if (left)
        {
            left.Tween<Transform, Vector3, PositionTween>
                (lpos, containerFadeTime).scaledTime = false;

            left.Tween<Transform, Vector3, VectorRotationTween>
                (new Vector3(0, 0, 0), containerFadeTime)
                .scaledTime = false;
        }

        if (right)
        {

            right.Tween<Transform, Vector3, PositionTween>
                (rpos, containerFadeTime).scaledTime = false;

            right.Tween<Transform, Vector3, VectorRotationTween>
                (new Vector3(0, 0, 360), containerFadeTime, callback:
                () => right.rotation = Quaternion.Euler(0, 0, 0))
                .scaledTime = false;
        }
    }
}
