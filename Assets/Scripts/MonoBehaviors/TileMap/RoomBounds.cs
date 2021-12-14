using Scripts.OOP.Game_Modes;
using UnityEngine;

namespace Scripts.MonoBehaviors.TileMap
{
    public class RoomBounds : MonoBehaviour
    {
        public RoomHandler room;

        private void OnTriggerEnter2D(Collider2D collider)
            => GameModes.GameMode?.MapEntered(room, collider);

        private void OnTriggerExit2D(Collider2D collider)
            => GameModes.GameMode?.MapExited(room, collider);

    }
}
