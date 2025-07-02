namespace API.SignalR
{
    public static class SignalRGroups
    {
        public static string BusGroup(int busId) => $"bus_{busId}";
    }
}
