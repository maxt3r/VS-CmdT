Cmd + T for Visual Studio
=======

NOTE: This functionality is now built-in in all the recent VS versions. Absolutely no need for this extension anymore.
=======

The Command-T extension provides an extremely fast, intuitive mechanism for
opening files with a minimal number of keystrokes. It's named
"Command-T" because it replicates the "Go to File" window bound to
Command-T in TextMate.

![CmdT Visual Studio](https://dl.dropboxusercontent.com/u/347209/cmdt.png)

## Installation
Go to `Tools` - `Extensions and upgrades` and search for `Cmd + T`

## Usage

You can launch it by clicking `Go to file` in the `Tools` menu, but the prefered way to do that is by pressing a shortcut. The default shortcut is `Alt + T`, since `Ctrl + t` is used by Visual Studio for some weird command no one have ever used.

You can rebind it by going to `Tools - Options - Environment - Keyboard`. The name of the command to bind is: `Tools.GoToFile`

Once you see the extension window you can start typing. It uses a fuzzy search algorithm, so if you type `mdlusrcs`, it should show `Models\User.cs` file. Press `Enter` to open it and start editing right away.

## Contributions

There are lots of thing that could be improved. It's been tested only on VS2012. The search algorithm is also far from perfect and it probably has performance issues on large projects. Help me fix them, Feel free to submit a pull request or create an issue.

## Authors and license

**Max Al Farakh from [JitBit](http://www.jitbit.com/)**

[@maxt3r](https://twitter.com/maxt3r)

Licensed under [the MIT License](http://opensource.org/licenses/MIT)
