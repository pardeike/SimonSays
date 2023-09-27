using RimWorld;
using System;
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
			public int duration = 60;
			public bool realtime = true;
			public Action startAction;
			public Action endAction;
		}

		public static Task[] tasks = new Task[]
		{
			new Task { title = "Welcome to my world! You will follow my rules now!", sound = Defs.Simon01, startAction = Simon.Task01 },
			new Task { title = "You're too slow! Play on 3x speed!", sound = Defs.Simon02, startAction = Simon.Task02 },
			new Task { title = "Look! Your colonists are hidden!", sound = Defs.Simon03, startAction = Simon.Task03 },
			new Task { title = "Be careful! Do not right-click or a colonist dies!", sound = Defs.Simon04, startAction = Simon.Task04 },
			new Task { title = "Freeze! Who needs to move the map!", sound = Defs.Simon05, startAction = Simon.Task05 },
			new Task { title = "Selecting stuff on the map is stupid!", sound = Defs.Simon06, startAction = Simon.Task06 },
			new Task { title = "Switcheroo! Commands to colonists are mixed up!", sound = Defs.Simon07, startAction = Simon.Task07 },
			new Task { title = "Who needs details? Play fully zoomed out!", sound = Defs.Simon08, startAction = Simon.Task08 },
			new Task { title = "Who needs an overview? Play fully zoomed in!", sound = Defs.Simon09, startAction = Simon.Task09 },
			new Task { title = "Your colonists will never walk more than 10 cells!", sound = Defs.Simon10, startAction = Simon.Task10 },
			new Task { title = "Up and Down, Left and Right. Who cares!", sound = Defs.Simon11, startAction = Simon.Task11 },
			new Task { title = "Colonists need to be selected to move around!", sound = Defs.Simon12, startAction = Simon.Task12 },
			new Task { title = "I love my name! All colonists are now called 'Simon'!", sound = Defs.Simon13, startAction = Simon.Task13 },
			new Task { title = "Leave your colonists alone! They will only do stuff when not in sight!", sound = Defs.Simon14, startAction = Simon.Task14 },
		};
	}
}