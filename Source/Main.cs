﻿using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace SimonSays
{
	public class SimonSays_Main : Mod
	{
		public SimonSays_Main(ModContentPack content) : base(content)
		{
			LongEventHandler.ExecuteWhenFinished(() => new Harmony("net.pardeike.simonsays").PatchAll());
		}
	}

	public class SimonSays : WorldComponent
	{
		public SimonSays(World world) : base(world) { }
		public override void WorldComponentTick() => Simon.Instance.Tick();
		public override void ExposeData() => Simon.Instance.ExposeData();
	}

	[HarmonyPatch(typeof(Root_Entry), nameof(Root_Entry.Update))]
	public class Root_Entry_Update_Patch
	{
		static void Postfix() => Simon.Instance.Tick();
	}
}