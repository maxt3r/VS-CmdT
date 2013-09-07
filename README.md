Cmd + T for Visual Studio
=======

The Command-T extension provides an extremely fast, intuitive mechanism for
opening files with a minimal number of keystrokes. It's named
"Command-T" because it is inspired by the "Go to File" window bound to
Command-T in TextMate.
		
![Screenshot](https://www.dropbox.com/s/oq0gyrc65uo3t95/Screenshot%202013-09-07%2012.57.49.png)

## Installation
Go to `Tools` - `Extensions and upgrades` and search for `Cmd + T`

## Usage

You can launch it by clicking `Go to file` in the `Tools` menu, but the preffered way to do that is by pressing a shortcut. The default shortcut is `Alt + T`, since `Ctrl + t` is used by Visual Studio for some weird command no one have ever used.

You can rebind it by going to `Tools - Options - Environment - Keyboard`.

Once you see the extension window you can start typing. It uses a fuzzy search algorithm, so if you type `mdlusrcs`, it should show `Models\User.cs` file. Press `Enter` to open it and start editing right away.

## Contributions

There are lots of thing that could be improved. I've made it in two hours and it's been tested only on VS2012. The search algorithm is also far from perfect. Feel free to submit a pull request or create an issue.

## Authors and license

**Max Al Farakh from [JitBit](http://www.jitbit.com/)**

[https://twitter.com/maxt3r](https://twitter.com/maxt3r)
