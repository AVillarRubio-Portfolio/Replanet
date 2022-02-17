using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class soundParameters
{
    public FMODUnity.StudioEventEmitter emitter;
    public float delay;
}

public class SoundManager : MonoBehaviour
{
    #region instance
    private static SoundManager instance = null;

	List<soundParameters> sounds;

	// Game Instance Singleton
	public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            DestroyImmediate(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

		sounds = new List<soundParameters>();

	}
	#endregion

	public void PlaySound(FMODUnity.StudioEventEmitter emitter, string ev)
    {
		emitter.Stop();
		emitter.Event = ev;
		emitter.Play();
	}

    public void PlaySoundWithDelay(FMODUnity.StudioEventEmitter emitter, float delay)
    {
		soundParameters aux_ = new soundParameters();
		aux_.emitter = emitter;
		aux_.delay = delay;
		sounds.Add(aux_);
	}
}
