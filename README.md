# Technical Documentation

## Project Setup 

To set up, simply download the git repository from the Releases sidebar on GitHub.

The git repository will include a .NET solution which you can open in any modern Visual Studio version, or your IDE of choice. 

The git repository will also include a folder containing the Godot Game Engine. 

Please note you do not need to know anything about how the Godot Game Engine works to get this up and running, but you will need to know how to open a project and how to run a project.

## Running the Godot Project

Included with the Git repository, you will have a `Godot.zip` file as well. This folder contains the entire Godot game engine that you’ll need to run the project. Extract this to a local folder on your machine. 

To open a Godot project, either double click the `project.godot` in the repository and, if no default app is set up, navigate to the Godot.exe executable and set it up as the default app for opening the `.godot` extension; or open up the `Godot.exe` app, and import a new project to the project list, navigate to the `project.godot` file in the Tankathon repository, and select it. 

Once the project is open, wait for the engine UI to load, and then simply click the play button at the top right of the Godot GUI (or press F5) to run the application:

![image](https://github.com/user-attachments/assets/edc87964-c3fc-4562-bbfc-60c41d42a462)

A new window should pop up and immediately start the project application. 

While this method doesn’t have any preset way to hit breakpoints, it does have access to the Console Debugger at the bottom of the UI where all debug print messages, and errors will be displayed:

![image](https://github.com/user-attachments/assets/ecb17ad6-f5f9-4700-b4fb-a41ded2fe9c8)

Notice on the right hand side, you have info, error, warning, and editor messages, as well as the ability to toggle visibility. 

## Navigating the .NET Project

The .NET solution may have a lot of files, and you’re free to look throughout them all as you wish, though most will be of little impact or relevance to what you will be creating.

The important folder to identify is the “MyTank” folder. This is where the whole of your work will happen. Inside this folder you will see a `MyTank.cs` Class. This is your tank.  

## Debugging the Project via Visual Studio

To set up Visual Studio Debugging, you will need to add a new executable profile to your Visual Studio instance. 

In your Launch Profile dropdown, you should see two options: `Tankathon` and `Godot Executable`. 

The profile you’ll be working with is “Godot Executable” for the entirety of this challenge. To set this up, click on the dropdown button and select `Tankathon Debug Properties`.

![image](https://github.com/user-attachments/assets/1af4f786-06cd-48ce-b8b0-e0fb1fa7b5cf)

Which will open up your Launch Profile config window. Select “Godot Executable” and in the `Executable` input box, replace the “YOUR_GODOT_EXE_PATH_HERE” text with the path to your `Godot.exe` that we’ve extracted earlier (include the actual `.exe` in the path). 

![image](https://github.com/user-attachments/assets/8d751f1e-0d8f-4a14-8571-7363653bc3e6)

Finally, set up the Working directory as the current one (if not already set up as such):

![image](https://github.com/user-attachments/assets/c9d066fc-b4e7-4d89-815e-f67dcc61d5d1)

You should now be able to click the play button, which will load the debug symbols, as well as any breakpoints, and you will see a Godot game window pop up. You can now step through and debug your C# code.

## Creating A Bot

Inside your `MyTank.cs` Class, you have access to two methods. 

1. The `Setup(...)` method is called the first time the project is loaded, and only called once. You can use this function to set up any initial values you may need for your bot brain. 

    - This method also provides an interface `setup` that you can use to set up the initial values for your bot. You have `name`, `primaryColor`, and `secondaryColor`. You are encouraged to set your team bot’s name, and color scheme here. Note: The colors are just strings that should be formatted as a valid hex code. 

>[!NOTE]
> I considered exposing these color props as Godot Color objects instead of strings so that we’re not using magic strings, but I figured it’s more trouble than it’s worth to do that. Let me know if you think that would have been better and I will consider it for the next version! 

2. The `Do(...)` method is called on every frame after the project starts. This is where your brain logic will live. This method runs 60 times per second on average (this also runs on the physics update of the game, meaning that state is deterministic. I.e. it is not susceptible to inherent randomness). 

    - Inside this method, you have access to two objects that you can use to your advantage: The `actions` object and the `scoreboard` object. 

### Actions Object

Inside the Actions object you will have access to the available commands you can use to influence your bot’s actions:

---

#### `Scan()`

Your tank has “conical vision”, it has a slanted right, left, and middle “vision” ray that look at anything in the ray’s path and tells you what the tank can see.

i.e.: `if(actions.Scan()[Side.Left].eType == EntityType.Tank)` to check if there’s a tank to the left, for example.

**Returns:**

An `Entity` object provides you with a few potentially useful pieces of information, including:

- `eType` - the type of the entity, including Obstacles, Bullets, and Tanks.
- `globalPosition` - the current X,Y global position of the scanned entity.
- `rotation` - the rotation, in degrees, of the scanned object, if your bot’s rotation is the same as the scanned entity, this would mean that you’re moving in the same direction. 
- `distanceTo` - how far away the center of your tank is from the center of the scanned entity.

>[!TIP]
>
> Out of the box, the tanks will have green, red, and blue lines emitted from their “front”. This is a debug representation of the raycasts that are being used internally.
> 
> This is simply for visual help and is not functional. Also note that this is a crude representation, as the drawn lines can pass through objects, whereas the raycasts would not. 

---

#### `MoveForward()`

Moves the tank forward by the base tank speed.

**Returns:** 

A `bool` indicating whether or not the movement was successful

---

#### `Aim(Rotation)`

Rotates the tank clockwise or counter-clockwise depending on Rotation direction input

**Returns:**

A `float` representing the tank’s rotation in degrees

---

#### `Fire()`

Fires the cannon! The bullet will fire from the tank body, and travel in a straight line until it hits something, exploding on impact. After a successful shot, a reload countdown begins internally. 

**Returns:**

A `float` detailing how much longer until the cannon is ready in seconds. If the shot was successful, the return value will be the maximum cooldown timer. 

---

#### `FireCooldown()`

Allows you to check whether or not you can fire the cannon, without actually attempting to using the `Fire()` method. 

**Returns:**

A `float` representing the time in seconds that the cannon has left on its cooldown

---

#### `actions.stats`

An object representing your tank’s current stats:

- `rotation` - your tank’s current rotation this frame
- `xPos` - your tank’s current global x Position
- `yPos` - your tank’s current global y Position
- `healthCurrent` - your tank’s current health
- `score` - your tank’s current score

### Scoreboard Object

Inside your scoreboard object, you will have access to view the game’s internal state properties

#### `timeLeft`

An integer representing the match remaining time, in seconds. It might be beneficial for your tank to know how much longer is left in the match.

## Fighting Bots

The project comes with two very basic bots: `DumTank` and `EvilTank`. Both can be found in the EvilTanks directory. By default, the project is set up to use the `DumTank` bot. Though, as the name might imply, it’s not the smartest bot ever conceived. 

You can, however, opt for a slightly (read: _slightly_) harder challenge, by changing the opponent to be `EvilTank` instead. 

To do this, open the `GameManager.cs` file, and make the following change: 

From: `redTank.thisTank = new Tankathon.EvilTank.DumTank(); `
To: `redTank.thisTank = new Tankathon.EvilTank.EvilTank();`

You can also make further changes by instead instantiating your own tank for both the red and blue sides once it’s got some logic going. This will allow for better iteration of your tank logic and you can be sure to test against a smarter bot-brain.

 Happy tanking! 

## Submission

> [!WARNING]
> Submission deadline date is May 30, 2025 at 11:59PM

To submit your bot brain, simply zip up the MyTank folder and send it to Cristian. If you require additional directories (like for helper classes, or custom objects), please create them inside the MyTank folder so that it can all be zipped as one directory.

- Code will be sanitized:
   - The MyTank namespace will change. Since we can’t all be part of the same namespace, I will change your tank’s namespace, though I will make sure it continues to work. 
   - All print statements will be removed (just for cleanliness sake)
   - Other additional sanitization may happen at the reviewer’s discretion

Alongside the code for the tank, you will also be asked to submit the following:
- Your team name - assigned in your Tank’s bot-brain
- Your team color(s) - assigned in your Tank’s bot-brain 
   - (Set both the primary and secondary colors as the same color, if you only want to utilize one team color)
- Your team/tank’s logo - include an image file (jpg/jpeg/png/webp/svg) submitted with your submission zip file
- Your tank’s custom sound effects - include a sound effect (wav/ogg/mp3) of your choice for your tank’s “shoot” sound, and “death” sound (that’s the one you don’t want to hear).
   - Please feel free to be as creative as possible! I will try to make the sound levels be consistent so we don’t explode any eardrums.

## Competition Rules, Considerations, & Conduct

While this is a friendly competition for the most part, there are some rules that need to be obeyed to make this fair and fun for everyone. 

- I will try to test all of the bots ahead of time and give the teams a heads up if a bot seems broken when running on my machine. I will give an extension/allowance past the due date for fixes, if the bot was submitted on time.
- If a bot breaks the game, i.e. with an infinite loop and it cannot be fixed, it will be disqualified. 
- In the spirit of the game, while there may be ways to gain access to the greater Godot functions and features, which in turn will provide your tank greater control of the game state as a whole, we ask that you do not go out of your way to access these features, and use instead only what is provided for you. If you are unsure if you’re using something out of bounds, feel free to ask, and I can clarify that for you. Trying to circumvent the rules in this way may lead to a disqualification. 
   - Unfortunately it’s quite difficult, if not impossible, to lock down everything as I expect since both the C# paradigms, as well as Godot’s paradigms would like you to have as much access to the resources around you as possible. In other words, neither C#, nor the Godot game engine are made for the purpose we’re using it for.
- Third party packages are out of bounds. You shouldn’t need any third party libraries to accomplish a working bot-brain.
   - This also includes third party web requests too! (No you can’t write an interface to connect to an Xbox controller or hook up your bot-brain to an AI!) There’s no guarantee of internet access when the official game runs.
- If you're doing something and you're not sure if you're breaking the rules, always feel free to ask me! I’ll confirm one way or another.

Some other things to keep in mind when developing the bot brain. 
- The stage might be different to what the sample project provides
- Your bot may start in any corner of the arena in the official tournament run, not just the bottom half like in the sample project, and it should be able to adapt accordingly. 
- You are not limited to just the one `MyTank.cs` file. Feel free to break up your logic into multiple files as needed to keep things nice and tidy. 


## FAQ

#### What happens if I call an action multiple times in the same frame?
- There’s already some logic to make sure that actions such as `MoveForward()` or `Aim(...)` can only complete once per frame. This means that technically, the “once per frame” logic for certain actions is handled for you.
- Actions like `Scan()` and `FireCooldown()` will work multiple times per frame, but since nothing moves/changes during the same frame, you’ll get the same response from the functions. 
- `Fire()` will be callable multiple times per frame as well, though there’s no guarantee it will fire since there is a set 5 second cooldown timer for shooting.

#### I found a bug! What do I do? 
- If you find any bugs that need fixing, feel free to [open up an issue here](https://github.com/ccioanca/Tankathon-v2/issues). (And also please just message me on Teams - I’ll see this faster probably)
- If a bug needs fixing, I will put out a hotfix to get that fix, and upload an updated release to the GitHub Releases in the repo. I will also make sure to shoot a message to everyone on Teams so that everyone’s aware! 
- To update to the new version, just redo what you’ve done when first downloading the project, and move your tank brain over to the new project! 

#### The documentation is wrong! 
- I’m only human, if the documentation is wrong, just shoot me a message and let me know. I’ll update it as soon as I can and send out a message so everyone knows. 
