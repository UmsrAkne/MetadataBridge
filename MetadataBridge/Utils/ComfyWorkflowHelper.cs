using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MetadataBridge.Utils
{
    public static class ComfyWorkflowHelper
    {
        /// <summary>
        /// 指定されたJSONファイル内の "title": "Seed" を持つノードの "randomize" を "fixed" に書き換えて上書き保存します。
        /// </summary>
        /// <param name="filePath">対象のJSONファイルパス</param>
        /// <returns>変更されて保存された場合は true、対象が見つからなかった場合は false</returns>
        public static bool FixSeedRandomize(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("ファイルが見つかりません。", filePath);
            }

            var jsonText = File.ReadAllText(filePath);
            var jObj = JObject.Parse(jsonText);

            // "title" が "Seed" のオブジェクトを検索
            var seedNode = jObj.Descendants()
                .OfType<JObject>()
                .FirstOrDefault(o => (string)o["title"] == "Seed");

            if (seedNode?["widgets_values"] is JArray widgetsArray)
            {
                var isUpdated = false;

                for (var i = 0; i < widgetsArray.Count; i++)
                {
                    if ((string)widgetsArray[i] == "randomize")
                    {
                        widgetsArray[i] = "fixed";
                        isUpdated = true;
                    }
                }

                // 変更があった場合のみファイルに上書き保存
                if (isUpdated)
                {
                    File.WriteAllText(filePath, jObj.ToString(Formatting.Indented));
                    return true;
                }
            }

            return false;
        }
    }
}