
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint5
{
    /*
     * Singleton class used for creating all SoundEffect and Song objects.
     */
    public class SoundFactory
    {

        // Private SoundEffect Members         
        private SoundEffect OneUp;
        private SoundEffect BreakBlock;
        private SoundEffect BumpBlock;
        private SoundEffect CollectCoin;
        private SoundEffect ThrowFireball;
        private SoundEffect Flagpole;
        private SoundEffect GameOver;
        private SoundEffect SmallMarioJump;
        private SoundEffect BigMarioJump;
        private SoundEffect KickShell;
        private SoundEffect MarioDies;
        private SoundEffect PauseGame;
        private SoundEffect PipeSound;
        private SoundEffect CollectPowerUp;
        private SoundEffect PowerUpAppears;
        private SoundEffect ClearingLevel;
        private SoundEffect Stomp;
        private SoundEffect NoBlocks;
        private SoundEffect PlaceBlock;

        // Private Song Members
        private Song MarioRegularGameMusic;
        private Song MarioStarMusic;
        private Song MarioUndergroundMusic;
        private Song MarioCastleTheme;


        /*
         * Private Instance of <c> this </c>.
         */
        private static SoundFactory instance = new SoundFactory();

        /*
         * Public Instance of <c> this </c>.
         */
        public static SoundFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
        private SoundFactory() { }

        /*
         * Load all sounds.
         * 
         * NOTE: this will be moved into level loader soon
         */
        public void LoadSounds(ContentManager content)
        {
            OneUp = content.Load<SoundEffect>("smb_1-up");
            BreakBlock = content.Load<SoundEffect>("smb_breakblock");
            BumpBlock = content.Load<SoundEffect>("smb_bump");
            CollectCoin = content.Load<SoundEffect>("smb_coin");
            ThrowFireball = content.Load<SoundEffect>("smb_fireball");
            Flagpole = content.Load<SoundEffect>("smb_flagpole");
            GameOver = content.Load<SoundEffect>("smb_gameover");
            SmallMarioJump = content.Load<SoundEffect>("smb_jump-small");
            BigMarioJump = content.Load<SoundEffect>("smb_jump-super");
            KickShell = content.Load<SoundEffect>("smb_kick");
            MarioDies = content.Load<SoundEffect>("smb_mariodie");
            PauseGame = content.Load<SoundEffect>("smb_pause");
            PipeSound = content.Load<SoundEffect>("smb_pipe");
            CollectPowerUp = content.Load<SoundEffect>("smb_powerup");
            PowerUpAppears = content.Load<SoundEffect>("smb_powerup_appears");
            ClearingLevel = content.Load<SoundEffect>("smb_stage_clear");
            Stomp = content.Load<SoundEffect>("smb_stomp");
            NoBlocks = content.Load<SoundEffect>("emptyinventory");
            PlaceBlock = content.Load<SoundEffect>("game_placeblock");

            MarioRegularGameMusic = content.Load<Song>("smb_theme_high_bitrate");
            MarioUndergroundMusic = content.Load<Song>("smb_underground");
            MarioStarMusic = content.Load<Song>("smb_startheme");
            MarioCastleTheme = content.Load<Song>("smb_castletheme");

            //General volume settings so ears don't get absolutely blasted
            MediaPlayer.Volume = (float) 0.5;
            SoundEffect.MasterVolume = (float)0.5;
        
        }


        /*
         * Return a new Sound Effect based on a single name paramater.
         */
        public SoundEffect GetSoundEffect(String effectName)
        {
            SoundEffect returnVal;

            switch(effectName)
            {
                case "OneUp":
                    returnVal = OneUp;
                    break;
                case "BreakBlock":
                    returnVal = BreakBlock;
                    break;
                case "BumpBlock":
                    returnVal = BumpBlock;
                    break;
                case "CollectCoin":
                    returnVal = CollectCoin;
                    break;
                case "ThrowFireball":
                    returnVal = ThrowFireball;
                    break;
                case "Flagpole":
                    returnVal = Flagpole;
                    break;
                case "GameOver":
                    returnVal = GameOver;
                    break;
                case "SmallMarioJump":
                    returnVal = SmallMarioJump;
                    break;
                case "BigMarioJump":
                    returnVal = BigMarioJump;
                    break;
                case "KickShell":
                    returnVal = KickShell;
                    break;
                case "MarioDies":
                    returnVal = MarioDies;
                    break;
                case "PauseGame":
                    returnVal = PauseGame;
                    break;
                case "PipeSound":
                    returnVal = PipeSound;
                    break;
                case "CollectPowerUp":
                    returnVal = CollectPowerUp;
                    break;
                case "PowerUpAppears":
                    returnVal = PowerUpAppears;
                    break;
                case "ClearingLevel":
                    returnVal = ClearingLevel;
                    break;
                case "Stomp":
                    returnVal = Stomp;
                    break;
                case "NoBlocks":
                    returnVal = NoBlocks;
                    break;
                case "PlaceBlock":
                    returnVal = PlaceBlock;
                    break;
                default:
                    returnVal = null;
                    break;
            }

            return returnVal;

        }

        /*
        * Return a new Song based on a single name paramater.
        * 
        */
        
        public Song GetSong(String songName)
        {

            Song returnVal;
            
            switch(songName) {
                case "Theme":
                    returnVal = MarioRegularGameMusic;
                    break;
                case "Underground":
                    returnVal = MarioUndergroundMusic;
                    break;
                case "Star":
                    returnVal = MarioStarMusic;
                    break;
                case "Castle":
                    returnVal = MarioCastleTheme;
                    break;
                default:
                    returnVal = MarioRegularGameMusic;
                    break;
            }

            return returnVal;
        } 
    }
}
