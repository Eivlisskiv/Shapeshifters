using Scripts.OOP.Character.Stats;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.UI;
using UnityEngine;

public class PlayerController : BaseController
{
    public static PlayerController Instantiate(GameObject bodyPrefab,
        GameObject uiPrefab, Camera cam, Transform uiCanvas)
    {
        GameObject body = Instantiate(bodyPrefab);
        body.name = "Player";
        GameObject ui = Instantiate(uiPrefab, uiCanvas);
        ui.name = $"{body.name}_UI";

        PlayerController player = body.AddComponent<PlayerController>();
        player.cam = new PlayerCamera(cam, 15f);

        player.ui = ui.GetComponent<CharacterUIHandler>();
        var uit = player.ui.GetComponent<RectTransform>();
        uit.anchoredPosition = Vector3.zero;
        uit.localScale = new Vector3(1, 1, 1);

        return player;
    }

    public PlayerCamera cam;

    private CharacterUIHandler ui;
    public CharacterUIHandler UI { get => ui; }

    public override void OnStart() 
    {
        stats = new Stats(200);
        Body.corners = Random.Range(3, 10);

        if (ui)
        {
            SetHealthBar(ui.healthContainer);
            ui.UpdateXP(xp / XPRequired);
        }

        UpdateHealthBar();
    }

    public override void OnUpdate()
    {
        cam.Update(transform.position);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            var perk = new Mine_Drop();
            perk.LevelUp();
            perks.Add(perk, UI);
        }
#endif
    }

    public override bool IsFiring(out float angle)
    {
        if(!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space))
        {
            angle = 0;
            return false;
        }

        //Get the world mouse position
        Vector3 mousePos = cam.MouseToWorld();

        //Get the angle relative to the position
        Vector2 pos = transform.position;
        int y = pos.y > mousePos.y ? -1 : 1;
        angle = Vector2.Angle(Vector2.left, pos - (Vector2)mousePos) * y;

        return true;
    }

    public override void OnXPChange(bool isUp)
    {
        if (ui)
        {
            if (isUp) ui.LevelUp(level);
            ui.UpdateXP(xp / XPRequired);
        }
    }

    public override bool OnDeath()
    {
        GameModes.GameMode.MemberDestroyed(this);
        return true;
    }

    public override void OnDeathEnded()
    {
        base.OnDeathEnded();
        if (ui) Destroy(ui.gameObject);
        GameModes.GameMode.PlayerElimenated(this);
    }
}
