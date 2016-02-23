using QSSLTool.Compacts;
using System;
using System.Windows.Media;

namespace QSSLTool.Gateways
{
    [Serializable]
    public class ColorSettings
    {
        public ColorHolder NeutralBG;
        public ColorHolder NeutralFG;
        public ColorHolder PositiveBG;
        public ColorHolder PositiveFG;
        public ColorHolder NegativeBG;
        public ColorHolder NegativeFG;

        public ColorSettings()
        {
            RestoreDefaults();
        }

        public void RestoreDefaults()
        {
            NeutralBG = new ColorHolder(255, 255, 235, 156);
            NeutralFG = new ColorHolder(255, 191, 149, 0);
            PositiveBG = new ColorHolder(255, 198, 239, 206);
            PositiveFG = new ColorHolder(255, 0, 97, 0);
            NegativeBG = new ColorHolder(255, 255, 199, 206);
            NegativeFG = new ColorHolder(255, 156, 0, 6);
        }
    }
}
