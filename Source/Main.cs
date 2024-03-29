using HarmonyLib;
using RimWorld.Planet;
using Verse;
using Brrainz;
using LudeonTK;

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
		[DebugAction("Simon Says", "Do: Task 01", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task01() => Simon.StartTask(1);
		[DebugAction("Simon Says", "Do: Task 02", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task02() => Simon.StartTask(2);
		[DebugAction("Simon Says", "Do: Task 03", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task03() => Simon.StartTask(3);
		[DebugAction("Simon Says", "Do: Task 04", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task04() => Simon.StartTask(4);
		[DebugAction("Simon Says", "Do: Task 05", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task05() => Simon.StartTask(5);
		[DebugAction("Simon Says", "Do: Task 06", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task06() => Simon.StartTask(6);
		[DebugAction("Simon Says", "Do: Task 07", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task07() => Simon.StartTask(7);
		[DebugAction("Simon Says", "Do: Task 08", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task08() => Simon.StartTask(8);
		[DebugAction("Simon Says", "Do: Task 09", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task09() => Simon.StartTask(9);
		[DebugAction("Simon Says", "Do: Task 10", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task10() => Simon.StartTask(10);
		[DebugAction("Simon Says", "Do: Task 11", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task11() => Simon.StartTask(11);
		[DebugAction("Simon Says", "Do: Task 12", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task12() => Simon.StartTask(12);
		[DebugAction("Simon Says", "Do: Task 13", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task13() => Simon.StartTask(13);
		[DebugAction("Simon Says", "Do: Task 14", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task14() => Simon.StartTask(14);
		[DebugAction("Simon Says", "Stop Task", false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)] public static void Task15() => Simon.Instance.StopTask(true);
	}
}