using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace Sprint5
{
    public enum PipeType
	{
        None,   //None is used for the sake of "nulling" a variable holding a PipeType, that way if a pipe does not lead to another pipe but instead places Mario in world, then anything managing the interaction of that pipe knows to do so
        UpPipe,
        UpPipeBase,
        LeftPipe,
        RightPipe,
        CornerLeftPipe,
        CornerRightPipe
	}

    public class Pipe : IStructure, IInteractable
    {
        public ISprite Sprite { get; set; }
        public IUseability UState { get; set; }
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }
        public PipeType pipeType;
        public int NextArea;
        public Vector2 OutLoc;
        public PipeType outPipeType;

        private static SpriteFactory SpriteFactory;

        //Constructor for a non interactive pipe
        public Pipe(Vector2 location, SpriteFactory spriteFactory, PipeType type)
        {
            SpriteFactory = spriteFactory;
            Location = location;
            destroyObject = false;
            UState = new SolidPipe(this);

            //NOTE: may need to change later
            pipeType = type;
            Sprite = spriteFactory.GetSprite(type.ToString());
            Sprite.Location = location;
        }

        public Pipe(Vector2 location, SpriteFactory spriteFactory, PipeType type, int nextArea, Vector2 outLoc, PipeType outPipeType)
        {
            SpriteFactory = spriteFactory;
            Location = location;
            destroyObject = false;
            this.outPipeType = outPipeType;
            UState = new InteractivePipe(this);
            NextArea = nextArea;
            OutLoc = outLoc;


            //NOTE: may need to change later
            pipeType = type;
            Sprite = spriteFactory.GetSprite(type.ToString());
            Sprite.Location = location;
        }

        public Rectangle GetHitBox()
        {
            return Sprite.GetHitBox();
        }
 


        public void PlayerInteract(Mario player, GameTime gametime)
        {
            if (UState.IsInteractive())
            {
                UState.ChangeUseability(gametime);
                SoundFactory.Instance.GetSoundEffect("PipeSound").Play();
                AnimationEngine.Instance.EnterPipe(gametime, player, this, NextArea);
                GameState.Instance.ExitingPipe = true;
                //GameState.Instance.GoToLevel(NextArea);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Sprite.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return Sprite.IsDrawn();
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            //Not Implemented for Sprint 3
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            //Not Implemented for Sprint 3
        }
        public void Update(GameTime gametime)
        {
            UState.Update(gametime);
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }

        public void Broken(GameTime gametime)
        {

        }
    }
}