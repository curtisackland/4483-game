# Apex Extermination Lone Operative

Link to Github source code: https://github.com/curtisackland/4483-game


## How to Install

If you have the Build folder, run the Apex Extermination Lone Operative.exe.

If you don't have the build folder, you will need to open the project in Unity 2022.3.9f1, build and then run the game.

## How to Play

The player will spawn into Area52 in a container with supplies in it.
The player must eliminate enemies and level up while they explore Area52.

Movement:
- WASD to move
- Space to jump
- Shift to toggle sprint
- Ctrl to toggle crouch

Interact:
- Use E to interact

Inventory:
- TAB to open your inventory. You can move weapons around by dragging and dropping.

Combat:
- Use mouse to aim
- Left click to shoot
- Right click to aim down sights
- R to reload
- 1,2,3,4 to swap to that respective weapon

Menu:
- ESC to bring up the pause menu


## Features in this Demo

This version of Apex Extermination: Lone Operative made in Unity.
We have added a user interface for player stats (XP and health) and statuses, a custom open world, 
first person player movement mechanics, player combat mechanics with several types of weapons, 
and several types of enemy that have custom behaviour and spawning mechanics.

User Interface:
- Health bar that chips away health when damaged or healed
- XP bar that shows XP level progress
- Health and XP level are also shown to the left of the health and xp bar
- When damaged, a damage overlay will appear for a short time and fade away
- Reload progress bar when reloading
- Crosshair in the middle of the screen
- Current ammo count and magazine size
- Mission objectives in the top left
- Compass in the top left to tell the player which direction they're facing (this is to help locate objectives)
- Boss health bar in the top middle of the screen when fighting a boss
- Main menu screen that lets the user start or exit the game
- Pause menu that stops the game and gives the user the option to resume or exit the game. It also shows the controls for the game
- Shop menu that displays several guns that players can buy if they have enough monster points and xp
- Inventory menu where players can check their guns, amount of each type of ammo, and reorganize their weapon loadout by dragging and dropping guns into different slots.
- Victory Screen - players can restart or exit the game after completing the game.

Open World:
- Big open world that the player can explore
- Custom terrain with hills, mountains, trees, and buildings
- The player's base is a cargo container that they can go to get health (healthpack), ammo (ammo crate), and buy weapons from the store (large wooden crate/container below the ammo crate). It is also used as a safe place where the player can open and close the door to protect themselves
- There is smoke around the map to indicate health and ammo stations
- There are three boss areas
  - Additional enemies spawn 

Player:
- Player can move around, sprint, crouch, jump
- First person camera attached to player that is the player's point of view

Player Combat:
- Player can swap between many guns that each have a different stats (rate of fire, ammo count, damage, recoil etc.)
- Player can only have 4 guns equipped/in their weapons slots at one time (can hold more in their inventory)
- Player can aim down sites with each weapon
    - Aiming down sites with a sniper creates a sniper scope overlay
- Player can shoot each gun and damage enemies
- Shooting creates bullet tracers of where the bullet went
- Bullet impact effects
- Different types ammo for different weapon types: Pistol, AR, Sniper, Shotgun
- Shooting decreases ammo count of that specific gun's ammo type
- Reloading replenishes ammo
- When player kills an enemy - XP is added to the xp bar
- When the player is damaged or healed, this is reflected in the health bar
- When the player dies, they are given a prompt that they died and teleport back to their spawn location

Enemies:
- 3 different enemy types - Zombie, Cyber Zombie, and Small Demon
- 3 bosses - Demon, Monkey, Mutant 
- Enemies will target players once the player is in their field of view
- Each enemy has a different ability like: shooting, a lot of health, or have fast movement
- When the player moves out of their field of view they will travel to the player's last known position and search around that area.
- Enemy spawning:
    - Maximum of 20 enemies can spawn around the player
    - Enemies randomly spawn between a maximum radius and minimum radius from the player so they do not spawn too far or too close
    - When an enemy becomes too far away from the player they will despawn
    - Additional enemies can spawn in the boss areas

Audio:
- Guns and many different interactions have different audio cues
- Music throughout the game: in the main menu, ingame, boss music, and victory music
- Enemies make different sounds like idle, and attacking noises
- Interactable items have sound like the container doors, store, ammo refill, and first aid pack

## Challenges

We are all still beginners with Unity so we were exposed to many new technologies through this project that we had to learn to put this game together.
This took weeks of work to put all together however we are happy with the results and it should be easy to add more features in the future, if we want to, with how we have the project setup.
The most difficult challenge I had was making shooting look decent.
I wanted to be able to aim down sites or hip fire with bullet tracers coming from the barrel of the gun while the actual raycast to hit enemies came from the main camera so it would point to where the player was actually looking.
This was hard to line up between hip fire state and aim down site state all while making the animation look smooth between states.
The custom recoil system was also hard to get right.
It was hard to make each different gun have an appropriate recoil.
The inventory system was also a challenge and decent chunk of work.
Getting the drag and drop feature to work along with storing multiple different guns took some planning and reorganizing, but in the end worked out really well.
Animations for the enemies and bosses were extremely challenging to get right and work correctly so that the enemies actually felt like they were attacking the player.
Balancing/tuning weapons, monster health, xp, weapon costs, and enemy stats took a long time to find a place where everything felt even.

## Resources

Med Kit:
https://assetstore.unity.com/packages/3d/props/tools/survival-game-tools-139872

Guns:
https://assetstore.unity.com/packages/3d/props/guns/guns-pack-low-poly-guns-collection-192553

Cargo crate:
https://assetstore.unity.com/packages/3d/environments/industrial/rpg-fps-game-assets-for-pc-mobile-industrial-set-v2-0-86679

Sky Box:
https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353

Terrain assets:
https://assetstore.unity.com/packages/3d/environments/lowpoly-environment-nature-free-medieval-fantasy-series-187052

Base Terrain Kit:
https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808

Most of the game sounds:
https://freesound.org/\

Main Menu Music:
https://www.youtube.com/watch?v=FdCVU4vF1ZU

Boss Battle Music:
https://www.youtube.com/watch?v=U8FczHSlKBo

Victory Music:
https://www.youtube.com/watch?v=hXR1361koMo