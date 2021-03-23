using System;
using UnityEngine;

namespace Scripts.OOP.UI
{
    [Serializable]
    public class PlayerCamera
    {
        const float speed = 0.5f;

        float sizeX;
        float sizeY;

        Camera cam;
        Transform background;

        Vector2 last;

        public PlayerCamera(Camera cam, float size)
        {
            this.cam = cam;
            cam.orthographicSize = size;

            background = cam.transform.GetChild(0).transform;
            float s = size / 3f;
            sizeY = s * 2;
            sizeX = s * 2.5f;
            background.localScale = new Vector3(s, s, 1);
        }

        public Vector3 MouseToWorld()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = cam.nearClipPlane;
            return cam.ScreenToWorldPoint(mousePos);
        }

        public void Update(Vector2 pos)
        {
            cam.transform.position = new Vector3(
                pos.x, pos.y, -10);

            Vector2 m = pos - last;
            Vector3 c = background.localPosition;
            background.localPosition = new Vector3(
                Coord(c.x, m.x, sizeX),
                Coord(c.y, m.y, sizeY), 
                50);

            last = pos;
        }

        private float Coord(float c, float m, float max)
            => Mathf.Clamp(c - (m * speed), -max, max);

        public void Detach()
        {
            background.localPosition = new Vector3(0, 0, 50);
        }
    }
}
