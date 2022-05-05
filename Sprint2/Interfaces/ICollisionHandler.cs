using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint5
{
	public enum Direction
    {
		LEFT,
		RIGHT,
		TOP,
		BOTTOM
    }
	public interface ICollisionHandler
	{
		IDictionary<Tuple<ObjectType, ObjectType, Direction>, List<ICommand>> Dictionary { get; set; }

		void HandleCollision(IGameObject object1, IGameObject object2, Direction direction, Rectangle Intersection);
	}
}
