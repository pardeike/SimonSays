using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace SimonSays
{
	[DefOf]
	public static class Defs
	{
		public static SoundDef Simon00;
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
		public static SoundDef Simon15;
		public static SoundDef Simon16;
		public static SoundDef Simon17;
		public static SoundDef Simon18;
		public static SoundDef Simon19;
		public static SoundDef Simon20;
		public static SoundDef Thankyou;
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
			public int duration = 120; // seconds
			public bool realtime = true;
			public Action startAction;
			public Action tickAction;
			public Action eventAction;
			public Action endAction;
		}

		public static Task[] tasks =
		[
			/* 00 */ new Task { title = $"Welcome to my world! You will follow my rules now!\n\nEveryone gets the same tasks the same time (roughly every {Simon.interval} minutes)", sound = Defs.Simon00, duration = 0 },
			/* 01 */ new Task { title = "You're too slow! Play on 3x speed!", sound = Defs.Simon01, startAction = Simon.Play3XSpeed },
			/* 02 */ new Task { title = "Look! Your colonists are hidden!", sound = Defs.Simon02, startAction = Simon.ColonistsHidden },
			/* 03 */ new Task { title = "Be careful! Do not right-click or a colonist dies!", sound = Defs.Simon03, eventAction = Simon.RightClickColonistDies },
			/* 04 */ new Task { title = "Freeze! Who needs to move the map!", sound = Defs.Simon04, startAction = Simon.DoNotMoveMap },
			/* 05 */ new Task { title = "Selecting stuff on the map is stupid!", sound = Defs.Simon05, startAction = Simon.NoMapSelect },
			/* 06 */ new Task { title = "Switcheroo! Commands to colonists are mixed up!", sound = Defs.Simon06, startAction = Simon.MixUpColonists },
			/* 07 */ new Task { title = "Who needs details? Play fully zoomed out!", sound = Defs.Simon07, startAction = Simon.FullyZoomedOut },
			/* 08 */ new Task { title = "Who needs an overview? Play fully zoomed in!", sound = Defs.Simon08, startAction = Simon.FullyZoomedIn },
			/* 09 */ new Task { title = "Your colonists will never walk more than 10 cells!", sound = Defs.Simon09 },
			/* 10 */ new Task { title = "Up and Down, Left and Right. Who cares!", sound = Defs.Simon10 },
			/* 11 */ new Task { title = "Colonists need to be selected to move around!", sound = Defs.Simon11 },
			/* 12 */ new Task { title = "I love my name! All colonists are now called 'Simon'!", sound = Defs.Simon12, startAction = Simon.RenameColonists },
			/* 13 */ new Task { title = "Leave your colonists alone! They will only do stuff when not in sight!", sound = Defs.Simon13 },
			/* 14 */ new Task { title = "Let's focus with some tunnel vision!", sound = Defs.Simon14, tickAction = Simon.TunnelVision },
			/* 15 */ new Task { title = "I think your mouse is drunk!", sound = Defs.Simon15, eventAction = Simon.DrunkenMouse },
			/* 16 */ new Task { title = "Your colonists break if they get too close to each other!", sound = Defs.Simon16, startAction = Simon.CloseMeansBreak },
			/* 17 */ new Task { title = "Check your walls, I think they need repairing!", sound = Defs.Simon17, startAction = Simon.WallBreaking },
			/* 18 */ new Task { title = "You've got Midas touch!", sound = Defs.Simon18, startAction = Simon.MidasTouchStart, endAction = Simon.MidasTouchEnd },
			/* 19 */ new Task { title = "Shake it up baby!", sound = Defs.Simon19, startAction = Simon.FlipMapStart, tickAction = Simon.FlipMapTick, endAction = Simon.FlipMapEnd },
			/* 20 */ new Task { title = "Pointing is rude!", sound = Defs.Simon20, startAction = Simon.ReverseTunnelStart, tickAction = Simon.ReverseTunnelVision, endAction = Simon.ReverseTunnelEnd },
		];
	}
}