using System;

namespace QSSLTool.Compacts
{
    [Serializable]
    public class ColorHolder
    {
        public byte A, R, G, B;
        
        public ColorHolder(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
    }
}
