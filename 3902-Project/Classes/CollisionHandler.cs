using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Sprint5
{
	public enum ObjectType
	{
		PLAYER,
		ENEMY,
		ITEM,
		BLOCK,
		FLAG,
		PIPE,
		PROJECTILE,
		SHELL,
		DEBRIS,
		UNKNOWN
	}

	public class CollisionObject
    {
		public IGameObject GameObject;
    }

	public class CollisionRectangle
    {
		public Rectangle Intersection;
    }

	public class CollisionHandler: ICollisionHandler
	{
		public IDictionary<Tuple<ObjectType, ObjectType, Direction>, List<ICommand>> Dictionary { get; set; }
		private GameState gameState;
		private CollisionRectangle intersection = new CollisionRectangle();
		private CollisionObject collider = new CollisionObject();
		private CollisionObject collided = new CollisionObject();

		public CollisionHandler(GameState gameState)
        {
			this.gameState = gameState;

			Dictionary = new Dictionary<Tuple<ObjectType, ObjectType, Direction>, List<ICommand>>();

			AddPlayerCollisions();

			AddEnemyCollisions();

			AddItemCollisions();

			AddProjectileCollisions();

			AddBlockCollisions(); //Block needs to add item to list when collided this will be handled in block now

			AddShellCollisions();


		}

		public void HandleCollision(IGameObject collider, IGameObject collided, Direction direction, Rectangle intersection) 
        {
			this.collider.GameObject = collider;
			this.collided.GameObject = collided;
			this.intersection.Intersection = intersection;
			ObjectType type1 = GetObjectType(this.collider.GameObject);
			ObjectType type2 = GetObjectType(this.collided.GameObject);
			List<ICommand> commands = Dictionary[new Tuple<ObjectType, ObjectType, Direction>(type1, type2, direction)];
			ExecuteCommands(commands);
        }

		private void ExecuteCommands(List<ICommand> commands)
        {
            foreach (ICommand command in commands)
            {
				command.Execute();
            }
        }

		private ObjectType GetObjectType(IGameObject object1)
        {
			ObjectType objectType;
			Type type = object1.GetType();
			if(type.Equals(typeof(Koopa)) || type.Equals(typeof(Goomba)))
            {
				objectType = ObjectType.ENEMY;
            }
            else if (type.Equals(typeof(Mario)))
            {
				objectType = ObjectType.PLAYER;
			}
			else
            {
				objectType = GetCustomObjectType(object1);
			}
			return objectType;
        }

		private ObjectType GetCustomObjectType(IGameObject object1)
		{
			ObjectType objectType;
			Type type = object1.GetType();
            if (type.Equals(typeof(Block)))
            {
				objectType = ObjectType.BLOCK;
            }
			else if (type.Equals(typeof(Pipe)))
			{
				objectType = ObjectType.PIPE;
			}
			else if (type.Equals(typeof(Flag)))
			{
				objectType = ObjectType.FLAG;
            }
			else if (type.Equals(typeof(Fireball)))
			{
				objectType = ObjectType.PROJECTILE;
			}
			else if (type.Equals(typeof(Shell)))
			{
				objectType = ObjectType.SHELL;
			}
			else if (type.Equals(typeof(Debris)))
			{
				objectType = ObjectType.DEBRIS;
			}
			else
            {
				objectType = ObjectType.ITEM;
			}
			return objectType;
		}

		//Shell Collisions
		private void AddShellCollisions()
		{
			ObjectType type = ObjectType.SHELL;
			ICommand collideBottom = new GameObjectCollideBottomCommand(collider, collided, intersection, gameState);
			ICommand collideTop = new GameObjectCollideTopCommand(collider, collided, intersection, gameState);
			ICommand collideLeft = new GameObjectCollideLeftCommand(collider, collided, intersection, gameState);
			ICommand collideRight = new GameObjectCollideRightCommand(collider, collided, intersection, gameState);

			AddToDictionary(type, ObjectType.BLOCK, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.BLOCK, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.BLOCK, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.BLOCK, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.SHELL, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.SHELL, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.SHELL, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.SHELL, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.ENEMY, Direction.RIGHT, new List<ICommand> { new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.ENEMY, Direction.LEFT, new List<ICommand> { new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.ENEMY, Direction.TOP, new List<ICommand> { new EnemyTakeDamageCommand(collider, gameState) }); 
			AddToDictionary(type, ObjectType.ENEMY, Direction.BOTTOM, new List<ICommand> { new EnemyTakeDamageCommand(collided, gameState) });

			AddToDictionary(type, ObjectType.PIPE, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.PIPE, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.PIPE, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.PIPE, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.FLAG, Direction.RIGHT, new List<ICommand> { collideLeft }); 
			AddToDictionary(type, ObjectType.FLAG, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.FLAG, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.FLAG, Direction.BOTTOM, new List<ICommand> { collideTop });


			AddToDictionary(type, ObjectType.DEBRIS, Direction.RIGHT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.LEFT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.TOP, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.BOTTOM, new List<ICommand> { });

		}

		private void AddPlayerCollisions()
        {
			ObjectType type = ObjectType.PLAYER;
			ICommand collideBottom = new GameObjectCollideBottomCommand(collider, collided, intersection, gameState);
			ICommand collideTop = new GameObjectCollideTopCommand(collider, collided, intersection, gameState);
			ICommand collideLeft = new GameObjectCollideLeftCommand(collider, collided, intersection, gameState); 
			ICommand collideRight = new GameObjectCollideRightCommand(collider, collided, intersection, gameState);

			AddToDictionary(type, ObjectType.BLOCK, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.BLOCK, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.BLOCK, Direction.TOP, new List<ICommand> { collideBottom, new BlockBounceCommand(collider, collided, gameState) }); 
			AddToDictionary(type, ObjectType.BLOCK, Direction.BOTTOM, new List<ICommand>{ collideTop, new BlockBumpCommand(collider, collided, gameState) });


			AddToDictionary(type, ObjectType.SHELL, Direction.RIGHT, new List<ICommand> { new MarioTakeDamageCommand(collider, collided, gameState), new KickShellCommand(collided, false) });
			AddToDictionary(type, ObjectType.SHELL, Direction.LEFT, new List<ICommand> { new MarioTakeDamageCommand(collider, collided, gameState), new KickShellCommand(collided, true) });
			AddToDictionary(type, ObjectType.SHELL, Direction.TOP, new List<ICommand> { collideBottom, new EnemyTakeDamageCommand(collided, gameState), new BounceCommand(collider) });
			AddToDictionary(type, ObjectType.SHELL, Direction.BOTTOM, new List<ICommand> { new MarioTakeDamageCommand(collider, collided, gameState) });


			AddToDictionary(type, ObjectType.ENEMY, Direction.RIGHT, new List<ICommand> { new MarioTakeDamageCommand(collider, collided, gameState)});
			AddToDictionary(type, ObjectType.ENEMY, Direction.LEFT, new List<ICommand> { new MarioTakeDamageCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.ENEMY, Direction.TOP, new List<ICommand> { collideBottom, new EnemyTakeDamageCommand(collided, gameState), new BounceCommand(collider) });
			AddToDictionary(type, ObjectType.ENEMY, Direction.BOTTOM, new List<ICommand> { new MarioTakeDamageCommand(collider, collided, gameState) });
			

			AddToDictionary(type, ObjectType.PIPE, Direction.RIGHT, new List<ICommand> { collideLeft , new MarioEnterRightPipeCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.PIPE, Direction.LEFT, new List<ICommand> { collideRight , new MarioEnterLeftPipeCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.PIPE, Direction.TOP, new List<ICommand> { collideBottom , new MarioEnterTopPipeCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.PIPE, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.FLAG, Direction.RIGHT, new List<ICommand> { collideLeft, new FlagInteractCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.FLAG, Direction.LEFT, new List<ICommand> { collideRight, new FlagInteractCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.FLAG, Direction.TOP, new List<ICommand> { collideBottom, new FlagInteractCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.FLAG, Direction.BOTTOM, new List<ICommand> { collideTop, new FlagInteractCommand(collider, collided, gameState) });


			AddToDictionary(type, ObjectType.ITEM, Direction.RIGHT, new List<ICommand> { new MarioCollectCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.ITEM, Direction.LEFT, new List<ICommand> { new MarioCollectCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.ITEM, Direction.TOP, new List<ICommand> { new MarioCollectCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.ITEM, Direction.BOTTOM, new List<ICommand> { new MarioCollectCommand(collider, collided, gameState) });

			AddToDictionary(type, ObjectType.DEBRIS, Direction.RIGHT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.LEFT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.TOP, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.BOTTOM, new List<ICommand> { });

		}

		private void AddEnemyCollisions()
		{
			ObjectType type = ObjectType.ENEMY;
			ICommand collideBottom = new GameObjectCollideBottomCommand(collider, collided, intersection, gameState);
			ICommand collideTop = new GameObjectCollideTopCommand(collider, collided, intersection, gameState);
			ICommand collideLeft = new GameObjectCollideLeftCommand(collider, collided, intersection, gameState);
			ICommand collideRight = new GameObjectCollideRightCommand(collider, collided, intersection, gameState);

			AddToDictionary(type, ObjectType.BLOCK, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.BLOCK, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.BLOCK, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.BLOCK, Direction.BOTTOM, new List<ICommand> { collideTop }); 

			AddToDictionary(type, ObjectType.ENEMY, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.ENEMY, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.ENEMY, Direction.TOP, new List<ICommand> { collideBottom }); 
			AddToDictionary(type, ObjectType.ENEMY, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.SHELL, Direction.RIGHT, new List<ICommand> { collideLeft, new EnemyTakeDamageCommand(collider, gameState) });
			AddToDictionary(type, ObjectType.SHELL, Direction.LEFT, new List<ICommand> { collideRight, new EnemyTakeDamageCommand(collider, gameState) });
			AddToDictionary(type, ObjectType.SHELL, Direction.TOP, new List<ICommand> { collideBottom, new EnemyTakeDamageCommand(collider, gameState) });
			AddToDictionary(type, ObjectType.SHELL, Direction.BOTTOM, new List<ICommand> { collideTop, new EnemyTakeDamageCommand(collider, gameState) });

			AddToDictionary(type, ObjectType.PIPE, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.PIPE, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.PIPE, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.PIPE, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.FLAG, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.FLAG, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.FLAG, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.FLAG, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.ITEM, Direction.RIGHT, new List<ICommand> {  });
			AddToDictionary(type, ObjectType.ITEM, Direction.LEFT, new List<ICommand> {  });
			AddToDictionary(type, ObjectType.ITEM, Direction.TOP, new List<ICommand> { });
			AddToDictionary(type, ObjectType.ITEM, Direction.BOTTOM, new List<ICommand> { });

			AddToDictionary(type, ObjectType.DEBRIS, Direction.RIGHT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.LEFT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.TOP, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.BOTTOM, new List<ICommand> { });

		}

		private void AddProjectileCollisions()
		{
			ObjectType type = ObjectType.PROJECTILE;
			ICommand collideBottom = new GameObjectCollideBottomCommand(collider, collided, intersection, gameState);
			ICommand collideTop = new GameObjectCollideTopCommand(collider, collided, intersection, gameState);
			ICommand collideLeft = new GameObjectCollideLeftCommand(collider, collided, intersection, gameState);
			ICommand collideRight = new GameObjectCollideRightCommand(collider, collided, intersection, gameState);

			AddToDictionary(type, ObjectType.BLOCK, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.BLOCK, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.BLOCK, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.BLOCK, Direction.BOTTOM, new List<ICommand> { collideTop });


			AddToDictionary(type, ObjectType.ENEMY, Direction.RIGHT, new List<ICommand> { collideLeft, new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.ENEMY, Direction.LEFT, new List<ICommand> { collideRight, new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.ENEMY, Direction.TOP, new List<ICommand> { collideBottom, new EnemyTakeDamageCommand(collided, gameState) }); 
			AddToDictionary(type, ObjectType.ENEMY, Direction.BOTTOM, new List<ICommand> { collideTop, new EnemyTakeDamageCommand(collided, gameState) });

			AddToDictionary(type, ObjectType.SHELL, Direction.RIGHT, new List<ICommand> { collideLeft, new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.SHELL, Direction.LEFT, new List<ICommand> { collideRight, new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.SHELL, Direction.TOP, new List<ICommand> { collideBottom, new EnemyTakeDamageCommand(collided, gameState) });
			AddToDictionary(type, ObjectType.SHELL, Direction.BOTTOM, new List<ICommand> { collideTop, new EnemyTakeDamageCommand(collided, gameState) });


			AddToDictionary(type, ObjectType.PIPE, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.PIPE, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.PIPE, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.PIPE, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.FLAG, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.FLAG, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.FLAG, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.FLAG, Direction.BOTTOM, new List<ICommand> { collideTop });


			AddToDictionary(type, ObjectType.PLAYER, Direction.RIGHT, new List<ICommand> { collideLeft, new MarioTakeDamageCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.PLAYER, Direction.LEFT, new List<ICommand> { collideRight, new MarioTakeDamageCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.PLAYER, Direction.TOP, new List<ICommand> { collideBottom, new MarioTakeDamageCommand(collider, collided, gameState) });
			AddToDictionary(type, ObjectType.PLAYER, Direction.BOTTOM, new List<ICommand> { collideTop, new MarioTakeDamageCommand(collider, collided, gameState) });

		}
		private void AddItemCollisions()
		{
			ObjectType type = ObjectType.ITEM;
			ICommand collideBottom = new GameObjectCollideBottomCommand(collider, collided, intersection, gameState);
			ICommand collideTop = new GameObjectCollideTopCommand(collider, collided, intersection, gameState);
			ICommand collideLeft = new GameObjectCollideLeftCommand(collider, collided, intersection, gameState);
			ICommand collideRight = new GameObjectCollideRightCommand(collider, collided, intersection, gameState);

			AddToDictionary(type, ObjectType.BLOCK, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.BLOCK, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.BLOCK, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.BLOCK, Direction.BOTTOM, new List<ICommand> { collideTop }); 

			AddToDictionary(type, ObjectType.PIPE, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.PIPE, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.PIPE, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.PIPE, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.FLAG, Direction.RIGHT, new List<ICommand> { collideLeft });
			AddToDictionary(type, ObjectType.FLAG, Direction.LEFT, new List<ICommand> { collideRight });
			AddToDictionary(type, ObjectType.FLAG, Direction.TOP, new List<ICommand> { collideBottom });
			AddToDictionary(type, ObjectType.FLAG, Direction.BOTTOM, new List<ICommand> { collideTop });

			AddToDictionary(type, ObjectType.DEBRIS, Direction.RIGHT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.LEFT, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.TOP, new List<ICommand> { });
			AddToDictionary(type, ObjectType.DEBRIS, Direction.BOTTOM, new List<ICommand> { });
		}

		private void AddBlockCollisions()
		{
			ObjectType type = ObjectType.ITEM;
			AddToDictionary(type, ObjectType.ENEMY, Direction.BOTTOM, new List<ICommand> { new EnemyTakeDamageCommand( collided, gameState)});

		}

		private void AddToDictionary(ObjectType type1, ObjectType type2, Direction direction, List<ICommand> commands)
        {
			Dictionary.Add(new Tuple<ObjectType, ObjectType, Direction>(type1, type2, direction), commands);

		}
	}
}