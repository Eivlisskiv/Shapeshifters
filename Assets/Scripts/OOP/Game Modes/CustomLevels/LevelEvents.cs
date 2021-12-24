﻿using IgnitedBox.EventSystem;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Position;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns;
using UnityEngine;

namespace Scripts.OOP.Game_Modes.Story
{
    public partial class CustomLevel
    {
        public EventsHandler<System.Type> ObjectiveEvents
        { get; private set; } = new EventsHandler<System.Type>();

        public override void MapEntered(RoomHandler room, Collider2D subject)
        {
            //The next map is the one we want to enter
            if (room != map.loading) return;
            PlayerController player = subject.gameObject.GetComponent<PlayerController>();
            if (!player || playersReady.Contains(player)) return;

            int count = playersReady.Count + 1;

            if (count == GetTeam(0).Count)
            {
                map.current.OpenGate(false);
                playersReady.Clear();
                NextMap();
            }
            else playersReady.Add(player);

            ObjectiveEvents.Invoke(typeof(Reach_Map), this, count);

        }

        public override void MapExited(RoomHandler room, Collider2D subject)
        {
            //The next map is the one we want to enter
            if (room != map.loading) return;
            PlayerController player = subject.gameObject.GetComponent<PlayerController>();
            if (!player) return;

            playersReady.Remove(player);

            ObjectiveEvents.Invoke(typeof(Reach_Map), this, playersReady.Count);

        }

        public override void MemberDestroyed(BaseController member)
        {
            base.MemberDestroyed(member);

            Score++;

            ObjectiveEvents.Invoke(typeof(IControllerElimenated), this, member);
        }
    }
}