
# *Super Mario Bros.* 1.5

This project was created throughout the Spring 2022 semester in completion of CSE 3902 - Project: Interactive Systems by a team of 6 software developers.

This project presents an interactive, multi-level game inspired by Super Mario Bros. with custom additions.


## Features
- 4 original levels
- Ability to interact with level by placing blocks
- Bounce blocks
- New textures

- Mario can fall into pits, once he falls far enough it will give you the GAME OVER screen. While falling, Mario can still move left and right. With this and the ability to place blocks, it is possible to save Mario from falling all the way down and dying.
- In normal mario, Small Mario is unable to break brick blocks. But due to our use of placing blocks as a feature, it was decided that Small Mario would have the ability to break blocks in order to be able to place them and not get soft locked.

## Playing the Game
There are two currently-supported ways to play the game.  The easiest (and recommended) way is to download the appropriate executable from the [Releases](https://github.com/tgkasarcik/FinalBounty-Public/releases) section of this repo.  

IMPORTANT: Please install this font (https://www.urbanfonts.com/fonts/Emulogic.htm) in order to run the project solution.

## Screenshots

### Level 1: Tropical

<img src="https://user-images.githubusercontent.com/77713266/167062357-a9fba829-ae5f-4d9a-874a-fbba19112e01.png" alt="Level 1" title="Level 1: Tropical">

### Level 2: Fire

<img src="https://user-images.githubusercontent.com/77713266/167062362-174ee11f-8ab3-4517-9867-a546c5fcbb7c.png" alt="Level 2" title="Level 2: Fire">

### Level 3: Ice

<img src="https://user-images.githubusercontent.com/77713266/167062364-9b37d38e-8f13-45e7-8eef-59d98c7d9428.png" alt="Level 3" title="Level 3: Ice">

### Level 4: Ohio State v. Michigan

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
- **1** - Give Mario the Fire Powerup
- **2** - Give Mario the Mushroom Powerup
- **3** - Give Mario the Star Powerup
- **Left Mouse** - Return to previous level
- **Right Mouse** - Skip to next level

## Known Issues and Bugs
- Mario can occasionally clip through and climb over walls
- Music starts incorrectly when changing levels with Star Powerup enabled
- No main menu screen currently exists
- Fireflowers move and they should not
- There is no working lives system. Once a player dies, they must either restart or quit the game
