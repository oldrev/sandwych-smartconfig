namespace Sandwych.SmartConfig.Util
{
    public static class BytesHelper
    {
        public static ushort CombineUshort(byte high4bit, byte low4bit) =>
            (ushort)(((high4bit << 4) & 0xF0) | (low4bit & 0x0F));

        public static (byte high, byte low) Bisect(this byte b) =>
            ((byte)(b >> 4), (byte)(b & 0x0F));
    }
}
