
# The-BRAC-ETs

Sprint 5 is now done, see "Sprint 5 Final" release for our source code or go to "Sprint5" branch.
Even though the main folder is named Sprint2, it is in fact the full folder of the project that contains the Sprint 5 code.
Our ZenHub page can be found at: https://app.zenhub.com/workspaces/3902-61f46253ce5d81001af0d3f4/board?repos=453223397

IMPORTANT: To have the Text properly displaying in the game and not crash, please follow this link (https://www.urbanfonts.com/fonts/Emulogic.htm), download the font file, open the file on your computer, and press install. Then the game should display the right text and not crash.

Name of Project:
- CSE 3902: Sprint 5 - Extending Your Game Framework

Team Name:
The BRAC-ETs

Description of Project:
- This project presents a working, interactive, multi-level Mario game with custom additions. Custome features include bounce blocks, abaility to place blocks, custom levels and textures.
- Upon completion of a level, the next level will automatically be loaded for the player.

Authors:
- Alek Srode
- Ruidong Zhang
- Eric Chen
- Ben Borszcz
- Tommy Kasarcik
- Catherine Quamme

Commands:
All commands are keyboard and mouse based, there are no game controller/gamepad controls.

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

Known Issues and/or Bugs:
- Mario has some times where he will be able to clip into a wall and jump up it almost like a ladder
- Mario can fall into pits, once he falls far enough it will give you the GAME OVER screen. While falling, Mario can still move left and right. With this and the ability to place blocks, it is possible to save Mario from falling all the way down and dying.
- When changing levels with the Star power before it has ended, the new area loaded will play its music and override the Star Power music, then Mario's star power will cause the song played for the level to start over from the beginning after officially ending.
- There is currently no "Level Start" screen that has been fully implemented. We have a screen for this, but the process of drawing it was not coded in due to time limits and difficulty.
- There is no working lives system. Once you die, instead of fully restarting from the very first level, you can choose to reset yourself. This was more of a choice to leave as is rather than a problem.
- Fireflowers move and they should not.
- In normal mario, Small Mario is unable to break brick blocks. But due to our use of placing blocks as a feature, it was decided that Small Mario would have the ability to break blocks in order to be able to place them and not get soft locked.

A General Note: There are a couple TODO comments throughout the code. They were spots where we intended to do more, but didn't have the time and so they were deemed less necessary than other things. If we had more time/more sprints, we would go back in and finish those.
