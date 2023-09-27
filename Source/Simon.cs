using RimWorld;
using System.Threading;
using System.Timers;
using UnityEngine;
using Verse;

namespace SimonSays
{
	public class Simon : IExposable
	{
		public static Simon Instance = new();

		public bool firstTime = true;

		public Simon()
		{
		}

		public void ExposeData()
		{
		}

		public void Tick()
		{
			if (firstTime)
			{
				Find.WindowStack.Add(new Dialog(0));
				firstTime = false;
			}
		}
	}

	public class Dialog : Window
	{
		public int task;

		public override Vector2 InitialSize => new(480f, 200f);
		public override string CloseButtonText => "OK".Translate();
		public override bool IsDebug => true;

		public Dialog(int task) : base()
		{
			this.task = task;
			doCloseButton = true;
			closeOnClickedOutside = false;
			forcePause = true;
			soundAppear = Resources.tasks[task].sound;
		}

		public override void DoWindowContents(Rect inRect)
		{
			var rect = inRect.LeftPartPixels(64).TopPartPixels(64);
			Widgets.DrawTextureFitted(rect, Textures.simon, 1f);
			rect = inRect.RightPartPixels(inRect.width - 64 - Margin);
			Widgets.Label(rect, $"Simon Says: {Resources.tasks[task].title}");
		}
	}
}