using Scripts.OOP.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Menu.Settings
{
    public class VolumeControl : MonoBehaviour
    {
        public enum VolumeType { Effects, Music }

        public VolumeType volumeType;

        public Slider slider;
        public InputField field;

        private int lastValue;

        private void Start()
        {
            VolumeSetting set = GetSetting();
            lastValue = (int)(set.Volume * 100);
            slider.value = lastValue;
            field.text = lastValue.ToString();
        }

        private VolumeSetting GetSetting()
        {
            switch (volumeType)
            {
                case VolumeType.Effects: return VolumeSetting.Effect;
                case VolumeType.Music: return VolumeSetting.Music;
            }

            return null;
        }

        private void SetValue(int value)
        {
            lastValue = value;
            VolumeSetting set = GetSetting();
            set.Volume = value / 100f;
        }

        public void InputChanged()
        {
            if (!int.TryParse(field.text, out int value))
            {
                field.text = lastValue.ToString();
                return;
            }

            if(value < 0)
            {
                value = 0;
                field.text = "0";
            }
            else if (value > 100)
            {
                value = 100;
                field.text = "100";
            }

            slider.value = value;

            SetValue(value);
        }

        public void SliderChanged()
        {
            int value = (int)slider.value;

            if (value < 0)
            {
                value = 0;
                slider.value = 0;
            }
            else if (value > 100)
            {
                value = 100;
                slider.value = 100;
            }

            field.text = value.ToString();

            SetValue(value);
        }
    }
}
