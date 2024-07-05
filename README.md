
# *Super Mario Bros.* 1.5

This project was created throughout the Spring 2022 semester in completion of CSE 3902 - Project: Interactive Systems by a team of 6 software developers.

This project presents an interactive, multi-level game inspired by Super Mario Bros. with custom additions.


## Features:
- 4 original levels
- Ability to interact with level by placing blocks
- Bounce blocks
- New textures

## Screenshots

Level 1: Tropical

<img src="https://user-images.githubusercontent.com/77713266/167062357-a9fba829-ae5f-4d9a-874a-fbba19112e01.png" alt="Level 1" title="Level 1: Tropical">

Level 2: Fire

<img src="https://user-images.githubusercontent.com/77713266/167062362-174ee11f-8ab3-4517-9867-a546c5fcbb7c.png" alt="Level 2" title="Level 2: Fire">

Level 3: Ice

<img src="https://user-images.githubusercontent.com/77713266/167062364-9b37d38e-8f13-45e7-8eef-59d98c7d9428.png" alt="Level 3" title="Level 3: Ice">

Level 4: Ohio State v. Michigan

<img src="https://user-images.githubusercontent.com/77713266/167062367-48ff86cf-0610-4e2f-a57e-2516cc338512.png" alt="Level 4" title="Level 4: Ohio State v. Michigan">

## Controlls

### Movement
- **W** - Jump
- **A** - Move left
- **S** - Crouch
    - *Note: Small Mario cannot crouch*
- **D** - Move right
- **N** - Throw fireball
- **Left Arrow** - Place block to Mario's left
- **Down Arrow** - Place block below Mario
- **Up Arrow** - Place block above Mario
- **Right Arrow** - Place block to Mario's right

### Level Navigation
- **R** - Reset the current level
- **Q** - Quit the game

### Debug Controls
- **1** - Give Mario the Fire powerup
- **2** - Give Mario the Mushroom powerup
- **3** - Give Mario the Star powerup
- **Left Mouse** - Return to previous level
- **Right Mouse** - Skip to next level

## Known Issues and/or Bugs
- Mario can clip through and climb over walls occasionally


- Mario can fall into pits, once he falls far enough it will give you the GAME OVER screen. While falling, Mario can still move left and right. With this and the ability to place blocks, it is possible to save Mario from falling all the way down and dying.
- When changing levels with the Star power before it has ended, the new area loaded will play its music and override the Star Power music, then Mario's star power will cause the song played for the level to start over from the beginning after officially ending.
- There is currently no "Level Start" screen that has been fully implemented. We have a screen for this, but the process of drawing it was not coded in due to time limits and difficulty.
- There is no working lives system. Once you die, instead of fully restarting from the very first level, you can choose to reset yourself. This was more of a choice to leave as is rather than a problem.
- Fireflowers move and they should not.
- In normal mario, Small Mario is unable to break brick blocks. But due to our use of placing blocks as a feature, it was decided that Small Mario would have the ability to break blocks in order to be able to place them and not get soft locked.


IMPORTANT: Please install this font (https://www.urbanfonts.com/fonts/Emulogic.htm) in order to run the project solution.

A General Note: There are a couple TODO comments throughout the code. They were spots where we intended to do more, but didn't have the time and so they were deemed less necessary than other things. If we had more time/more sprints, we would go back in and finish those.
