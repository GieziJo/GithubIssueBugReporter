# Github Issues Bug Reporter
Bug reporter for Unity which automagically creates a new issue on Github with a screenshot of the current situation in the game, as well as a description provided by the player.

## Description
This package allows to manually push **F12** in game and/or on any error, pause the game and open a bug reporting GUI:
![GUI](https://filedn.eu/lgUOHeW73WkzUY76gfcY8s5/EarlyCoffee.Games/Images/Blog/BugReporter/GUI.png)

The GUI asks for the following informations:
<ul>
<li>User: the name you want to be displayed as person having submitted the bug.</li>
<li>Github issue mention toggle: if enabled, you get a "@" mention in the issue and get a notification in Github.</li>
<li>Title: the title of your issue.</li>
<li>Description: describe the issue to the best of your ability.</li>
<li>Adding the Player log to the error message.</li>
<li>Submitting and canceling buttons.</li>
</ul>

Additionally the bug description is tagged with the application's current version for faster filtering.

After hitting submit, an issue is created on github with a screenshot of the game:
![Issue](https://filedn.eu/lgUOHeW73WkzUY76gfcY8s5/EarlyCoffee.Games/Images/Blog/BugReporter/Issue.png)

An article describing the implementation is to be found here: [https://earlycoffee.games/blog/2021_06_12_bugreportingtool/](https://earlycoffee.games/blog/2021_06_12_bugreportingtool/)

## Additional features
### Automatic popup on Error
The bug reporter will also automatically popup on any error. If this is not wanted, there are two ways to disable it:
1. within Unity, add the define `GIEZI_TOOLS_DISABLE_ON_ERROR_POPUP` to the defines list. This option can be set in the option dialogue (see below).
2. wherever in code, set the player pref variable `Giezi.Tools.GithubBugReporter.PopupOnError` to `0`:
```csharp
PlayerPrefs.SetInt("Giezi.Tools.GithubBugReporter.PopupOnError", 0);
```
 you can use this to enable and disable this feature in a menu.

### Background error reporting
Setting the player pref variable `Giezi.Tools.GithubBugReporter.ErrorPopupInBackground` to `1` will prevent the GUI to be displayed, and will simply generate an error report without addition info (username, description, etc.). In place a small message pops up `Bug Reported`.

This value is to be set in code, and should be done with the user's consent. Best practice is a dialogue at the begining of the game asking for the User's preference, and an option to change in menu.

## Installation
### Using Unity's package manager
Add the line
```
"ch.giezi.tools.githubissuebugreporter": "https://github.com/GieziJo/GithubIssueBugReporter.git#master"
```
to the file `Packages/manifest.json` under `dependencies`, or in the `Package Manager` add the link [`https://github.com/GieziJo/GithubIssueBugReporter.git#master`](https://github.com/GieziJo/GithubIssueBugReporter.git#master) under `+ -> "Add package from git URL...`.

## Setup
### Setup repo data
You need to provide certain informations to where the issues and screenshots should be sent.

Go to `Tools > GieziTools > Github Bug Reporter` and fill in the different fields:
![Json](https://filedn.eu/lgUOHeW73WkzUY76gfcY8s5/EarlyCoffee.Games/Images/Blog/BugReporter/Json.png)

Once you click the `Generate Json` button, the tool will generate the two Json files `Assets/Resources/GieziTools.GithubIssueBugReporter/GithubInfos.json` and `Assets/Resources/GieziTools.GithubIssueBugReporter/GithubAccessTokens.json` and along with a `.gitignore` file containing the following two lines:

```git
GithubAccessTokens.json
GithubAccessTokens.json.meta
```

This way the access tokens in the Json file won't be committed to Github. Remove the `.gitignore` file if you think this should be the case.
The tokens are encrypted before getting stored in the Json file, but I'm open to suggestions how to better handle this.

<span style="color:red">DISCLAIMER: I'm an absolute noob in terms of web security so let me know if things could be done in a better way!</span>

### Input System
Make sure the supported devices in the input system settings is either empty or contains `Keyboard` and `Mouse`, otherwise you won't be able to interact with the bug reporter.

## Known issues and tweaks to be made
<details>
<summary>List of known issues</summary>

### [Improve way tokens are stored and accessed](https://github.com/GieziJo/GithubIssueBugReporter/issues/1)

</details>
