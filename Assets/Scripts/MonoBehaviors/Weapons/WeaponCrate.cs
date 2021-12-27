using Scripts.OOP.Perks;
using Scripts.OOP.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCrate : MonoBehaviour
{
    public SpriteRenderer icon;

    private Type weapon;

    private bool shuffle = true;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        RandomType();
        time = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shuffle) return;
        if(time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        RandomType();
    }

    private void RandomType()
    {
        var n = Weapon.Types.RandomElement();
        if (n != weapon)
        {
            weapon = n;
            icon.sprite = Weapon.GetIcon(weapon);
        }
        time = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerController player)) return;

        if (shuffle) shuffle = false;

        player.SetWeapon(weapon);
        player.AddPerk(PerksHandler.RandomCharge(player.Level, 10));

        Destroy(gameObject);

        //Open ui
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerController player)) return;

        if(shuffle) shuffle = false;

        //Open ui
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerController player)) return;


        //close ui
    }
}
