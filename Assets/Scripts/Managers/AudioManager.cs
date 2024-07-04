using UnityEngine;
using YG;

namespace RaspberryGames.BlockPuzzle
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField] private AudioSource soundSource;
		[SerializeField] private AudioSource musicSource;
		[Header("Sounds")]
		[SerializeField] private AudioClip clickSound;
		[SerializeField] private AudioClip winSound;
				
		private void Awake()
		{
		}
		
		private void OnEnable()
		{
			YandexGame.GetDataEvent += Init;
		}

		private void OnDisable()
		{
			YandexGame.GetDataEvent -= Init;
		}
		
		private void Init()
		{
			soundSource.mute = !YandexGame.savesData.isSoundOn;
			musicSource.mute = !YandexGame.savesData.isMusicOn;
		}
		
		public void SwitchSound()
		{
			YandexGame.savesData.isSoundOn = !YandexGame.savesData.isSoundOn;
			soundSource.mute = !YandexGame.savesData.isSoundOn;
		}
		
		public void SwitchMusic()
		{
			YandexGame.savesData.isMusicOn = !YandexGame.savesData.isMusicOn;
			musicSource.mute = !YandexGame.savesData.isMusicOn;
		}

		
		public void PlaySound(AudioClip clip)
		{
			soundSource.PlayOneShot(clip);
		}
		
		public void PlaySoundWithRandomPitch(AudioClip clip, float pitchOffset)
		{
			soundSource.pitch = 1f + Random.Range(-pitchOffset, pitchOffset);
			PlaySound(clip);
			soundSource.pitch = 1f;
		}
	}
}

