using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using MetadataExtractor;

namespace MetadataBridge.Utils
{
    public static class JsonExtractor
    {
        private readonly static JsonSerializerOptions JsonOptions = new ()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        public static FileInfo ExportWorkflow(string pngPath, params string[] targetKeys)
        {
            // 出力したファイルへの参照を保持する FileInfo を返す
            // キー指定がない場合は "workflow" のみ対象にする
            if (targetKeys == null || targetKeys.Length == 0)
            {
                targetKeys = ["workflow",];
            }

            var directories = ImageMetadataReader.ReadMetadata(pngPath);
            var baseDir = Path.GetDirectoryName(pngPath) ?? "";
            var fileName = Path.GetFileNameWithoutExtension(pngPath);

            foreach (var dir in directories)
            {
                foreach (var tag in dir.Tags)
                {
                    var desc = tag.Description ?? "";
                    if (tag.Name.Contains("Textual", StringComparison.OrdinalIgnoreCase)
                        || tag.Name.Contains("Text", StringComparison.OrdinalIgnoreCase))
                    {
                        var colonIdx = desc.IndexOf(':');
                        if (colonIdx <= 0)
                        {
                            continue;
                        }

                        var key = desc[..colonIdx].Trim();

                        // 指定されたキー以外はスキップ（大文字小文字は無視）
                        if (!targetKeys.Any(t => string.Equals(t, key, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }

                        var text = desc[(colonIdx + 1)..].Trim();
                        var outPath = Path.Combine(baseDir, $"{fileName}_{key}.json");

                        try
                        {
                            using var doc = JsonDocument.Parse(text);
                            File.WriteAllText(outPath, JsonSerializer.Serialize(doc.RootElement, JsonOptions));
                            return new FileInfo(outPath);
                        }
                        catch (JsonException)
                        {
                            File.WriteAllText(Path.ChangeExtension(outPath, ".txt"), text);
                        }
                    }
                }
            }

            return null;
        }
    }
}