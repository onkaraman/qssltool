using QSSLTool.Patterns;

namespace QSSLTool.Gateways
{
    public class Settings : LazyStatic<Settings>
    {
        private ColorSettings _colorSettings;
        private AnalyzerSettings _analyzerSettings;
        public ColorSettings ColorSettings { get { return _colorSettings; } }
        public AnalyzerSettings AnalyzerSettings { get { return _analyzerSettings; } }

        public Settings()
        {
            _colorSettings = new ColorSettings();
            _analyzerSettings = new AnalyzerSettings();
        }
    }
}
