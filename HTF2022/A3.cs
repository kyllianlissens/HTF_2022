using System.Net.Http.Json;

namespace HTF2022;

public class Response
{
    public List<int>? Doors { get; set; }
    public int RoomNr { get; set; }
    public bool Finished { get; set; }


    public override string ToString()
    {
        return Doors != null
            ? $"{nameof(Doors)}: {Doors.Count}, {nameof(RoomNr)}: {RoomNr}, {nameof(Finished)}: {Finished}"
            : $"{nameof(RoomNr)}: {RoomNr}, {nameof(Finished)}: {Finished}";
    }
}

public class RoomEntity
{
    public int Room { get; set; }
    public List<Door> Doors { get; set; } = null!;
}

public class Door
{
    public int DoorNr { get; set; }
}

internal static class A3
{
    private static readonly string TestUrl = "/api/path/1/hard/Sample";
    private static readonly string ProductionUrl = "/api/path/1/hard/Puzzle";

    private static readonly HTTPInstance ClientInstance = new();

    internal static void LocalExecution()
    {
        Console.WriteLine("-Local Execution: \n");
    }

    internal static async Task TestExecution()
    {
        Console.WriteLine("-Test Execution: \n");


        var initialResponse = await ClientInstance.Client.GetFromJsonAsync<Response>(TestUrl);

        var roomList = new List<RoomEntity>();

        var room = new RoomEntity
        {
            Room = initialResponse!.RoomNr,
            Doors = initialResponse.Doors!.Select(x => new Door { DoorNr = x }).ToList()
        };
        
        roomList.Add(room);

        var continueLoop = true;
        var currentRoomNr = 1;

        do
        {
            var currentRoom = roomList.Find(x => x.Room == currentRoomNr);

            var possibleDoor = currentRoom!.Doors.First();

            var postResponse = await ClientInstance.Client.PostAsJsonAsync(TestUrl, possibleDoor.DoorNr).Result
                .Content.ReadFromJsonAsync<Response>();
           
            if (postResponse!.Finished)
            {
                continueLoop = false;
            }
            else if (postResponse.RoomNr == 1)
            {
                currentRoom.Doors.Remove(possibleDoor);
                currentRoomNr = 1;
            }
            else
            {
                var newRoom = new RoomEntity
                {
                    Room = postResponse.RoomNr,
                    Doors = postResponse.Doors!.Select(x => new Door { DoorNr = x }).ToList()
                };

                roomList.Add(newRoom);
                currentRoomNr = postResponse.RoomNr;
            }
            
        } while (continueLoop);
    }


    internal static async Task ProductionExecution()
    {
        Console.WriteLine("-Production Execution: \n");
        var initialResponse = await ClientInstance.Client.GetFromJsonAsync<Response>(ProductionUrl);

        var roomList = new List<RoomEntity>();


        var room = new RoomEntity
        {
            Room = initialResponse.RoomNr,
            Doors = initialResponse.Doors.Select(x => new Door { DoorNr = x }).ToList()
        };

        roomList.Add(room);


        var continueLoop = true;

        var currentRoomNr = 1;
        do
        {
            Console.WriteLine("Current room number: " + currentRoomNr);
            var currentRoom = roomList.Find(x => x.Room == currentRoomNr);


            //for in currentRoom.Doors
            var possibleDoor = currentRoom.Doors.First();


            if (possibleDoor == null)
            {
                continueLoop = false;
                Console.WriteLine("OOPSIEE");
            }
            else
            {
                Console.WriteLine("Possible door: " + possibleDoor.DoorNr);
                var postResponse = await ClientInstance.Client.PostAsJsonAsync(ProductionUrl, possibleDoor.DoorNr)
                    .Result.Content.ReadFromJsonAsync<Response>();
                Console.WriteLine($"Post Response: {postResponse}");

                if (postResponse.Finished)
                {
                    continueLoop = false;
                }
                else if (postResponse.RoomNr == 1)
                {
                    currentRoom.Doors.Remove(possibleDoor);
                    currentRoomNr = 1;
                }
                else if (postResponse.RoomNr == currentRoom.Room)
                {
                    currentRoom.Doors.Remove(possibleDoor);
                }

                else
                {
                    var newRoom = new RoomEntity
                    {
                        Room = postResponse.RoomNr,
                        Doors = postResponse.Doors.Select(x => new Door { DoorNr = x }).ToList()
                    };

                    roomList.Add(newRoom);
                    currentRoomNr = postResponse.RoomNr;
                }
            }

            Console.ReadLine();
        } while (continueLoop);
    }
}