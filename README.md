# SuperControls

An input recorder for Unity3D.

## What does it do?

The SuperControl system is like a "DVR" where it records input given from Unity3D, stores it in a Timeline and can be played back at a given time.

---

## Setup

 1. Add create a timeline and add it to a GameObject using a PlayableDirector
 
 2. In that same GameObject, add a SuperController.
 
 3. Change the type of input to record to the one you choose.
 
 ## Usage
 You must add 
 ```csharp
 using Lopea.SuperControls;
 ```
 to the top of your script before using any SuperControl functions.
 ### Keyboard
 To implement SuperControls into your keyboard input, replace
```csharp
Input.Getkey(KeyCode.A);
```
to
```csharp
SuperInput.GetKey(KeyCode.A);
```
Dont worry! Any current input will still apply. 

#### Supported Functions
```csharp
SuperInput.GetKey(KeyCode key);
SuperInput.GetKeyDown(KeyCode key);
```
---
## TODO
- finish keyboard functions
- Add Mouse position Integration
- add Unity InputSystem Integration
