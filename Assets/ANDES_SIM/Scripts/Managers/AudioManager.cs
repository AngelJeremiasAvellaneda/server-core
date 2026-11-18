using UnityEngine;

namespace AndesSim
{
    // Genera la confirmación sonora sin archivos de audio externos.
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private AudioSource source;
        private AudioClip confirmClip;

        private void Awake()
        {
            Instance = this;
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = 0.32f;
            confirmClip = BuildBeep(880f, 0.16f);
        }

        public void PlayConfirm()
        {
            if (source != null && confirmClip != null) source.PlayOneShot(confirmClip);
        }

        private static AudioClip BuildBeep(float frequency, float seconds)
        {
            const int sampleRate = 44100;
            var length = Mathf.CeilToInt(sampleRate * seconds);
            var data = new float[length];
            for (var i = 0; i < length; i++)
            {
                var envelope = 1f - i / (float)length;
                data[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate) * envelope * 0.35f;
            }
            var clip = AudioClip.Create("Andes_Confirm", length, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
