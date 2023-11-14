namespace Manager;
public partial class MusicPlayer : AudioStreamPlayer2D
{
	[Export] private AudioStream mainMusicTrack;
	[Export] private AudioStream bossMusicTracks;
	public override void _Ready()
	{
		GameEvents.Instance.WaveCleared += OnWaveCleared;
	}	
	
	public override void _ExitTree()
    {
		GameEvents.Instance.WaveCleared -= OnWaveCleared;
    }

	private void OnWaveCleared(int waveNumber)
	{
		if(waveNumber == 10)
		{
			Stop();
			Stream = bossMusicTracks;
			Play();
		}
	}
}
