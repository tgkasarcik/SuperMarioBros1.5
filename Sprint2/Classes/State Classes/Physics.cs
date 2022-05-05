using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sprint5
{
    /*
     * ALL UNITS OF DISTANCE ARE IN PIXELS
     */
    class RigidPhysicsY : IPhysicsY
    {
        private static double GRAVITY_ACCELERATION = 1;   //Constant downwards acceleration
        private const double CONS_MAX_JUMP_VELOCITY_Y = 2.0, MAX_FALL_VELOCITY_Y = 3;   //Maximum velocity on the Y axis
        private static double VELOCITY_UPDATE_INTERVAL = 200;  //The number of milliseconds before the velocity can be updated again
        private static int NEXT_UPDATE_INTERVAL = 0;    //Used to specify if the update interval for the velocity has been reached
        private static int NO_MOVE = 0; //The amount moved when an object is not moving
        private double specifiedMaxJumpVel;
        private double y_Vel;   //Holds the Y velocity of the object
        private int y_Collision_Adjustment;  //Holds the value used to adjust the position of player after a collision
        private double lastMoveUpdate; //The value, in milliseconds, of the totalgametime at which the last movement update was made
        private bool grounded;  //Stores the truth of if Mario is on the ground

        public RigidPhysicsY()
        {
            y_Vel = NO_MOVE;
            y_Collision_Adjustment = NO_MOVE;
            lastMoveUpdate = NO_MOVE;
            specifiedMaxJumpVel = CONS_MAX_JUMP_VELOCITY_Y;
        }

        public int MovementAdjustmentY(GameTime gameTime, bool jumping)
        {
            if (jumping)    //Object is jumping
            {
                if (y_Vel == 0) y_Vel = -specifiedMaxJumpVel;    //Object has not started jumping yet, this begins the jump at max upwards velocity
            }
            else   //Object is not jumping
            {
                if (y_Vel < MAX_FALL_VELOCITY_Y && !grounded)
                {
                    if ((VELOCITY_UPDATE_INTERVAL.CompareTo((gameTime.TotalGameTime.TotalMilliseconds - lastMoveUpdate))) < NEXT_UPDATE_INTERVAL)
                    {
                        y_Vel += GRAVITY_ACCELERATION;
                        lastMoveUpdate = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                } else
                {
                    grounded = false;
                }
            }
            return (int) y_Vel;
        }

        public int CollisionAdjustmentY(Rectangle rectangle)
        {
            if (y_Vel > 0)  //Object was going down on collision, must move back up
            {
                y_Collision_Adjustment = -rectangle.Height;
                grounded = true;
            }
            else if (y_Vel < 0)//Player going up on collision, must move back to down
            {
                y_Collision_Adjustment = rectangle.Height;
            }

            y_Vel = NO_MOVE;

            return y_Collision_Adjustment;
        }

        public void AdjustYMovementVal(double newVal = CONS_MAX_JUMP_VELOCITY_Y)
        {
            specifiedMaxJumpVel = newVal;
        }
    }

    class BouncyPhysicsY : IPhysicsY
    {
        private static double GRAVITY_ACCELERATION = 1;   //Constant downwards acceleration
        private const double MAX_VELOCITY_Y = 4;   //Maximum velocity on the Y axis
        private static double VELOCITY_UPDATE_INTERVAL = 50;  //The number of milliseconds before the velocity can be updated again
        private static int NEXT_UPDATE_INTERVAL = 0;    //Used to specify if the update interval for the velocity has been reached
        private static int NO_MOVE = 0; //The amount moved when an object is not moving
        private double y_Vel;   //Holds the Y velocity of the object
        private int y_Collision_Adjustment;  //Holds the value used to adjust the position of player after a collision
        private double lastMoveUpdate; //The value, in milliseconds, of the totalgametime at which the last movement update was made

        public BouncyPhysicsY()
        {
            y_Vel = NO_MOVE;
            y_Collision_Adjustment = NO_MOVE;
            lastMoveUpdate = NO_MOVE;
        }

        public int MovementAdjustmentY(GameTime gameTime, bool jumping)    //The boolean value passed in here is not used/considered in any way for bouncing
        {
            if (y_Vel < MAX_VELOCITY_Y && (VELOCITY_UPDATE_INTERVAL.CompareTo((gameTime.TotalGameTime.TotalMilliseconds - lastMoveUpdate))) < NEXT_UPDATE_INTERVAL)
            {
                y_Vel += GRAVITY_ACCELERATION;  //Object is falling, must increase downward speed
                lastMoveUpdate = gameTime.TotalGameTime.TotalMilliseconds;
            }

            return (int) y_Vel;
        }

        public int CollisionAdjustmentY(Rectangle rectangle)
        {
            if (y_Vel > 0)  //Object was going down on collision, must move back up
            {
                y_Collision_Adjustment = -rectangle.Height;
            }
            else if (y_Vel <= 0)//Player going up on collision, must move back to down
            {
                y_Collision_Adjustment = rectangle.Height;
            }

            y_Vel = -MAX_VELOCITY_Y;    //Reverse the direction the object is going from down back to up, should make it appear to bounce

            return y_Collision_Adjustment;
        }

        public void AdjustYMovementVal(double newVal = MAX_VELOCITY_Y)
        {
            //Do nothing for now, will change if needed
        }
    }

    class PlayerPhysicsX : IPhysicsX
    {
        private static int X_ACCELERATION = 1, X_DECELERATION = 1;   //Constant acceleration and decceleration on the X axis
        private static int MAX_VELOCITY_X = 2;   //Maximum velocity on the X axis
        private static double VELOCITY_UPDATE_INTERVAL = 200;  //The number of milliseconds before the velocity can be updated again
        private static int NO_MOVE = 0; //The amount moved when an object is not moving
        private static int NEXT_UPDATE_INTERVAL = 0;    //Used to specify if the update interval for the velocity has been reached
        private double x_Vel;   //Holds the X velocity of the object
        private int x_Collision_Adjustment;  //Holds the value used to adjust the position of player after a collision
        private double lastMoveUpdate; //The value, in milliseconds, of the totalgametime at which the last movement update was made
        private bool moveLocked;    //States whether Mario is allowed to move or not (Used to prevent Mario from moving into an object)

        public PlayerPhysicsX()
        {
            moveLocked = false;
            x_Vel = NO_MOVE;
            x_Collision_Adjustment = NO_MOVE;
            lastMoveUpdate = NO_MOVE;
        }

        public int MovementAdjustmentX(GameTime gameTime, bool moving, bool faceRight)
        {
            if (moving && !moveLocked)   //Player is moving and movement is not locked?
            {
                if ((VELOCITY_UPDATE_INTERVAL.CompareTo((gameTime.TotalGameTime.TotalMilliseconds - lastMoveUpdate))) < NEXT_UPDATE_INTERVAL)   //Is the object's X velocity not maxed out in either direction yet and has reached the update interval to which it can change?
                {
                    if (faceRight && (x_Vel < MAX_VELOCITY_X))
                    {
                        if (x_Vel >= NO_MOVE) x_Vel += X_ACCELERATION;
                        if (x_Vel < NO_MOVE) x_Vel += X_DECELERATION;
                    } else if (!faceRight && (x_Vel > -MAX_VELOCITY_X))
                    {
                        if (x_Vel <= NO_MOVE) x_Vel -= X_ACCELERATION;
                        if (x_Vel > NO_MOVE) x_Vel -= X_DECELERATION;
                    }
                    
                    lastMoveUpdate = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else  if ((VELOCITY_UPDATE_INTERVAL.CompareTo((gameTime.TotalGameTime.TotalMilliseconds - lastMoveUpdate))) < NEXT_UPDATE_INTERVAL)//Player not moving
            {
                if (x_Vel < 0) x_Vel += X_DECELERATION; //Player has stopped moving entirely, was moving left, slowing down to no movement
                if (x_Vel > 0) x_Vel -= X_DECELERATION; //Player has stopped moving entirely, was moving right, slowing down to no movement
                lastMoveUpdate = gameTime.TotalGameTime.TotalMilliseconds;
            }
           
            moveLocked = false;
            
            return (int) x_Vel;
        }

        public int CollisionAdjustmentX(bool faceRight, Rectangle rectangle)
        {

            if (faceRight)  //Player facing right on collision, must move back to left
            {
                x_Collision_Adjustment = -rectangle.Width;
            }
            else  //Player facing left on collision, must move back to right
            {
                x_Collision_Adjustment = rectangle.Width;
            }

            x_Vel = NO_MOVE;

            moveLocked = true;

            return x_Collision_Adjustment;
        }
    }

    class EnemyPhysicsX : IPhysicsX
    {
        private double MAX_VELOCITY_X;   //Maximum velocity on the x axis
        private static int NO_MOVE = 0; //The amount moved when an object is not moving
        private double x_Vel;   //Holds the X velocity of the enemy
        private int x_Collision_Adjustment;  //Holds the value used to adjust the position of enemy after a collision

        public EnemyPhysicsX(double maxVelocity)
        {
            MAX_VELOCITY_X = maxVelocity;
            x_Vel = MAX_VELOCITY_X;
            x_Collision_Adjustment = NO_MOVE;
        }

        public int MovementAdjustmentX(GameTime gameTime, bool moving, bool faceRight)
        {
            if (moving)
            {
                if(faceRight) return (int)x_Vel;
                return -(int)x_Vel;  //Enemies stay at the same speed in the X direction
            }
            else
            {
                return NO_MOVE;    //Enemy has stopped moving and is therefore not going to go anywhere
            }
        }

        public int CollisionAdjustmentX(bool faceRight, Rectangle rectangle)
        {
            //x_Vel = -x_Vel; //Reverses the direction in which the enemy is moving

            if (faceRight)  //Enemy was moving right on collision, must move back to left
            {
                x_Collision_Adjustment = -rectangle.Width;
            }
            else  //Enemy was moving left on collision, must move back to right
            {
                x_Collision_Adjustment = rectangle.Width;
            }

            return x_Collision_Adjustment;
        }
    }

    class ItemPhysicsX : IPhysicsX
    {
        private static int NO_MOVE = 0; //The amount moved when an object is not moving
        private double x_Vel;   //Holds the X velocity of the item
        private int x_Collision_Adjustment;  //Holds the value used to adjust the position of item after a collision

        private static double maxXVel;   //Maximum velocity on the x axis

        public ItemPhysicsX(bool faceRight = true, double altMaxVel = 1)
        {
            maxXVel = altMaxVel;
            if (faceRight) {
                x_Vel = maxXVel;
            } else
            {
                x_Vel = -maxXVel;
            }
            x_Collision_Adjustment = NO_MOVE;
        }

        public int MovementAdjustmentX(GameTime gameTime, bool moving, bool faceRight)
        {
            if (moving)
            {
                return (int) x_Vel; //Item stay at the same speed in the X direction
            }
            else
            {
                return NO_MOVE;    //Item has stopped moving and is therefore not going to go anywhere
            }
        }

        public int CollisionAdjustmentX(bool faceRight, Rectangle rectangle)
        {
            if (faceRight)  //Item was moving right on collision, must move back to left
            {
                x_Collision_Adjustment = -rectangle.Width;
            }
            else  //Item was moving left on collision, must move back to right
            {
                x_Collision_Adjustment = rectangle.Width;
            }

            x_Vel = -x_Vel; //Reverses the direction in which the item is moving

            return x_Collision_Adjustment;
        }
    }
}
