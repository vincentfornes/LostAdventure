
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LostAdventureTest
{
	public class SpriteAnimator
	{
		private readonly Image target;
		private readonly Dictionary<string, BitmapImage[]> sequences = new();
		private string? current;
		private int frameIndex = 0;
		private double fps = 10.0;
		private bool loop = true;
		private DateTime lastFrameTime = DateTime.UtcNow;
		private Action? onSequenceComplete;

		public SpriteAnimator(Image targetImage) { target = targetImage; }

		public void DefineSequence(string name, string uriPattern, int frameCount)
		{
			var list = new BitmapImage[frameCount];
			for (int i = 0; i < frameCount; i++)
			{
				int idx = i + 1;
				list[i] = new BitmapImage(new Uri(string.Format(uriPattern, idx)));
			}
			sequences[name] = list;
		}

		public void Play(string name, double fps = 10.0, bool loop = true, Action? onComplete = null)
		{
			if (!sequences.ContainsKey(name)) return;
			current = name;
			frameIndex = 0;
			this.fps = fps;
			this.loop = loop;
			onSequenceComplete = onComplete;
			target.Source = sequences[current][frameIndex];
			lastFrameTime = DateTime.UtcNow;
		}

		public void Update()
		{
			if (current == null) return;
			double frameDurationMs = 1000.0 / fps;
			if ((DateTime.UtcNow - lastFrameTime).TotalMilliseconds >= frameDurationMs)
			{
				frameIndex++;
				var frames = sequences[current];
				if (frameIndex >= frames.Length)
				{
					if (loop) frameIndex = 0;
					else
					{
						frameIndex = frames.Length - 1;
						var complete = onSequenceComplete;
						onSequenceComplete = null;
						current = null;
						complete?.Invoke();
						return;
					}
				}
				target.Source = frames[frameIndex];
				lastFrameTime = DateTime.UtcNow;
			}
		}

		public bool IsPlaying => current != null;
		public string? Current => current;
	}
}

