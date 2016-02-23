using QSSLTool.Patterns;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

        public void Save()
        {
            serialize("ColorSettings.bin", _colorSettings);
            serialize("AnalyzerSettings.bin", _analyzerSettings);
        }

        private void serialize(string filename, object obj)
        {
            IFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(filename,
                FileMode.Create, FileAccess.Write, FileShare.None);

            formatter.Serialize(stream, obj);
        }
    }
}
