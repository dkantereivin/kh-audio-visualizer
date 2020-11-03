using Godot;
using System;

public class AudioStreamPlayer : Godot.AudioStreamPlayer
{
	private const int RANGE_COUNT = 20;
	private const int MIN_FREQ = 20;
	private const int MAX_FREQ = 20000;


	private int INTERVAL = Mathf.RoundToInt((MAX_FREQ - MIN_FREQ) / RANGE_COUNT);

	private AudioEffectSpectrumAnalyzerInstance _analyzer =
		(AudioEffectSpectrumAnalyzerInstance) AudioServer.GetBusEffectInstance(0, 0);

	private AudioStreamPlayer()
	{
		if (INTERVAL < 1)
			throw new Exception("AudioStreamPlayer.Interval must be set greater than 0.");
	}

	public override void _Ready()
	{
		this.Play();
	}

	public override void _Process(float delta)
	{
		GD.Print(GetInstantaneousMagnitudes()[19]);
	}

	/**
	 * Returns a float array of length RANGE_COUNT with the float magnitude of each spectrum range as provided by Godot.
	 * * TODO: Should GetMagnitudeForFrequencyRange use Average or Max (default) magnitude calculation mechanism?
	 * * TODO: Add conversation from Db to a non-relative/absolute representation of volume.
	 */
	public float[] GetInstantaneousMagnitudes()
	{
		var ranges = new float[RANGE_COUNT];
		for (var i = 0; i < RANGE_COUNT; i++)
		{
			float rangeMin = MIN_FREQ + INTERVAL * i;
			Vector2 magnitude = _analyzer.GetMagnitudeForFrequencyRange(rangeMin, rangeMin + INTERVAL);
			float db = GD.Linear2Db(magnitude.Length());
			ranges[i] = db;
		}
		return ranges;
	}
}
