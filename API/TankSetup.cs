namespace Tankathon.API
{
    public class TankSetup : ITankSetup
    {
        public string name { get; set; } = "MyTank";
        public string primaryColor { get; set; } = "#000000";
        public string secondaryColor { get; set; } = "#ffffff";
    }
}
