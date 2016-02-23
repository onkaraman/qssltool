using System.Windows.Media;

namespace QSSLTool.Gateways
{
    public class ColorSettings
    {
        public Color NeutralBG;
        public Color NeutralFG;
        public Color PositiveBG;
        public Color PositiveFG;
        public Color NegativeBG;
        public Color NegativeFG;

        public ColorSettings()
        {
            RestoreDefaults();
        }

        public void RestoreDefaults()
        {
            NeutralBG = Color.FromArgb(255, 255, 235, 156);
            NeutralFG = Color.FromArgb(255, 191, 149, 0);
            PositiveBG = Color.FromArgb(255, 198, 239, 206);
            PositiveFG = Color.FromArgb(255, 0, 97, 0);
            NegativeBG = Color.FromArgb(255, 255, 199, 206);
            NegativeFG = Color.FromArgb(255, 156, 0, 6);
        }
    }
}
