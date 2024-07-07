using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Sprint5
{
    public enum BlockType
    {
        Question,
        CoinBlock,
        Brick,
        UsedBlock,
        GroundBlock,
        StructBlock,
        HiddenBlock,
        BounceBlock,
        Debris
    }
    public class Block : IStructure, IInteractable
    {
        public IUseability UState { get; set; }
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }

        //topCollideable bool

        private static SpriteFactory spriteFactory;
        public BlockType Type;
        private ISprite sprite;
        private bool collided;
        private int PositionAdjust = 5;
        private int coins;
        public Block(Vector2 location, SpriteFactory spritefactory, BlockType type, ItemType item, int coins)
        {
            this.coins = coins;
            spriteFactory = spritefactory;
            Location = location;
            Type = type;
            collided = false;
            destroyObject = false;
            if (type == BlockType.Question)
            {
                UState = new InteractiveQuestionBlock(this, item);
            }
            else if (type == BlockType.CoinBlock)
            {
                UState = new InteractiveCoinBlock(this, coins);
            }
            else if (type == BlockType.Brick)
            {
                UState = new InteractiveBrick(this);
            }
            else
            {
                UState = new SolidBlock(this);
            }
            sprite = spriteFactory.GetSprite(Type.ToString());
            sprite.Location = location;
        }

        public void PlayerInteract(Mario player, GameTime gametime)
        {
            UState.ChangeUseability(gametime);
        }


        public void Draw(SpriteBatch spritebatch)
        {
            sprite.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return sprite.IsDrawn();
        }

        public void Update(GameTime gametime)
        {
            BlockType temp = Type;
            if (collided)
            {
                if (temp == BlockType.CoinBlock && coins != 0)
                {
                    coins--;
                    collided = false;
                    if(coins == 0)
                    {
                        temp = BlockType.UsedBlock;
                    }
                }
            } else
            {
                collided = false;
            }

            if (UState.GetType().Name.Contains("Destroyed"))
            {
                temp = BlockType.UsedBlock;
            }

            if (temp != Type)
            {
				sprite = spriteFactory.GetSprite(BlockType.UsedBlock.ToString());
			}

            UState.Update(gametime);
            sprite.Location = Location;
            sprite.Update(gametime);
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            //Does nothing when collided in the X direction
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            if (isUp)
            {
                collided = true;
            }
        }

        public Rectangle GetHitBox()
        {
            return sprite.GetHitBox();
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }

        public void Broken(GameTime gametime)
        {
            DestroyObject();
            GameObjectManager.Instance.SkyObjectList.Add(new Debris(new Vector2(Location.X - PositionAdjust, Location.Y - PositionAdjust), DebrisType.BrickPiece1, gametime));
            GameObjectManager.Instance.SkyObjectList.Add(new Debris(new Vector2(Location.X + PositionAdjust, Location.Y - PositionAdjust), DebrisType.BrickPiece2, gametime));
            GameObjectManager.Instance.SkyObjectList.Add(new Debris(new Vector2(Location.X - PositionAdjust, Location.Y + PositionAdjust), DebrisType.BrickPiece3, gametime));
            GameObjectManager.Instance.SkyObjectList.Add(new Debris(new Vector2(Location.X + PositionAdjust, Location.Y + PositionAdjust), DebrisType.BrickPiece4, gametime));
        }
    }
}