using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.MonoBehaviors.UI.Menu
{
    public class GeneralButtonTriggers : EventTrigger
    {
        public GeneralButton Button { get; set; }

        private void Start()
        {
            if (!Button)
            {
                Button = GetComponent<GeneralButton>();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.touchCount > 0) return;
            if (!Button) return;
            Button.ChangeFocus(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (Input.touchCount > 0) return;
            if (!Button) return;
            Button.ChangeFocus(false);
        }
    }
}
