# Github Issues Bug Reporter
## Description
This package allows to push **F12** in game, pause the game and open a bug reporting GUI:
![GUI](https://filedn.eu/lgUOHeW73WkzUY76gfcY8s5/EarlyCoffee.Games/Images/Blog/BugReporter/GUI.png)

The GUI asks for the following informations:
<ul>
<li>User: the name you want to be displayed as person having submitted the bug.</li>
<li>Github issue mention toggle: if enabled, you get a "@" mention in the issue and get a notification in Github.</li>
<li>Title: the title of your issue.</li>
<li>Description: describe the issue to the best of your ability.</li>
<li>Submitting and canceling buttons.</li>
</ul>

After hitting submit, an issue is created on github with a screenshot of the game:
![Issue](https://filedn.eu/lgUOHeW73WkzUY76gfcY8s5/EarlyCoffee.Games/Images/Blog/BugReporter/Issue.png)

An article describing the implementation is to be found here: [https://earlycoffee.games/blog/2021_06_12_bugreportingtool/](https://earlycoffee.games/blog/2021_06_12_bugreportingtool/)

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


</details>
