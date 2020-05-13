using SimpleJSON;

namespace ChainFixer
{
    public class Encoder
    {
        public static string GetConfig(Config config)
        {
            var configJSON = new JSONObject();

            configJSON["handColor"] = config.handColor;
            configJSON["rightR"] = config.rightR;
            configJSON["rightG"] = config.rightG;
            configJSON["rightB"] = config.rightB;
            configJSON["leftR"] = config.leftR;
            configJSON["leftG"] = config.leftG;
            configJSON["leftB"] = config.leftB;


            return configJSON.ToString(4);
        }

        public static void SetConfig(Config config, string data)
        {
            var configJSON = JSON.Parse(data);

            config.handColor = configJSON["handColor"];
            config.rightR = configJSON["rightR"];
            config.rightG = configJSON["rightG"];
            config.rightB = configJSON["rightB"];
            config.leftR = configJSON["leftR"];
            config.leftG = configJSON["leftG"];
            config.leftB = configJSON["leftB"];
        }
    }
}
