using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicEngine : MonoBehaviour {

	public AudioClip musicA;
	public float musicAFadeInTime;
	public float musicAFadeOutTime;

	public AudioClip musicB;
	public float musicBFadeInTime;
	public float musicBFadeOutTime;

	public AudioClip transitionAToB;
	public float nextSectionStartTimeAfterAtoBTransitionStart;
	public AudioClip transitionBToA;
	public float nextSectionStartTimeAfterBtoATransitionStart;

	public AudioSource musicAAudioSource;
	public AudioSource musicBAudioSource;

	public AudioSource transitionAudioSource;

	//public AudioMixerSnapshot musicBusAOn;
	//public AudioMixerSnapshot musicBusAOff;
	//public AudioMixerSnapshot musicBusBOn;
	//public AudioMixerSnapshot musicBusBOff;
	public AudioMixer musicMixerBusA;
	public AudioMixer musicMixerBusB;

	private float timeElapsed = 0.0f;
	private float nextPlayTime = 0.0f;
	private bool playingTransition = false;
	private bool musicBusA = true;

	public bool useIntroduction = false;
	public AudioClip introductionAudioClip;
	public float introductionOverlapTime = 0.0f;



	// Use this for initialization
	void Awake () {
		Debug.Log ("Music engine is awake");
		
		if(useIntroduction && introductionAudioClip != null){
			//hooray we have an introduction
			transitionAudioSource.clip = introductionAudioClip;
			StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", 0.0f, 0.0f));
			StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusB, "masterVolume", 0.0f, 0.0f));

		} else { 
			musicAAudioSource.clip = musicA;
			musicBAudioSource.clip = musicB;
			StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", 0.0f, 0.0f));
			StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusB, "masterVolume", 0.0f, 0.0f));
		} 
			

	}

	void Start () {
		Debug.Log ("Music engine has started");
		StartCoroutine(StartMusic());

	}

	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator StartMusic()
	{ 
		yield return new WaitForSeconds(1.0f);

		if (useIntroduction && introductionAudioClip != null)
		{
			Debug.Log("Playing introduction.");
			musicAAudioSource.Stop();
			musicBAudioSource.Stop();
			transitionAudioSource.Play();
			nextPlayTime = introductionAudioClip.length - introductionOverlapTime;
			Debug.Log("nextPlayTime: " + nextPlayTime);
			musicBusA = false;
			playingTransition = true;
		}
		else
		{
			Debug.Log("No introduction.");
			playingTransition = false;
			musicAAudioSource.Play();
			StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", musicAFadeInTime, 80.0f));
		}
	}

	// 
	void FixedUpdate () {
		timeElapsed += Time.deltaTime;
		if(playingTransition){
			if (timeElapsed > nextPlayTime) {
				//play the next piece of music
				if (musicBusA) {
					FinishSwitchingToB ();
					musicBusA = false;
					musicAAudioSource.Stop ();
				
					StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", 0.0f, 0.0f));
				} else {
					FinishSwitchingToA ();
					musicBusA = true;
					musicBAudioSource.Stop ();
					
					StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusB, "masterVolume", 0.0f, 0.0f));
				}
					
				playingTransition = false;
			}
		}
	}

	void OnDestroy(){
		
		//StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", 0.0f, 0.0f));
		
		//StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusB, "masterVolume", 0.0f, 0.0f));
		Debug.Log ("The music engine object has been destroyed.");
	}


	public void SwitchToMusicB(){
		if (!musicBusA) {
			return;
		}

		Debug.Log ("SwitchToMusicB");
		timeElapsed = 0.0f;
		nextPlayTime = nextSectionStartTimeAfterAtoBTransitionStart;

		//begin playing the transition
		transitionAudioSource.clip = transitionAToB;
		transitionAudioSource.Play ();
		playingTransition = true;

		//fade out music bus A
		
		StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", musicAFadeOutTime, 0.0f));

		//musicBusBOn.TransitionTo (0.0f);

	}

	void FinishSwitchingToB() {
		Debug.Log ("Finish switching to B");
		musicBAudioSource.clip = musicB;
		musicBAudioSource.Play ();
		
		StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusB, "masterVolume", musicBFadeInTime, 80.0f));

	}

	public void SwitchToMusicA(){
		if (musicBusA) {
			return;
		}
		Debug.Log ("SwitchToMusicA");
		timeElapsed = 0.0f;
		nextPlayTime = nextSectionStartTimeAfterBtoATransitionStart;

		//begin playing the transition
		transitionAudioSource.clip = transitionBToA;
		transitionAudioSource.Play ();
		playingTransition = true;

		//fade out music bus A
		
		StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusB, "masterVolume", musicBFadeOutTime, 0.0f));

		//musicBusBOn.TransitionTo (0.0f);

	}

	void FinishSwitchingToA() {
		Debug.Log ("Finish switching to A");
		musicAAudioSource.clip = musicA;
		musicAAudioSource.Play ();
		
		StartCoroutine(FadeMixerGroup.StartFade(musicMixerBusA, "masterVolume", musicAFadeInTime, 80.0f));

	}




}
