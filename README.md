
# CSE 3902 Group Project

IMPORTANT: Please install this font (https://www.urbanfonts.com/fonts/Emulogic.htm) in order to run the project solution.

**Team Name:**
The BRAC-ETs

This project presents an interactive, multi-level game inspired by Super Mario Bros. with custom additions.

**Authors:**
- Alek Srode
- Ruidong Zhang
- Eric Chen
- Ben Borszcz
- Tommy Kasarcik
- Catherine Quamme

**Features:**
- 4 original levels
- Ability to interact with level by placing blocks
- Bounce blocks
- New textures

**Screenshots**

Level 1: Tropical

<img src="https://user-images.githubusercontent.com/77713266/167062357-a9fba829-ae5f-4d9a-874a-fbba19112e01.png" alt="Level 1" title="Level 1">

Level 2: Fire

<img src="https://user-images.githubusercontent.com/77713266/167062362-174ee11f-8ab3-4517-9867-a546c5fcbb7c.png" alt="Level 2" title="Level 2">

Level 3: Ice

<img src="https://user-images.githubusercontent.com/77713266/167062364-9b37d38e-8f13-45e7-8eef-59d98c7d9428.png" alt="Level 3" title="Level 3">

Level 4: Ohio State v. Michigan

<img src="https://user-images.githubusercontent.com/77713266/167062367-48ff86cf-0610-4e2f-a57e-2516cc338512.png" alt="Level 4" title="Level 4">

**Controlls:**

Mario Movement:
- 'D' to move Mario right.
- 'A' to move Mario left.
- 'S' to have Mario crouch.
    - Note: Neither Small Mario nor Dead Mario can crouch
- 'N' will have Fire Mario throw a fireball.
- 'W' will make Mario jump
    - Note: You can hold the jump button down to make him jump higher, and only tapping the button will make him jump a small amount
    - The smallest jump height DOES have a minimum though
- 'R' to reset the current level
    - Switching between levels also resets current HUD statistics as well as Mario's state(s)
    - If Mario dies, reseting the level will allow play again.
- '1' will give Mario the Fireflower power
    - Fireflower power will change Mario to FireMario and allows him to take two hits of damage before dying as well as the ability to throw fireballs
- '2' will give Mario the Mushroom power
    - Mushroom power will change Mario from Small to Big and allows him to take one hit from an enemy without dying but should then become Small
- '3' will give Mario the Star power
    - Star power should last about 10 seconds
- 'RMB' will switch to next level
- 'LMB' will switch to previous level
    - Cannot switch levels while Mario is dead, must reset level first
- Left, Right, Down, Up Arrow Keys will, given Mario has blocks, place blocks in the level relative to Mario's position in the direction pressed
	- e.g. Press right arrow to place a block to Mario's right; similar case for the other arrow keys

Reset and Quit Commands:
- 'Q' to quit the game.

**Known Issues and/or Bugs:**
- Mario has some times where he will be able to clip into a wall and jump up it almost like a ladder
- Mario can fall into pits, once he falls far enough it will give you the GAME OVER screen. While falling, Mario can still move left and right. With this and the ability to place blocks, it is possible to save Mario from falling all the way down and dying.
- When changing levels with the Star power before it has ended, the new area loaded will play its music and override the Star Power music, then Mario's star power will cause the song played for the level to start over from the beginning after officially ending.
- There is currently no "Level Start" screen that has been fully implemented. We have a screen for this, but the process of drawing it was not coded in due to time limits and difficulty.
- There is no working lives system. Once you die, instead of fully restarting from the very first level, you can choose to reset yourself. This was more of a choice to leave as is rather than a problem.
- Fireflowers move and they should not.
- In normal mario, Small Mario is unable to break brick blocks. But due to our use of placing blocks as a feature, it was decided that Small Mario would have the ability to break blocks in order to be able to place them and not get soft locked.

A General Note: There are a couple TODO comments throughout the code. They were spots where we intended to do more, but didn't have the time and so they were deemed less necessary than other things. If we had more time/more sprints, we would go back in and finish those.
