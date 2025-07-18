using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public class SdoaqScriptReader
    {
        public bool Loaded { get; private set; } = false;
        public string FileName { get; private set; }

        private string[] _contents = null;

        public SdoaqScriptReader()
        {

        }

        public SdoaqScriptReader(string fileName)
        {
            LoadScript(fileName);
        }

        public bool LoadScript(string fileName)
        {
            FileName = fileName;
            Loaded = false;
            if (File.Exists(fileName) == false)
            {
                return false;
            }

            try
            {
                _contents = File.ReadAllLines(fileName);
                Loaded = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValueFromScript<T>(string token, T defaultVal, out T val) where T : struct        
        {
            val = defaultVal;
            if (Loaded == false)
            {
                return false;
            }

            token = Regex.Replace(token, @"\s+", "").ToUpper();

            foreach (var line in _contents)
            {
                string data = Regex.Replace(line, @"\s+", "").ToUpper();

                if (data.StartsWith("#"))
                {
                    continue;
                }

                var parts = data.Split('=');

                if (parts.Length < 2 || parts[0] != token)
                {
                    continue;
                }

                if (ParseHelper<T>.TryParse(parts[1], out val))
                {
                    return true;
                }
                else
                {
                    val = defaultVal;
                    return false;
                }
            }
            return false;
        }
    }
}
