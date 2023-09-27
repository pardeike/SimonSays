using System;
using Verse;

namespace SimonSays
{
	public class Simon : IExposable
	{
		public static Simon Instance = new();

		public const int interval = 7;
		public bool firstTime = true;
		public int lastMinute = -1;
		public int lastMinuteInterval = -1;
		public int currentTask = 0;
		public int minuteOffset = 0;

		public Simon()
		{
		}

		public void ExposeData()
		{
			Scribe_Values.Look(ref firstTime, "firstTime", true);
			Scribe_Values.Look(ref lastMinute, "lastMinute", -1);
			Scribe_Values.Look(ref lastMinuteInterval, "lastMinuteInterval", -1);
			Scribe_Values.Look(ref currentTask, "currentTask", 0);
			Scribe_Values.Look(ref minuteOffset, "minuteOffset", 0);
		}

		public void Tick()
		{
			var now = DateTime.UtcNow;
			var minute = now.Minute;
			if (lastMinute == -1)
				lastMinute = minute;
			if (minute == lastMinute)
				return;
			lastMinute = minute;
			var minuteInterval = ((minute + minuteOffset) / interval) * interval;
			if ((minute + minuteOffset + (100 * interval)) % interval == 0)
			{
				if (minuteInterval == lastMinuteInterval)
					return;
				lastMinuteInterval = minuteInterval;

				var seed = (int)(now - new DateTime(1970, 1, 1)).TotalMinutes;
				var rng = new Random(seed);
				currentTask = firstTime ? 0 : 1 + rng.Next(Resources.tasks.Length - 1);
				firstTime = false;
				minuteOffset = rng.Next(-2, 2);
			}
		}

		// You will follow my rules now
		public static void Task01()
		{

		}

		// Play on 3x speed
		public static void Task02()
		{

		}

		// Your colonists are hidden
		public static void Task03()
		{

		}

		// Do not right-click or a colonist dies
		public static void Task04()
		{

		}

		// Who needs to move the map
		public static void Task05()
		{

		}

		// Selecting stuff on the map is stupid
		public static void Task06()
		{

		}

		// Commands to colonists are mixed up
		public static void Task07()
		{

		}

		// Play fully zoomed out
		public static void Task08()
		{

		}

		// Play fully zoomed in
		public static void Task09()
		{

		}

		// Your colonists will never walk more than 10 cells
		public static void Task10()
		{

		}

		// Up and Down, Left and Right. Who cares
		public static void Task11()
		{

		}

		// Colonists need to be selected to move around
		public static void Task12()
		{

		}

		// All colonists are now called 'Simon'
		public static void Task13()
		{

		}

		// They will only do stuff when not in sight
		public static void Task14()
		{

		}
	}
}