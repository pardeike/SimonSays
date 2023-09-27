using HarmonyLib;
using Verse;

namespace SimonSays
{
	[HarmonyPatch(typeof(Root_Entry), nameof(Root_Entry.Update))]
	public class Root_Entry_Update_Patch
	{
		static void Postfix() => Simon.Instance.Tick();
	}
}
