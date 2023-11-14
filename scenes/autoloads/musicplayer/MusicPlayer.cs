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
		GameEvents.Instance.WaveCleared += OnWaveCleared;
	}	
	
	public override void _ExitTree()
    {
		GameEvents.Instance.WaveCleared -= OnWaveCleared;
    }

	public void StartGame()
	{
		Tween soundTween = CreateTween();
		soundTween.TweenProperty(this, "volume_db", -80, fadeTime);
		soundTween.TweenCallback(Callable.From(FadeInMainGameMusic));
	}
	private void FadeInMainGameMusic()
	{
		Stop();
		Stream = mainMusicTrack;
		Play();
		Tween soundTween = CreateTween();
		soundTween.TweenProperty(this, "volume_db", -20, fadeTime);
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
		if(waveNumber == 2)
		{
			Tween soundTween = CreateTween();
			soundTween.TweenProperty(this, "volume_db", -80, fadeTime);
			soundTween.TweenCallback(Callable.From(FadeInBossGameMusic));
		}
	}
}
