using RimWorld;
using UnityEngine;
using Verse;

namespace SimonSays
{
	public class Dialog : Window
	{
		public override Vector2 InitialSize => new(480f, 200f);
		public override string CloseButtonText => "OK".Translate();
		public override bool IsDebug => true;

		public Dialog() : base()
		{
			doCloseButton = true;
			closeOnClickedOutside = false;
			forcePause = true;
			soundAppear = Resources.tasks[Simon.Instance.nextTask].sound;
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (Simon.Instance.nextTask == -1)
				return;
			var rect = inRect.LeftPartPixels(64).TopPartPixels(64);
			Widgets.DrawTextureFitted(rect, Textures.simon, 1f);
			rect = inRect.RightPartPixels(inRect.width - 64 - Margin);
			Widgets.Label(rect, $"Simon Says: {Resources.tasks[Simon.Instance.nextTask].title}");
		}

		public override void PostClose() => Simon.Instance.StartTask();
	}
}