using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.Character.Stats;
using Scripts.OOP.Database;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks;
using Scripts.OOP.Perks.Weapons;
using Scripts.OOP.UI;
using UnityEngine;
using UnityEngine.EventSystems;

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

        GameObject pointer = Resources.Load<GameObject>("UI/ObjectivePointer");
        pointer = Instantiate(pointer, player.transform);
        pointer.transform.localPosition = new Vector3(0, 0, -5);

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
            ui.LevelUp(Level);
            ui.UpdateXP(Xp / XPRequired);
        }

        UpdateHealthBar();
    }

    internal void LoadProgress(StoryProgress previous)
    {
        Level = previous.LevelReached;
        Xp = previous.Experience;

        SerializedPerk[] perks = previous.Perks;
        this.perks = new PerksHandler();
        for (int i = 0; i < perks.Length; i++)
        {
            SerializedPerk sperk = perks[i];
            Perk perk = PerksHandler.Load(sperk.Name);
            perk.LevelUp(sperk.Level);
            perk.ChargeBuff(sperk.Buff, sperk.Charge);
            this.perks.Add(perk, ui);
        }

        for (int i = 0; i < Weapon.Types.Length; i++)
        {
            System.Type type = Weapon.Types[i];
            if (type.Name == previous.Weapon)
                SetWeapon(type);
        }
    }

    public override void OnUpdate()
    {
        cam.Update(transform.position);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            var perk = new Mine_Drop();
            perk.LevelUp(1);
            perks.Add(perk, UI);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            perks.Add(PerksHandler.Random(), UI);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ModifyHealth(50);
        }
#endif
    }

    public override void AddPerk(Perk perk)
        => perks.Add(perk, UI);

    public override bool IsFiring(out float angle)
    {
        if(!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space))
        {
            angle = 0;
            return false;
        }

        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected)
        {
            Debug.Log($"Mouse click hit {selected}");

            angle = 0;
            return false;
        }

        //Vector2 screenPos = Input.mousePosition;

        //Get the world mouse position
        Vector2 mousePos = cam.MouseToWorld();

        //Get the angle relative to the position
        Vector2 pos = transform.position;

        angle = pos.WorldAngle(mousePos);

        return true;
    }

    public override void OnXPChange(bool isUp)
    {
        if (ui)
        {
            if (isUp) ui.LevelUp(Level);
            ui.UpdateXP(Xp / XPRequired);
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
        //if (ui) Destroy(ui.gameObject);
        GameModes.GameMode.PlayerElimenated(this);
    }

    private void OnDestroy()
    {
        if(ui) Destroy(ui.gameObject);
    }
}
