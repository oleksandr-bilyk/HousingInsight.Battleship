namespace HousingInsight.Battleship
{
    public sealed class ShipCell
    {
        public ShipCell(ShipCoordinate coordinate)
        {
            Coordinate = coordinate;
        }
        public ShipCoordinate Coordinate { get; }
        public bool Hit { get; set; }
    }
}
