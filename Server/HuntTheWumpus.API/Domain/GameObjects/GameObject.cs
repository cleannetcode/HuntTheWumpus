﻿namespace HuntTheWumpus.API.Domain.GameObjects
{
    public abstract class GameObject
    {
        public GameObject(uint x, uint y)
        {
            X = x;
            Y = x;
        }

        public uint X { get; set; }
        public uint Y { get; set; }
    }
}
