using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MetadataBridge.Utils
{
    public static class ComfyWorkflowHelper
    {
        // ==========================================
        // 1. 個別の業務・用途ごとの便利メソッド群
        // ==========================================

        /// <summary>
        /// 指定されたJSONファイル内の "title": "Seed" を持つノードの "randomize" を "fixed" に書き換えます。
        /// </summary>
        public static bool FixSeedRandomize(string filePath)
        {
            return ReplaceWidgetValue(filePath, "Seed", "randomize", "fixed");
        }

        /// <summary>
        /// 【汎用】特定の title を持つノードの widgets_values 内の文字列を置換します。
        /// （例: KSampler の steps や cfg、sampler_name などの変更にも流用可能）
        /// </summary>
        public static bool ReplaceWidgetValue(string filePath, string nodeTitle, string oldValue, string newValue)
        {
            return UpdateNode(filePath,
                node => (string)node["title"] == nodeTitle,
                node =>
                {
                    if (node["widgets_values"] is not JArray widgetsArray)
                    {
                        return false;
                    }

                    var updated = false;
                    for (var i = 0; i < widgetsArray.Count; i++)
                    {
                        if ((string)widgetsArray[i] == oldValue)
                        {
                            widgetsArray[i] = newValue;
                            updated = true;
                        }
                    }

                    return updated;
                });
        }

        // ==========================================
        // 2. コアとなる汎用処理（ファイルの読み書き・検索・更新）
        // ==========================================

        /// <summary>
        /// 条件に一致するJObjectノードを検索し、指定された更新処理を実行して保存します。
        /// </summary>
        /// <param name="filePath">対象のJSONファイルパス</param>
        /// <param name="predicate">ノードの抽出条件 (例: o => (string)o["title"] == "Seed")</param>
        /// <param name="updater">ノードに対する更新処理 (変更を加えたら true を返すアクション)</param>
        /// <returns>変更されて保存された場合は true</returns>
        public static bool UpdateNode(string filePath, Func<JObject, bool> predicate, Func<JObject, bool> updater)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("ファイルが見つかりません。", filePath);
            }

            var jsonText = File.ReadAllText(filePath);
            var jObj = JObject.Parse(jsonText);

            // 条件に合致するノードをすべて抽出（複数ヒットにも対応可能）
            var targetNodes = jObj.Descendants()
                .OfType<JObject>()
                .Where(predicate)
                .ToList();

            if (targetNodes.Count == 0)
            {
                return false;
            }

            var anyUpdated = false;
            foreach (var node in targetNodes)
            {
                if (updater(node))
                {
                    anyUpdated = true;
                }
            }

            // 実際に中身が更新された場合のみファイルに書き出す
            if (anyUpdated)
            {
                File.WriteAllText(filePath, jObj.ToString(Formatting.Indented));
                return true;
            }

            return false;
        }
    }
}