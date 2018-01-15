using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CommonData;

namespace SignalRServer
{
    public class GameHub : Hub
    {
        public static int WorldX = 1920;

        public static int WorldY = 1200;
        #region gamehubvariables
        public static Queue<PlayerData> RegisteredPlayers = new Queue<PlayerData>(new PlayerData[]
        {
            new PlayerData {GamerTag = "Player 1",imageName = "SAStankBase",turretName = "SAStankTurret",playerID = Guid.NewGuid().ToString() },
            new PlayerData {GamerTag = "Player 2",imageName = "RACtankBase",turretName = "RACtankTurret",playerID = Guid.NewGuid().ToString() },
            new PlayerData {GamerTag = "Player 3",imageName = "IJFtankBase",turretName = "IJFtankTurret",playerID = Guid.NewGuid().ToString() },
            new PlayerData {GamerTag = "Player 4",imageName = "CRUtankBase",turretName = "CRUtankTurret",playerID = Guid.NewGuid().ToString() }
        });

        public static List<PlayerData> Players = new List<PlayerData>();

        public static Stack<string> characters = new Stack<string>(
            new string[] { "Player 4", "Player 3", "Player 2", "Player 1" });

        public List<CollectableData> Collectibles = new List<CollectableData>();

        #endregion
        public void Hello()
        {
            Clients.All.hello();
        }

        public PlayerData Join()
        {
            // Check and if the charcters
            if (characters.Count > 0)
            {
                // pop name
                string character = characters.Pop();
                // if there is a registered player
                if (RegisteredPlayers.Count > 0)
                {
                    PlayerData newPlayer = RegisteredPlayers.Dequeue();
                    newPlayer.playerPosition = new Position
                    {
                        X = new Random().Next(WorldX - 128),

                        Y = new Random().Next(WorldY - 128)

                    };
                    // Tell all the other clients that this player has Joined
                    Clients.Others.Joined(newPlayer);
                    // Tell this client about all the other current 
                    Clients.Caller.CurrentPlayers(Players);
                    // Finaly add the new player on teh server
                    Players.Add(newPlayer);
                    return newPlayer;
                }


            }
            return null;
        }


        //Method Server-Side for removing player from server when he leaves the game.
        public void Left(string playerID)
        {
            //First Server searches through its players that exist on it, if found, removes it as he just left, and sends a message to other clients informing them about it.
            PlayerData found = Players.FirstOrDefault(p => p.playerID == playerID);

            if (found != null)
            {
                Players.Remove(found);
                characters.Push(found.GamerTag);
                Clients.Others.Left(found, Players);

            }
        }

        public bool IsGameReady()
        {
            if (Players.Count > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Moved(string playerID, Position newPosition)
        {
            // Update the collection with the new player position is the player exists
            PlayerData found = Players.FirstOrDefault(p => p.playerID == playerID);

            if (found != null)
            {
                // Update the server player position
                found.playerPosition = newPosition;
                // Tell all the other clients this player has moved
                Clients.Others.OtherMove(playerID, newPosition);
            }
        }

        public void Fired(ProjectileData projectile)
        {
            PlayerData found = Players.FirstOrDefault(p => p.playerID == projectile.ID);

            if (found != null)
            {

                //Fired
                Clients.Others.Fired(projectile);
            }
        }

        public void Hit(ProjectileData projectile)
        {
            PlayerData found = Players.FirstOrDefault(p => p.playerID == projectile.ID);

            if (found != null)
            {

                // Tell all the other clients this player has been Hit
                Clients.Others.HitReg(projectile);
            }
        }

        public void SendWorldSize()
        {
            //Returns World Coordinates to client which call for them.
            Clients.Caller.SendWorldSize(WorldX, WorldY);
        }

        public void Collected(string playerID, CollectableData c)
        {
            PlayerData found = Players.FirstOrDefault(p => p.playerID == playerID);

            if(found != null)
            {
                Clients.Others.CollectableCollected(playerID, c);
            }
        }

        public void CreateCollectibles()
        {
            int amount = Utility.NextRandom(10, 20);
            //Creates random amount of collectibles in the world coordinates
            for (int i = 0; i < amount; i++)
            {
                Collectibles.Add(
                        new CollectableData(
                        i,
                        new Position()
                        {
                            X = Utility.NextRandom(WorldX - 128),

                            Y = Utility.NextRandom(WorldY - 128)
                        },
                        Utility.NextRandom(10, 50) // Assigns a random points value between 10 and 50
                        ));
            }

            Clients.All.createCollectibles(Collectibles); //Tells clients to create them locally in each client
        }
    }
}