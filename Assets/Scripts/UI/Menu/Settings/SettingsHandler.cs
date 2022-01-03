using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Ui.Settings
{
    public class SettingsHandler : MonoBehaviour
    {
        public Toggle inverter;

        private void Start()
        {
            float aimOffset = PlayerPrefs.GetFloat("AimOffset");
            inverter.isOn = aimOffset > 0;
        }

        public void ToggleInvertAim()
        {
            PlayerPrefs.SetFloat("AimOffset", inverter.isOn ? 180 : 0);
        }        

    }
}
