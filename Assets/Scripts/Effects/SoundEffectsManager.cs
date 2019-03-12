using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour {

	public AudioClip musicClip;

	public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
	public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
	public static SoundEffectsManager instance = null;     //Allows other scripts to call functions from SoundManager.             
	public float lowPitchRange = .97f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.02f;            //The highest a sound effect will be randomly pitched.

	public AudioClip soundSwingLight0;
	public AudioClip soundSwingLight1;
	public AudioClip soundSwingLight2;
	public AudioClip soundSwingLight3;
	public AudioClip soundSwingMedium0;
    public AudioClip soundSwingMedium1;
    public AudioClip soundSwingMedium2;
	public AudioClip soundSwingMedium3;
	public AudioClip soundSwingHard0;
    public AudioClip soundSwingHard1;
    public AudioClip soundSwingHard2;
	public AudioClip soundSwingHard3;

	public AudioClip soundHitLight0;
    public AudioClip soundHitLight1;
    public AudioClip soundHitLight2;
    public AudioClip soundHitLight3;
    public AudioClip soundHitMedium0;
    public AudioClip soundHitMedium1;
    public AudioClip soundHitMedium2;
    public AudioClip soundHitMedium3;
    public AudioClip soundHitHard0;
    public AudioClip soundHitHard1;
    public AudioClip soundHitHard2;
    public AudioClip soundHitHard3;
    public AudioClip soundExActivate;
	public AudioClip[] swingSoundEffects;
	public AudioClip[] hitSoundEffects;
	
	void Start() {
		//Play the clip.
		float randomStartingTime = Random.Range(0.0f, 60.0f);
		musicSource.clip = musicClip;
		musicSource.time = randomStartingTime;
		musicSource.volume = 0.4f;
		musicSource.loop = true;
		musicSource.Play();
		hitSoundEffects = new AudioClip[] {};
		// 0: Light0, 1: Light1, 2: Light2, 3: Light3
		// 4: Medium0, 5: Medium1, 6: Medium2, 7: Medium3
		// 8: Hard0, 9: Hard1, 10: Hard2, 11: Hard3
		swingSoundEffects = new AudioClip[] { soundSwingLight0, soundSwingLight1, soundSwingLight2, soundSwingLight3
											, soundSwingMedium0, soundSwingMedium1, soundSwingMedium2, soundSwingMedium3
											, soundSwingHard0, soundSwingHard1, soundSwingHard2, soundSwingHard3};
		hitSoundEffects = new AudioClip[] { soundHitLight0, soundHitLight1, soundHitLight2, soundHitLight3
											, soundHitMedium0, soundHitMedium1, soundHitMedium2, soundHitMedium3
											, soundHitHard0, soundHitHard1, soundHitHard2, soundHitHard3};

	}
	
	void Awake () {
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);
		
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}
	
	
	//Used to play single sound clips.
	public void PlaySingle(float volume, AudioClip clip){
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;
		
		//Play the clip.
		efxSource.PlayOneShot(clip, volume);
	}
	
	
	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeSfx (float volume = 0.8f, params AudioClip[] clips) {
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);
				
		RandomizePitch(ref efxSource);
		
		//Set the clip to the clip at our randomly chosen index.
		var clip = clips[randomIndex];

		//Play the clip.
		efxSource.PlayOneShot(clip, volume);
	}

	public void PlayRandomSwingSound(float volume = 0.8f, params int[] choices) {
		AudioClip[] audioToPlay = new AudioClip[choices.Length];
		for(int i = 0; i < choices.Length; i++) {			
			audioToPlay[i] = swingSoundEffects[choices[i]];
		}
				
		RandomizePitch(ref efxSource);

		int randomIndex = Random.Range(0, audioToPlay.Length);		
		var clip = audioToPlay[randomIndex];
		efxSource.PlayOneShot(clip, volume);
	}

	public void PlayRandomHitSound(float volume = 0.8f, params int[] choices) {
		AudioClip[] audioToPlay = new AudioClip[choices.Length];
		for(int i = 0; i < choices.Length; i++) {			
			audioToPlay[i] = hitSoundEffects[choices[i]];
		}
				
		RandomizePitch(ref efxSource);
		
		int randomIndex = Random.Range(0, audioToPlay.Length);		
		var clip = audioToPlay[randomIndex];
		efxSource.PlayOneShot(clip, volume);
	}

	public void RandomizePitch(ref AudioSource efxSource) {		
		//Choose a random pitch to play back our clip at between our high and low pitch ranges.		
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);		
		//Set the pitch of the audio source to the randomly chosen pitch.
		efxSource.pitch = randomPitch;
	}

}
