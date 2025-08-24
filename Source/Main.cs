using Brrainz;
using HarmonyLib;
using LudeonTK;
using RimWorld.Planet;
using Verse;

namespace SimonSays
{
	[StaticConstructorOnStartup]
	public class SimonSays_Main : Mod
	{
		static SimonSays_Main()
		{
			CrossPromotion.Install(76561197973010050);
		}

		public SimonSays_Main(ModContentPack content) : base(content)
		{
			LongEventHandler.ExecuteWhenFinished(() => new Harmony("net.pardeike.simonsays").PatchAll());
		}
	}

	public class SimonSays(World world) : WorldComponent(world)
	{
		public override void WorldComponentTick() => Simon.Instance.Tick();
		public override void ExposeData() => Simon.Instance.ExposeData();
	}

	public static class DebugMenu
	{
		[DebugAction("Simon Says", "Do: Task 01")] public static void Task01() => Simon.StartTask(1);
		[DebugAction("Simon Says", "Do: Task 02")] public static void Task02() => Simon.StartTask(2);
		[DebugAction("Simon Says", "Do: Task 03")] public static void Task03() => Simon.StartTask(3);
		[DebugAction("Simon Says", "Do: Task 04")] public static void Task04() => Simon.StartTask(4);
		[DebugAction("Simon Says", "Do: Task 05")] public static void Task05() => Simon.StartTask(5);
		[DebugAction("Simon Says", "Do: Task 06")] public static void Task06() => Simon.StartTask(6);
		[DebugAction("Simon Says", "Do: Task 07")] public static void Task07() => Simon.StartTask(7);
		[DebugAction("Simon Says", "Do: Task 08")] public static void Task08() => Simon.StartTask(8);
		[DebugAction("Simon Says", "Do: Task 09")] public static void Task09() => Simon.StartTask(9);
		[DebugAction("Simon Says", "Do: Task 10")] public static void Task10() => Simon.StartTask(10);
		[DebugAction("Simon Says", "Do: Task 11")] public static void Task11() => Simon.StartTask(11);
		[DebugAction("Simon Says", "Do: Task 12")] public static void Task12() => Simon.StartTask(12);
		[DebugAction("Simon Says", "Do: Task 13")] public static void Task13() => Simon.StartTask(13);
		[DebugAction("Simon Says", "Do: Task 14")] public static void Task14() => Simon.StartTask(14);
		[DebugAction("Simon Says", "Do: Task 15")] public static void Task15() => Simon.StartTask(15);
		[DebugAction("Simon Says", "Do: Task 16")] public static void Task16() => Simon.StartTask(16);
		[DebugAction("Simon Says", "Do: Task 17")] public static void Task17() => Simon.StartTask(17);
		[DebugAction("Simon Says", "Do: Task 18")] public static void Task18() => Simon.StartTask(18);
		[DebugAction("Simon Says", "Do: Task 19")] public static void Task19() => Simon.StartTask(19);
		[DebugAction("Simon Says", "Do: Task 20")] public static void Task20() => Simon.StartTask(20);
		[DebugAction("Simon Says", "Stop Task")] public static void StopTask() => Simon.Instance.StopTask(true);
	}
}