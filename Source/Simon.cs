using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SimonSays
{
	public class Simon : IExposable
	{
		public static Simon Instance = new();

		public const int interval = 6;
		static int lastMinute = -1; // transient

		public bool firstTime = true;
		public int currentTask = -1;
		public int nextTask = -1;
		public long endTimeTicks = 0;

		[DllImport("user32.dll")]
		static extern bool GetCursorPos(out Point pos);
		[DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);

		[StructLayout(LayoutKind.Sequential)]
		public struct Point
		{
			public int x;
			public int y;
		}

		public void ExposeData()
		{
			Scribe_Values.Look(ref firstTime, "firstTime", true);
			Scribe_Values.Look(ref currentTask, "currentTask", -1);
			Scribe_Values.Look(ref nextTask, "nextTask", -1);
			Scribe_Values.Look(ref endTimeTicks, "endTimeTicks", 0);
		}

		public static void StartTask(int task)
		{
			Resources.tasks[task].sound.PlayOneShotOnCamera();
			Instance.StopTask(false);
			LongEventHandler.ExecuteWhenFinished(() =>
			{
				Instance.nextTask = task;
				Instance.StartTask();
			});
		}

		public void StartTask()
		{
			currentTask = nextTask;
			nextTask = -1;
			endTimeTicks = DateTime.UtcNow.AddSeconds(Resources.tasks[currentTask].duration).Ticks;
			Resources.tasks[currentTask].startAction?.Invoke();
		}

		public void StopTask(bool playThankYou = true)
		{
			if (currentTask == -1)
				return;

			var task = Resources.tasks[currentTask];
			task.endAction?.Invoke();
			endTimeTicks = -1;
			currentTask = -1;

			if (playThankYou && task.duration > 0)
				Defs.Thankyou.PlayOneShotOnCamera();
		}

		public void OnGUI()
		{
			if (currentTask != -1)
				Resources.tasks[currentTask].eventAction?.Invoke();
		}

		public void Tick()
		{
			var now = DateTime.UtcNow;
			if (endTimeTicks > 0 && now.Ticks > endTimeTicks)
				StopTask();

			if (currentTask != -1)
				Resources.tasks[currentTask].tickAction?.Invoke();

			if (currentTask != -1 || nextTask != -1)
				return;
			var minute = now.Minute;
			if (minute == lastMinute)
				return;
			lastMinute = minute;

			var seed = (int)(now - new DateTime(1970, 1, 1)).TotalMinutes;
			var rng = new System.Random(seed);
			var delta = rng.Next(0, interval - 2);
			if (minute % interval == delta)
			{
				if (firstTime)
				{
					nextTask = 0;
					firstTime = false;
					Find.WindowStack.Add(new Dialog());
				}
				else if (Current.ProgramState == ProgramState.Playing)
				{
					nextTask = 1 + rng.Next(Resources.tasks.Length - 1);
					Find.WindowStack.Add(new Dialog());
				}
			}
		}

		public static void Play3XSpeed() // 1
		{
			Find.TickManager.curTimeSpeed = TimeSpeed.Superfast;
		}

		public static void ColonistsHidden() // 2
		{
			Find.Selector.ClearSelection();
		}

		public static void RightClickColonistDies() // 3
		{
			if (Event.current.type == EventType.MouseDown)
				if (Event.current.button == 1)
				{
					Event.current.Use();

					var map = Find.CurrentMap;
					if (map == null)
						return;
					var colonists = map.mapPawns.FreeColonists;
					if (colonists.Count == 0)
						return;
					colonists.RandomElement().Kill(null);
				}
		}

		public static void DoNotMoveMap()
		{
			CameraDriver_Update_Patch.Remember(0);
		}

		public static void NoMapSelect() // 5
		{
			SelectionDrawer.Clear();
			var selector = Find.Selector;
			selector.selected.Clear();
			selector.gotoController.Deactivate();
		}

		public static void MixUpColonists() // 6
		{
			FloatMenuMakerMap_GetOptions_Patch.offset = new System.Random().Next(0, 1000);
		}

		public static void FullyZoomedOut() // 7
		{
			var f = Find.CameraDriver.config.sizeRange.max;
			CameraDriver_Update_Patch.Remember(f);
		}

		public static void FullyZoomedIn() // 8
		{
			var f = Find.CameraDriver.config.sizeRange.min;
			CameraDriver_Update_Patch.Remember(f);
		}

		public static void RenameColonists() // 12
		{
			var bar = Find.ColonistBar;
			var colonists = Find.Maps.SelectMany(map => map.mapPawns.FreeColonists).ToArray();
			var order = new int[colonists.Length];
			for (var i = 0; i < order.Length; i++)
				order[i] = i;
			order.Shuffle();
			for (var i = 0; i < order.Length; i++)
				colonists[i].playerSettings.displayOrder = order[i];
			Find.ColonistBar.MarkColonistsDirty();
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
		}

		public static CellBoolDrawer tunnelVision;
		public static Map tunnelVisionMap;
		public static IntVec3 lastMouseCell = IntVec3.Invalid;
		public class TunnelDrawer : ICellBoolGiver
		{
			public static bool IsVisible(IntVec3 cell) => cell.DistanceTo(UI.MouseCell()) > 10f;

			public Color Color => Color.black;
			public bool GetCellBool(int index) => IsVisible(Find.CurrentMap.cellIndices.IndexToCell(index));
			public Color GetCellExtraColor(int index) => Color.black;
		}

		public static void TunnelVision() // 14
		{
			var map = Find.CurrentMap;
			if (tunnelVisionMap != map)
			{
				tunnelVision = new CellBoolDrawer(new TunnelDrawer(), map.Size.x, map.Size.z, 6000, 1f);
				tunnelVisionMap = map;
			}
			tunnelVision.CellBoolDrawerUpdate();
			tunnelVision.MarkForDraw();

			var cell = UI.MouseCell();
			if (lastMouseCell != cell)
			{
				lastMouseCell = cell;
				tunnelVision.SetDirty();
			}

			tunnelVision.CellBoolDrawerUpdate();
			tunnelVision.MarkForDraw();
		}

		static IntVec2 lastRealMousePosition = IntVec2.Invalid;
		static Vector2 currentExactPosition;
		//static float nextDestinationTime;

		public static void DrunkenMouse() // 15
		{
			GetCursorPos(out Point point);
			var currentRealPos = new IntVec2(point.x, point.y);

			if (lastRealMousePosition.IsValid == false)
				currentExactPosition = currentRealPos.ToVector2();
			else
				currentExactPosition += (currentRealPos - lastRealMousePosition).ToVector2();

			var a = Time.realtimeSinceStartup;
			var r = Mathf.Sin(0.79f * a) * Mathf.Cos(0.17f * a) / 6f;
			currentExactPosition += new Vector2(Mathf.Sin(4.7f * a), Mathf.Cos(3.9f * a)) * r;
			var x = (int)(currentExactPosition.x + 0.5f);
			var y = (int)(currentExactPosition.y + 0.5f);
			SetCursorPos(x, y);

			lastRealMousePosition = new IntVec2(x, y);
		}

		public static void CloseMeansBreak() // 16
		{
			Pawn_Position_Patch.closenessReady.Clear();
		}

		public static void WallBreaking() // 17
		{
			Find.Maps.SelectMany(map => map.listerBuildings.allBuildingsColonist).Do(building => building.hitPointsInt = 1);
		}

		public static void MidasTouchStart() // 18
		{
			var colonists = Find.Maps.SelectMany(map => map.mapPawns.FreeColonists.Where(p => p.IsColonistPlayerControlled));
			if (colonists.TryRandomElement(out var pawn))
				Pawn_Position_Patch.midas = pawn;
		}
		public static void MidasTouchEnd()
		{
			Pawn_Position_Patch.midas = null;
		}

		static Vector3 baseRotation; // 19
		static float angle, farClipPlane;
		public static void FlipMapStart()
		{
			baseRotation = Find.CameraDriver.MyCamera.transform.eulerAngles;
			angle = 0;
			Find.CameraDriver.MyCamera.transform.rotation = Quaternion.Euler(baseRotation + new Vector3(0, 0, 0));
			farClipPlane = Find.CameraDriver.MyCamera.farClipPlane;
			Find.CameraDriver.MyCamera.farClipPlane += 50f;
		}
		public static void FlipMapTick()
		{
			angle += 0.1f;
			var x = Mathf.Sin(angle) * 3;
			var z = Mathf.Cos(angle) * 3;
			Find.CameraDriver.MyCamera.transform.rotation = Quaternion.Euler(baseRotation + new Vector3(x, 0, z));
		}
		public static void FlipMapEnd()
		{
			Find.CameraDriver.MyCamera.transform.rotation = Quaternion.Euler(baseRotation);
			Find.CameraDriver.MyCamera.farClipPlane = farClipPlane;
		}

		public static CellBoolDrawer reverseTunnelVision;
		public static Map reverseTunnelVisionMap;
		public class ReverseTunnelDrawer : ICellBoolGiver
		{
			public static bool IsVisible(IntVec3 cell) => cell.DistanceTo(UI.MouseCell()) < 10f;

			public Color Color => Color.black;
			public bool GetCellBool(int index) => IsVisible(Find.CurrentMap.cellIndices.IndexToCell(index));
			public Color GetCellExtraColor(int index) => Color.black;
		}

		public static void ReverseTunnelVision() // 20
		{
			var map = Find.CurrentMap;
			if (reverseTunnelVisionMap != map)
			{
				reverseTunnelVision = new CellBoolDrawer(new ReverseTunnelDrawer(), map.Size.x, map.Size.z, 6000, 1f);
				reverseTunnelVisionMap = map;
			}
			reverseTunnelVision.CellBoolDrawerUpdate();
			reverseTunnelVision.MarkForDraw();

			var cell = UI.MouseCell();
			if (lastMouseCell != cell)
			{
				lastMouseCell = cell;
				reverseTunnelVision.SetDirty();
			}

			reverseTunnelVision.CellBoolDrawerUpdate();
			reverseTunnelVision.MarkForDraw();
		}

		public static void ReverseTunnelStart()
		{
			Cursor.visible = false;
		}

		public static void ReverseTunnelEnd()
		{
			Cursor.visible = true;
		}
	}
}