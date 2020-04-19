namespace Abaddon
{
    public static class Conversions
    {
        public static int AsHex(char c)
        {
            return int.Parse($"{c}", System.Globalization.NumberStyles.HexNumber);
        }
    }
}
