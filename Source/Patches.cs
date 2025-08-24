using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using Verse.AI;

namespace SimonSays
{
	[HarmonyPatch(typeof(Root), nameof(Root.OnGUI))]
	public class Root_OnGUI_Patch
	{
		public static void Prefix() => Simon.Instance.OnGUI();
	}

	[HarmonyPatch(typeof(Root_Entry), nameof(Root_Entry.Update))]
	public class Root_Entry_Update_Patch
	{
		public static void Postfix() => Simon.Instance.Tick();
	}

	// Play on 3x speed
	[HarmonyPatch(typeof(TickManager), nameof(TickManager.CurTimeSpeed), MethodType.Setter)]
	public class TickManager_CurTimeSpeed_Patch
	{
		public static void Postfix(ref TimeSpeed ___curTimeSpeed)
		{
			if (Simon.Instance.currentTask == 1)
				___curTimeSpeed = TimeSpeed.Superfast;
		}
	}
	[HarmonyPatch(typeof(TickManager), nameof(TickManager.TogglePaused))]
	public class TickManager_TogglePaused_Patch
	{
		public static bool Prefix() => Simon.Instance.currentTask != 1;
	}

	// --------------------------------------------------------------------------------------------------------------

	// Your colonists are hidden
	[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.RenderPawnInternal))]
	public class PawnRenderer_RenderPawnInternal_Patch
	{
		public static bool Prefix(Pawn ___pawn) => ___pawn.IsColonist == false || Simon.Instance.currentTask != 2;
	}
	[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.RenderPawnAt))]
	public class PawnRenderer_RenderPawnAt_Patch
	{
		public static bool Prefix(Pawn ___pawn) => ___pawn.IsColonist == false || Simon.Instance.currentTask != 2;
	}
	[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.RenderShadowOnlyAt))]
	public class PawnRenderer_RenderShadowOnlyAt_Patch
	{
		public static bool Prefix(Pawn ___pawn) => ___pawn.IsColonist == false || Simon.Instance.currentTask != 2;
	}
	[HarmonyPatch(typeof(GenMapUI), nameof(GenMapUI.DrawThingLabel))]
	[HarmonyPatch(new Type[] { typeof(Thing), typeof(string), typeof(Color) })]
	static class GenMapUI_DrawThingLabel_Patch
	{
		public static bool Prefix(Thing thing)
		{
			if (Simon.Instance.currentTask == 14)
				return Simon.TunnelDrawer.IsVisible(thing.Position) == false;
			if (Simon.Instance.currentTask == 20)
				return Simon.ReverseTunnelDrawer.IsVisible(thing.Position) == false;
			return true;
		}
	}
	[HarmonyPatch(typeof(GenMapUI), nameof(GenMapUI.DrawPawnLabel))]
	[HarmonyPatch(new Type[] { typeof(Pawn), typeof(Vector2), typeof(float), typeof(float), typeof(Dictionary<string, string>), typeof(GameFont), typeof(bool), typeof(bool) })]
	static class GenMapUI_DrawPawnLabel_Patch
	{
		public static bool Prefix(Pawn pawn)
		{
			if (Simon.Instance.currentTask == 14)
				return Simon.TunnelDrawer.IsVisible(pawn.Position) == false;
			if (Simon.Instance.currentTask == 20)
				return Simon.ReverseTunnelDrawer.IsVisible(pawn.Position) == false;
			return pawn.IsColonist == false || Simon.Instance.currentTask != 2;
		}
	}
	[HarmonyPatch(typeof(ColonistBar), nameof(ColonistBar.ColonistBarOnGUI))]
	public class ColonistBar_ColonistBarOnGUI_Patch
	{
		public static bool Prefix() => Simon.Instance.currentTask != 2;
	}
	[HarmonyPatch(typeof(PawnRenderTree), nameof(PawnRenderTree.Draw))]
	public class PawnRenderTree_Draw_Patch
	{
		public static bool Prefix(PawnDrawParms parms) => parms.pawn.IsColonist == false || Simon.Instance.currentTask != 2;
	}
	[HarmonyPatch]
	public class ThingSelectionUtility_SelectColonist_Patch
	{
		public static IEnumerable<MethodBase> TargetMethods()
		{
			yield return SymbolExtensions.GetMethodInfo(() => ThingSelectionUtility.SelectNextColonist());
			yield return SymbolExtensions.GetMethodInfo(() => ThingSelectionUtility.SelectPreviousColonist());
		}
		public static bool Prefix() => Simon.Instance.currentTask != 2;
	}

	// --------------------------------------------------------------------------------------------------------------

	// Who needs to move the map
	// Play fully zoomed in/out
	[HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.Update))]
	public class CameraDriver_Update_Patch
	{
		public static Vector3 forcedRootPos;
		public static float forcedDesiredSize;

		public static void Postfix(ref Vector3 ___rootPos, ref float ___rootSize)
		{
			var task = Simon.Instance.currentTask;
			if (task == 4)
				___rootPos = forcedRootPos;
			if (task == 7 || task == 8)
				___rootSize = forcedDesiredSize;
		}

		public static void Remember(float size)
		{
			forcedRootPos = Find.CameraDriver.rootPos;
			forcedDesiredSize = size;
		}
	}
	[HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.ApplyPositionToGameObject))]
	public class CameraDriver_ApplyPositionToGameObject_Patch
	{
		public static void Prefix(ref Vector3 ___rootPos, ref float ___rootSize)
		{
			var task = Simon.Instance.currentTask;
			if (task == 4)
				___rootPos = CameraDriver_Update_Patch.forcedRootPos;
			if (task == 7 || task == 8)
				___rootSize = CameraDriver_Update_Patch.forcedDesiredSize;
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// Selecting stuff on the map is stupid
	[HarmonyPatch(typeof(Selector), nameof(Selector.SelectInternal))]
	public class Selector_SelectInternal_Patch
	{
		public static bool Prefix(object obj) => (obj is Pawn pawn && pawn.IsColonist) || Simon.Instance.currentTask != 5;
	}

	// --------------------------------------------------------------------------------------------------------------

	// Commands to colonists are mixed up
	[HarmonyPatch(typeof(FloatMenuMakerMap), nameof(FloatMenuMakerMap.GetOptions))]
	public class FloatMenuMakerMap_GetOptions_Patch
	{
		public static int offset;

		public static void Prefix(ref List<Pawn> selectedPawns)
		{
			if (Simon.Instance.currentTask != 6)
				return;

			var tempList = selectedPawns.ToList();
			for (var i = 0; i < tempList.Count; i++)
			{
				var pawn = tempList[i];
				var otherPawns = pawn.Map.mapPawns.FreeColonists.Except(pawn).ToArray();
				var idx = (pawn.thingIDNumber + offset) % otherPawns.Length;
				tempList[i] = otherPawns[idx];
			}
			selectedPawns = tempList;
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// Colonists will never walk more than 10 cells
	[HarmonyPatch(typeof(Pawn_PathFollower), nameof(Pawn_PathFollower.TryEnterNextPathCell))]
	public class Pawn_PathFollower_TryEnterNextPathCell_Patch
	{
		public static void Prefix(Pawn_PathFollower __instance)
		{
			if (Simon.Instance.currentTask != 9)
				return;
			if (__instance.pawn.IsColonist == false)
				return;
			var path = __instance.curPath;
			if (path?.nodes == null)
				return;

			if (path.nodes.Count - path.curNodeIndex > 10)
				LongEventHandler.ExecuteWhenFinished(() =>
				{
					var pawn = __instance.pawn;
					if (pawn.drafter.Drafted)
						pawn.drafter.Drafted = false;
					pawn.jobs.ClearQueuedJobs(true);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				});
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// Up and Down, Left and Right
	[HarmonyPatch(typeof(Prefs), nameof(Prefs.MapDragSensitivity), MethodType.Getter)]
	public class Prefs_MapDragSensitivity_Patch
	{
		public static void Postfix(ref float __result)
		{
			if (Simon.Instance.currentTask == 10)
				__result = -__result;
		}
	}
	[HarmonyPatch(typeof(CameraDriver), nameof(CameraDriver.CameraDriverOnGUI))]
	public class CameraDriver_CameraDriverOnGUI_Patch
	{
		static float DollyRateKeys(CameraMapConfig me)
		{
			return Simon.Instance.currentTask == 10 ? -me.dollyRateKeys : me.dollyRateKeys;
		}

		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var dollyRateKeys = AccessTools.Field(typeof(CameraMapConfig), nameof(CameraMapConfig.dollyRateKeys));
			var matcher = new CodeMatcher(instructions);
			for (var i = 1; i <= 4; i++)
				matcher = matcher.MatchStartForward(new CodeMatch(OpCodes.Ldfld, dollyRateKeys))
				.SetInstruction(CodeInstruction.Call(() => DollyRateKeys(default)));
			return matcher.InstructionEnumeration();
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// Colonists need to be selected to move around
	// They will only do stuff when not in sight
	[HarmonyPatch(typeof(Pawn), nameof(Pawn.Tick))]
	public class Pawn_Tick_Patch
	{
		public static bool Prefix(Pawn __instance)
		{
			if (__instance.IsColonist)
			{
				if (Simon.Instance.currentTask == 11)
					return Find.Selector.IsSelected(__instance);
				if (Simon.Instance.currentTask == 13)
					return Find.CameraDriver.CurrentViewRect.Contains(__instance.PositionHeld) == false;
			}
			return true;
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// All colonists are now called 'Simon'
	[HarmonyPatch(typeof(GenMapUI), nameof(GenMapUI.GetPawnLabel))]
	static class GenMapUI_GetPawnLabel_Patch
	{
		public static bool Prefix(Pawn pawn, ref string __result)
		{
			if (Simon.Instance.currentTask != 12)
				return true;
			if (pawn.IsColonist == false)
				return true;
			__result = "Simon";
			return false;
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// TunnelVision
	[HarmonyPatch(typeof(MapInterface))]
	[HarmonyPatch(nameof(MapInterface.MapInterfaceUpdate))]
	static class MapInterface_MapInterfaceUpdate_Patch
	{
		static void Postfix()
		{
			if (Simon.Instance.currentTask == 14)
				Simon.TunnelVision();
			if (Simon.Instance.currentTask == 20)
				Simon.ReverseTunnelVision();
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// Colonists will have a mental break if they are getting too close to each other
	[HarmonyPatch(typeof(Thing))]
	[HarmonyPatch(nameof(Thing.Position), MethodType.Setter)]
	static class Pawn_Position_Patch
	{
		public static readonly HashSet<Pawn> closenessReady = [];
		public static Pawn midas = null;
		static readonly TerrainDef goldTile = DefDatabase<TerrainDef>.GetNamed("GoldTile");

		public static void Postfix(Thing __instance, IntVec3 value)
		{
			if (Simon.Instance.currentTask == 16)
			{
				if (__instance is not Pawn pawn)
					return;
				if (pawn.IsColonist == false || pawn.Downed)
					return;
				if (pawn.mindState.mentalBreaker.CanHaveMentalBreak() == false)
					return;

				var otherColonists = pawn.Map.mapPawns.FreeColonists
					.Where(p => p != pawn && p.Downed == false && MentalStateUtility.IsHavingMentalBreak(p) == false);
				if (otherColonists.Any(p => p.Position.DistanceToSquared(value) <= 9))
				{
					if (closenessReady.Contains(pawn))
					{
						Find.CameraDriver.desiredSize = Find.CameraDriver.config.sizeRange.min;
						Find.CameraDriver.rootSize = Find.CameraDriver.desiredSize;
						LongEventHandler.ExecuteWhenFinished(() =>
						{
							CameraJumper.TryJumpAndSelect(pawn, CameraJumper.MovementMode.Cut);
							pawn.mindState.mentalBreaker.TryGetRandomMentalBreak(MentalBreakIntensity.Minor, out var mentalBreakDef);
							pawn.mindState.mentalBreaker.TryDoMentalBreak("Reason: Simon Said So", mentalBreakDef);
						});
					}
				}
				else
					closenessReady.Add(pawn);
			}
			if (Simon.Instance.currentTask == 18 && __instance == midas)
			{
				var map = __instance.Map;
				if (map == null)
					return;
				for (var dx = -1; dx <= 1; dx++)
					for (var dy = -1; dy <= 1; dy++)
					{
						var pos = value + new IntVec3(dx, 0, dy);
						if (pos.InBounds(map))
						{
							if (map.terrainGrid.TerrainAt(pos) != goldTile)
							{
								map.terrainGrid.RemoveTopLayer(pos, false);
								map.terrainGrid.SetTerrain(pos, goldTile);
							}
							map.thingGrid.ThingsAt(pos).DoIf(t => t is not Pawn && t.def != ThingDefOf.Gold, t =>
							{
								t.Destroy();
								var thing = ThingMaker.MakeThing(ThingDefOf.Gold, null);
								GenPlace.TryPlaceThing(thing, pos, map, ThingPlaceMode.Direct);
							});
						}
					}
			}
		}
	}

	// --------------------------------------------------------------------------------------------------------------

	// Colonists will take weird paths
	[HarmonyPatch(typeof(PathFinderCostTuning), nameof(PathFinderCostTuning.For))]
	public class PathFinderCostTuning_For_Patch
	{
		public static void Postfix(ref PathFinderCostTuning __result, Pawn pawn)
		{
			if (Simon.Instance.currentTask != 20)
				return;
			if (pawn.IsColonist == false || pawn.Drafted == false)
				return;
			__result.costDanger = int.MinValue;
		}
	}
	//
	[HarmonyPatch(typeof(PathGridJob), nameof(PathGridJob.CostForCell))]
	public class PathGridJob_CostForCell_Patch
	{
		public static bool Prefix(ref int __result, ref PathFinderCostTuning ___tuning)
		{
			if (Simon.Instance.currentTask != 20)
				return true;
			if (___tuning.costDanger != int.MinValue)
				return true;
			__result = Rand.Range(0, 30000);
			return false;
		}
	}
}