# DelayJoinHearthstone

This is a small utility for the Blizzard game Hearthstone which patches the client to add in a random 5-60 second delay when starting a game. The intended use is for streamers trying to avoid harassment from snipers.


[![Video](http://img.youtube.com/vi/q8qg159EZp4/0.jpg)](http://www.youtube.com/watch?v=q8qg159EZp4)

To use, extract DelayJoin.zip to <Hearthstone Installation Dir>\Hearthstone_Data\Managed. Run DelayJoinPatcher.exe (using Mono on non-windows platforms). This will create a modified version of Assembly-CSharp.dll called Assembly-CSharp.Patched.dll. Backup or rename Assembly-CSharp.dll and change the name of Assembly-CSharp.Patched.dll to Assembly-CSharp.dll. All files except DelayJoin.dll may be removed at this point. To revert the change, overwrite the modified Assembly-CSharp.dll with the original version. The patch might work after an update without any changes, but may need to be reapplied.

When joining a game, the searching popup will show as normal, but the request will not actually be sent until a random amount of time between 5 and 60 seconds has passed). The delay can also be ended immediately by pressing the ` key. The cancel button works normally.
