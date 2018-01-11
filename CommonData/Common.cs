using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonData
{
    public class Position
    {
        public int X;
        public int Y;
        public float angle;
        public float TurretAngle;
        public bool HasFired;
    }

    public class PlayerData
    {
        public string playerID;
        public string imageName = string.Empty;
        public string GamerTag = string.Empty;
        public string playerName = string.Empty;
        public int Score;
        public int Wins;
        public Position playerPosition;
        public string Password;
    }
}
