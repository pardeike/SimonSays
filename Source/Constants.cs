using RimWorld;
using UnityEngine;
using Verse;

namespace SimonSays
{
	[DefOf]
	public static class Defs
	{
		public static SoundDef Simon01;
		public static SoundDef Simon02;
		public static SoundDef Simon03;
		public static SoundDef Simon04;
		public static SoundDef Simon05;
		public static SoundDef Simon06;
		public static SoundDef Simon07;
		public static SoundDef Simon08;
		public static SoundDef Simon09;
		public static SoundDef Simon10;
		public static SoundDef Simon11;
		public static SoundDef Simon12;
		public static SoundDef Simon13;
		public static SoundDef Simon14;
	}

	[StaticConstructorOnStartup]
	public static class Textures
	{
		public static Texture2D simon = ContentFinder<Texture2D>.Get("Simon", true);
	}

	public static class Resources
	{
		public class Task
		{
			public string title;
			public SoundDef sound;
		}

		public static Task[] tasks = new Task[]
		{
			new Task { title = "Welcome to my world! You will follow my rules now!", sound = Defs.Simon01 },
			new Task { title = "You're too slow! Play on 3x speed!", sound = Defs.Simon02 },
			new Task { title = "Look! Your colonists are hidden!", sound = Defs.Simon03 },
			new Task { title = "Be careful! Do not right-click or a colonist dies!", sound = Defs.Simon04 },
			new Task { title = "Freeze! Who needs to move the map!", sound = Defs.Simon05 },
			new Task { title = "Selecting stuff on the map is stupid!", sound = Defs.Simon06 },
			new Task { title = "Switcheroo! Commands to colonists are mixed up!", sound = Defs.Simon07 },
			new Task { title = "Who needs details? Play fully zoomed out!", sound = Defs.Simon08 },
			new Task { title = "Who needs an overview? Play fully zoomed in!", sound = Defs.Simon09 },
			new Task { title = "Your colonists will never walk more than 10 cells!", sound = Defs.Simon10 },
			new Task { title = "Up and Down, Left and Right. Who cares!", sound = Defs.Simon11 },
			new Task { title = "Colonists need to be selected to move around!", sound = Defs.Simon12 },
			new Task { title = "I love my name! All colonists are now called 'Simon'!", sound = Defs.Simon13 },
			new Task { title = "Leave your colonists alone! They will only do stuff when not in sight!", sound = Defs.Simon14 },
		};
	}
}