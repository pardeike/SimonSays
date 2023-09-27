using RimWorld;
using UnityEngine;
using Verse;

namespace SimonSays
{
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

		public override void PostClose()
		{
			Resources.tasks[task].startAction?.Invoke();
		}
	}
}