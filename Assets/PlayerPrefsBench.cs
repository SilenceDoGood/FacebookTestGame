using UnityEngine;
using System.Diagnostics;
using System.Collections;

public class PlayerPrefsBench : MonoBehaviour
{
	void Start()
	{
		var stopWatch = new Stopwatch();
		stopWatch.Start();
		for (int i = 0; i < 1000; ++i)
		{
			PlayerPrefs.GetString(i.ToString()); //"Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! Howdy Players, is this your first time playing Gem Junction! Let me show you how to play! ");
		}
		stopWatch.Stop();
		print(stopWatch.Elapsed);
	}
}

