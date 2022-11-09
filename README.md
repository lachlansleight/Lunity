# Lunity

Lunity is a collection of a bunch of things that I found myself doing again and again in Unity and just wanted to have sorted out when I start a new project. Some of these things are so minimal but SO useful that it kind of frustrates me that they're not just part of Unity by default (e.g. MinMaxRange, UnityEvents, many of my extension methods).

It's a bit of a mixed bag - some parts I use constantly in every single project, some parts I've only ever used once but thought I might need to use again. Most of it is my own work, but some parts are open-source scripts and assets (with the appropriate licenses + attribution) that I found myself copying from project to project.

## Scripts

### Handy Scripts

This is mostly what I use Lunity for. There's a ton of good shit in here.

-  AudioVisualization: A Windows-only WASAPI-based audio visualization script that lets you access various pieces of useful information about the currently-playing audio on the machine (volume, frequency bands, etc)
-  ArrayUtils: JavaScript-like functional array utilities. I did my best to mimic TypeScript syntax as much as possible. It's pretty nice.
-  AudioJukebox: For when you want to have an object play a sound effect from a list of sounds (e.g. impact sounds, animal calls, etc) without repeating itself in an efficient, simple way
-  AutoTransform: An easy way to apply a position/rotation/scale offset every frame - mostly handy for making things rotate in place
-  ComponentToggle: Lets you enable/disable a target component with a key press
-  CopyTransform: A very useful script - put it on an object, provide it a target, tell it what transform elements to copy and it does its thing. Works in edit mode.
-  Ease: A selection of useful easing functions available as static methods. Very handy!
-  FpsAverager: Pretty much what it says on the tin. Useful for recording performance metrics during testing.
-  ListNonAlloc: A non-allocating list - it uses more memory since it allocates everything at the start, but doesn't do any of the nasty runtime memory allocation stuff that triggers GC.
-  LProp: A serializable field with an OnChanged property, rather than `public float Something`, you do `public LProp<float> Something`, and then you can get `Something.Value` to get or set the value, but also `Something.OnChange += newVal => Debug.Log("The value has changed to " + newVal);` which is pretty nice.
-  LunityMath: Various math things that I kept doing, so I put them into a class and made them static. The last time I edited this readme, it has `Get2DIntersection`, `Get2DIntersectionBounded` (to calculate the intersection point of two lines, either infinite or of fixed length), `GetCrossfade` (an equal-power crossfade for audio), `VolumePower` (turns 0-1 into a dB fade value), `Map` (like lerp + inverse lerp) and `CubicBezier` which just evaluates a bezier spline from its four handles and the input parameter.
-  LWeb: Useful wrappers for HTTP requests - only has get requests atm
-  MinMaxRange & MinMaxRangeInt: I use these *all* the time - basically a vector2, but with `min` and `max` properties, as well as a bunch of useful functions and properties like `GetRandom` which returns a random value in the range, `Lerp` which lerps between the values, etc. Super useful.
-  MouseEventActions: Add it to an object with a collider to get various useful mouse events like hover states, click, etc.
-  ObjectPool: Just a normal object pool class - used whenever you would otherwise be instantiating many similar objects
-  ParticleSystemRateCopier: For when you want one particle system's emission rate to depend on another (e.g. fire and smoke) - you can use a script to set the 'source' system's emission rate and use this script on the other systems.
-  RandomQueue: Provide it a list of things and it'll shuffle them and put them into a queue. Then you just get the next item in the queue (it'll recreate the queue once it's empty)
-  SceneLoader: A wrapper around the SceneManager with some useful functions...only barely useful, but ReloadCurrentScene comes in handy
-  SeamlessAudioLooper: This one is great - takes in an audio track and crossfades it with itself to make it loop seamlessly, even if it doens't actually loop.
-  ShaderProperty: Script interface to manage setting arbitrary properties on a shader. Pairs with a custom property drawer.
-  SimpleSingleton: Just another singleton class - there are many like it, but this one is mine. Kind of minimal and probably slightly unsafe, but fine for prototyping and quick iteration.
-  Singleton: A fancier, more reliable Singleton class.
-  UiHeadFollower: A handy script to make a world space UI (or anything else, I suppose) smoothly position itself in front of the user's head. I mainly use this for VR when I need to show something important.
-  UnityEvents: Various unity events with type payloads. Lets you do things like `public UnityEvent<float> OnFloatValueChanged` and get a useful inspector item.
-  VectorDoubles: Replicants of all the default Vector types in Unity, but using double precision rather than float precision.

## Extension Scripts

Many, many very handy extension methods.

-  BoxColliderExtensions: `ContainsPoint` - whether the provided Vector3 is inside the box
-  ColorExtensions: `SetAlpha` - creates a new color from the source color with the specified alpha, `CreateGradient` - creates a gradient with two, three or four stops
-  DebugExtensions: Various log functions that let you provide optional colors and 'contexts' which appear as bold prefixes followed by a colon, `DrawBounds` which draws a box made out of lines, `DrawWorldLine` which is like DrawLine but it appears in the build (useful for VR), `LogObject` for logging arbitrary data formatted as a JSON string
-  LayerMaskExtensions: Check whether the provided mask contains the provided layer, and a way to turn an int into a layer mask and vice versa (which I remember needing for...some reason)
-  MiscExtensions: Lets you do this: `gameObject.AddComponent<any monobehaviour type>(someOtherObject.GetComponent<that same monobehaviour type>);` to add a copy of a source object's component to any other game object.
-  ParticleSystemExtensions: Has a simple SetRate method because I despise the new module-based script interface and I have to set particle system rates all the time
-  QuaternionExtensions: `GetFlattened` returns the same rotation but with x and z rotations set to zero. The other functions are self-explanatory and a little more niche.
-  RandomExtensions: `LRandom.RangeExcept` returns a random int within a range but you can provide a value or set of values that it shouldn't be. `LRandom.RandomSign` returns -1 or 1, `FlatRotation` returns a random rotation but only about the y axis (handy for placing objects in a scene), `LRandom.Boolean()` returns a random bool, and `LRandom.InsideCylinder` returns a random point inside a cylinder
-  RectExtensions: `GetPerimeterPoint` gets the point on the perimeter in the direction of the provided vector
-  TransformExtensions: `SetPosition[X/Y/Z]` does what you'd expect, as does `IncrementPosition[X/Y/Z]`. `FlatLookAt` is kind of like LookAt, but it never rotates about x or z. `FlatLookAtCheap` does the same thing, but less accurately and cheaper. `SetParentNeutral` is like SetParent, except it doesn't completely destroy relative rotation and scale when you do it to RectTransforms.
-  VectorExtensions: Heaps of handy functions in here - like dozens and dozens. Read through the class to see what's there. I use these constantly.

## Editor Scripts

Some things that make creating more useful inspectors easier, without having to go and make a whole custom inspector class, as well as a few other handy things that can happen at edit-time

-  ButtonPropertyDrawer allows you to add a `[Button]` attribute to any public method that returns void and takes no input, and it'll add a handy button to that script's inspector
-  MatrixPropertyDrawer adds a more convenient matrix visualization to the inspector that doesn't require you to expand a huge list of floats. It's a little janky, but I've still found it to be much more useful than the default Unity version
-  ReadOnlyPropertyDrawer allows you to add a `[ReadOnly]` attribute to anything that would appear in the inspector and have it appear as a nice greyed-out 'read only' variable. Note that this is janky as hell for multi-line things and anything that uses a custom property drawer
-  BakeObjectsToMesh: provides a menu item that, when clicked, takes all selected GameObjects and outputs a new GameObject with a created mesh for all the selected objects. I used this when I had a zillion tiny, simple objects that weren't going to move, and I wanted to not have the huge performance overhead of many thousands of objects. Kind of like a geometry bake.
-  LunityEditorUtils: contains one function that lets you provide a path in the asset folder and it'll create all the necessary folders to make that path valid. Handy when you need to generate assets at some path but you don't want to go through the whole process of making sure you create each folder recursively to avoid getting an IOException.
-  LUI: has a few useful things in it - a bunch of Icons for use as GUIContent in your custom inspector (they're built into Unity, but not easy to find or use), utilities to get left and right-aligned rects within a parent rect, and the good ol' ReorderableList that I'm sure everyone's copy-pasted from the internet at some point.
-  LineRendererCircleGenerator: Maybe this is really niche, but this lets you right-click on a LineRenderer component and generate the points necessary to create perfect circles.

## Assets

I try to keep these super minimal because I really want Lunity to stay really small. Textures are tiny, meshes are very low-poly.

-  Models: A bevelled cube, because it's nicer for prototyping than the default cube. A bunch of useful imposter meshes. A sphere with inverted normals. A human figure for scale. A double-sided quad.
-  Shaders: A nice gradient skybox (two or three color), a simple diffuse color for mobile, a simple outline shader, a simple vertex-color shader and the constantly useful UnlitTransparentTexture shaders (additive and blended) which are basically like the UnlitTexture shaders, but with tint color (I use these constantly for things like fading transparent objects out)
-  Textures: Just some useful soft particle textures with various falloff percentage values
-  Sprites (within the UI folder): A handful of useful sprites that I use quite a lot. Borders, arrows, tabs, dots, shapes, etc.

## Modules

Self-contained functional elements

-  BasicConsole: a very minimal way to get access to the output of the Unity console (or display your own custom messages if you don't want to hook into the Unity console) in a build. Also contains the ability to enter basic commands and contains a very simple script interface to parse and respond to said commands. Not as powerful, flexible, performant or reliable as a full asset store pack, but useful for testing and debugging and is literally two scripts, a monospaced font and a single prefab you add to a canvas.
-  BasicFader: a very simple way to fade in and out that works in VR. Contains a single script, a single prefab that you add to your scene, and a single material. Contains a static script interface so that you can simply call things like `BasicFader.SetAlpha(i);` inside a coroutine loop (for example) to fade in or out.
-  PrettyCam: intended for use in VR, provides a way to render a nicer view out to the 2D monitor that is smoothed, has a wider field of view and disables roll. A single script and a single prefab that's added to the scene. Obviously a big performance hit, but very useful for exhibitions and things where people will be spectating the scene view. Also useful for recording gameplay footage and demo videos.
-  WorldSpaceInspector: A single script + prefab that provides an extremely quick way to gain read-only access to all the fields and properties on any script in your scene on a world-space canvas. Very handy for debugging issues that only appear in-build.
-  VrController: a simple VR controller that you have full control over, if you don't want to have to import a huge, bloated package like the Oculus or SteamVR utilities. Has a small animating element that you can use to indicate the primary input event.

## UI Stuff

Unity's UI is pretty flexible, but it can be a little cumbersome, so I have a whole bunch of little prefabs and things to make working with it faster and simpler

### QuickUI

This is probably the most useful thing in Lunity, or at least the thing that saves me the most time. It lets you create a full UI (using Layout Groups) that can control most aspects of any component on any object in the inspector. It's not intended for use in a shipped game or anything, but when you just want to be able to modify a set of properties on some object, or create a dev control panel or something, it's amazing. 

You create a list of target properties, and for each indicate the target GameObject, the component on that object, and the field/property/method that you'd like a control for.

It supports the following data types: `int`, `float`, `bool`, `string` and `Vector3`, as well as any enum types as a dropdown. It also supports the following attributes in the same way that the Unity inspector does (more or less): `Range`, `Slider`, `Space` and `Header`. It's rad.

To use it, set up a container RectTransform somewhere in a canvas, add a suitable LayoutGroup component to it, and then add the QuickUi component. You can then add controls to its list. The inspector is a little cumbersome, but it works!

### UI Components

Various smaller UI things that come in handy

-  UiPanelCrossfader: I use this all the time - place it on a RectTransform that has multiple UI panels as children, then click 'Refresh Panels'. Using the inspector (which functions in edit mode) you can control crossfading between any two panels. Pairs well with Timeline.
-  RootCanvas: Place it at the root of your main canvas. Allows you to call `RootCanvas.main` to get a reference to that canvas. Unlike `Camera.main`, this will only search the scene once and caches the result statically between scenes.
-  ColorPicker: A nice color picker component - add the ColorPicker component to a UI button and provide it a RectTransform that the actual color picker should appear within when the button is clicked. The tint color of the button will be changed to match the output of the color picker
-  LoadingBar: A simple little loading bar with a convenient script interface
-  LoadingSpinner: A simple loading spinner with a controllable spin speed and ring count - looks decent for prototypes and demos
-  SelectableList: I can't remember why I built this, but it allows you to add a list of arbitrary data that can be selected from in a grid by calling `AddItem` on the script that's attached to the prefab.
-  YesNoBox: Kind of like a read-only toggle box, except with separate sprites for when it's enabled and disabled.
-  Confirmbox: Contains a static method `ConfirmWithCallback` - kind of like the web `alert` function - you pass in a rect transform, and it creates a popup on top of everything within that window with some text and two buttons (by default Confirm and Cancel), with callbacks for each button
-  UiUtils: Some static methods that come in handy - the functions in this script should be self explanatory
-  VersionText: A simple little component you put on a Unity text object and it changes the text value to match the application's version number
-  FpsText: Similar to VersionText, except it displays the current FPS. Why do people use whole-ass Unity asset store packages for this?
-  CopyTargetText: Place it on a text and provide it a source text - it'll copy it. I use this for custom shadows and that sort of thing.