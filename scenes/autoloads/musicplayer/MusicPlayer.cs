namespace Manager;
public partial class MusicPlayer : AudioStreamPlayer2D
{
	[Export] private AudioStream mainScreenTrack;
	[Export] private AudioStream mainMusicTrack;
	[Export] private AudioStream bossMusicTracks;
	public static MusicPlayer Instance { get; private set; }
	private float volume_db = -20;
	private float fadeTime = 2f;
	public override void _Ready()
	{
		Instance = this;
		VolumeDb = volume_db;
	}	

	public void ConnectToSignals()
	{
		GameEvents.Instance.WaveCleared += OnWaveCleared;
	}

	public void DisconnectFromSignals()
	{
		GameEvents.Instance.WaveCleared -= OnWaveCleared;
	}


	public void StartGame()
	{
		Tween soundTween = CreateTween();
								 
		var callable = Callable.From<float>(FadeOutMusic);
		soundTween.TweenMethod(callable,0f,1.0f,fadeTime);
		soundTween.TweenCallback(Callable.From(FadeInMainGameMusic));
	}
	private void FadeInMainGameMusic()
	{
		Stop();
		Stream = mainMusicTrack;
		Play();
		Tween soundTween = CreateTween();
		var callable = Callable.From<float>(FadeInMusic);
		soundTween.TweenMethod(callable,0f,1.0f,fadeTime);
	}

	
	void FadeOutMusic(float percent)
	{
		float value = volume_db -(60 * percent);
		VolumeDb = value;
	}

	void FadeInMusic(float percent)
	{
		float value = ((80+volume_db) * percent) - 80;
		VolumeDb = value;
	}

	private void FadeInBossGameMusic()
	{
		Stop();
		Stream = bossMusicTracks;
		Play();
		Tween soundTween = CreateTween();
		soundTween.TweenProperty(this, "volume_db", -20, fadeTime);
	}

	private void OnWaveCleared(int waveNumber)
	{
		if(waveNumber == 10)
		{
			Tween soundTween = CreateTween();
			soundTween.TweenProperty(this, "volume_db", -80, fadeTime);
			soundTween.TweenCallback(Callable.From(FadeInBossGameMusic));
		}
	}
}
