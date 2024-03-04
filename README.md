# Apex Extermination Lone Operative

Link to Github source code: https://github.com/curtisackland/4483-game

## Features in this Prototype

This a horizontal slice of Apex Extermination: Lone Operative made in Unity. I have added a user interface for player stats (XP and health) and statuses, a custom open world, first person player movement mechanics, player combat mechanics with two types of weapons, and one type of enemy that has custom behaviour and spawning mechanics.

User Interface:
- Health bar that chips away health when damaged or healed
- XP bar that shows XP level progress
- Health and XP level are also shown to the left of the health and xp bar
- When damaged, a damage overlay will appear for a short time and fade away
- Reload progress bar when reloading
- Crosshair in the middle of the screen
- Current ammo count and magazine size
- Pause menu that stops the game and gives the user the option to resume or exit the game

Open World:
- Big open world that the player can explore
- Custom generated terrain with hills, mountains, and trees
- The player's base is a cargo container that they can go to get health back and stay as a safe place. Can open and close the door to protect the player.

Player:
- Player can move around, sprint, crouch, jump
- First person camera attached to player that is the player's point of view

Player Combat:
- Player can swap between two guns (AR and Pistol) that each have a different stats (rate of fire, ammo count, etc.)
- Player can aim down sites with each weapon
- Player can shoot each gun and damage enemies
- Shooting creates bullet tracers of where the bullet went
- Shooting decreases ammo count
- Reloading replenishes ammo
- When player kills an enemy - XP is added to the xp bar
- When the player is damaged or healed, this is reflected in the health bar
- When the player dies, they are given a prompt that they died and teleport back to their spawn location

Enemies:
- Enemies will target players once the player is in their field of view
- When an Enemy targets the player they will shoot at the player (sometimes missing) and erratically move around the area to make it harder for the player to hit them.
- When the player moves out of their field of view they will travel to the player's last know position and search around that area.
- Enemy spawning:
    - Maximum of 5 enemies can be in the world
    - Enemies randomly spawn between a maximum radius and minimum radius from the player so they do not spawn too far or too close
    - When an enemy becomes too far away from the player they will despawn
    - When the total number of enemies drops to 3 or less, mor enemies will be spawned aronud the player

## How to Install

If you have the Build folder, run the Apex Extermination Lone Operative.exe.

If you don't have the build folder, you will need to open the project in Unity and build it and then run it.

## How to Play

The player will spawn into Area52 in a container with supplies in it. The player must eliminate enemies and level up while they explore Area52.

Movement:
- WASD to move
- Space to jump
- Shift to toggle sprint
- Ctrl to toggle crouch

Combat:
- Use mouse to aim
- Left click to shoot
- Right click to aim down sights
- R to reload
- 1 to swap to first weapon
- 2 to swap to second weapon

Menu:
- ESC to bring up the pause menu

## Challenges

I am still a beginner with Unity so I was exposed to many new technologies through this project that I had to learn to put this prototype together. This took several days of work to put all together however I am happy with the results and it should be easy to add more features with how the project is setup. The most difficult challenge I had was making shooting look decent. I wanted to be able to aim down sites or hip fire with bullet tracers coming from the barrel of the gun while the actual raycast to hit enemies came from the main camera so it would point to where the player was actually looking. This was hard to line up between hip fire state and aim down site state all while making the animation look smooth between states. All the other features in the game required a learning curve but I am, for the most part, happy  with how they turned out for this prototype.

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

