using QSSLTool.Patterns;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QSSLTool.Gateways
{
    public class Settings : LazyStatic<Settings>
    {
        private string _filenameColorSettings;
        private string _filenameAnalyzerSettings;
        private ColorSettings _colorSettings;
        private AnalyzerSettings _analyzerSettings;
        public ColorSettings ColorSettings { get { return _colorSettings; } }
        public AnalyzerSettings AnalyzerSettings { get { return _analyzerSettings; } }

        public Settings()
        {
            _filenameAnalyzerSettings = "AnalyzerSettings.bin";
            _filenameColorSettings = "ColorSettings.bin";

            if (!checkSavedSettings())
            {
                _colorSettings = new ColorSettings();
                _analyzerSettings = new AnalyzerSettings();
            }
        }

        private bool checkSavedSettings()
        {
            if (File.Exists(_filenameColorSettings)
                || File.Exists(_filenameAnalyzerSettings))
            {
                _colorSettings = (ColorSettings)unserialize(_filenameColorSettings);
                _analyzerSettings = (AnalyzerSettings)unserialize(_filenameAnalyzerSettings);
                return true;
            }
            return false;
        }

        public void Save()
        {
            serialize(_filenameColorSettings, _colorSettings);
            serialize(_filenameAnalyzerSettings, _analyzerSettings);
        }

        private void serialize(string filename, object obj)
        {
            IFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(filename,
                FileMode.Create, FileAccess.Write, FileShare.None);

            formatter.Serialize(stream, obj);
        }

        private object unserialize(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, 
                FileMode.Open, FileAccess.Read, FileShare.Read);
            var o = formatter.Deserialize(stream);
            stream.Close();
            return o;
        }
    }
}
