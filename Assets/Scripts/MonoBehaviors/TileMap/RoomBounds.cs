using UnityEngine;

namespace Scripts.MonoBehaviors.TileMap
{
    public class RoomBounds : MonoBehaviour
    {
        public RoomHandler room;

        private void OnTriggerEnter2D(Collider2D collider)
            => OOP.Game_Modes.GameModes.GameMode?.MapEntered(room, collider);

        private void OnTriggerExit2D(Collider2D collider)
            => OOP.Game_Modes.GameModes.GameMode?.MapExited(room, collider);

    }
}
