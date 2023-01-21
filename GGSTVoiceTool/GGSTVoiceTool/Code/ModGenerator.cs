using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GGSTVoiceTool
{
    public static class ModGenerator
    {
        #region Methods

        public static Task GenerateNarrationMod(Language langId, Character charId, bool silent)
        {
            return GenerateNarration(langId, charId, silent);
        }

        private static async Task GenerateNarration(Language langId, Character charId, bool silent)
        {
            Paths.Properties.Language  = langId;
            Paths.Properties.Character = charId;

            // Make sure the temp folder is clear before generating or it could break things
            if (Directory.Exists(Paths.Generator.Temp))
                Directory.Delete(Paths.Generator.Temp, true);

            using Stream stream = await DownloadManager.DownloadAsset(Paths.Assets.Narration.URL, Paths.Assets.Narration.Cache);
            using ZipArchive zip = new ZipArchive(stream);

            Directory.CreateDirectory(Paths.Generator.Narration.Unpack);

            // Unpack the asset bundle into a temporary directory
            await Task.Run(() => zip.ExtractToDirectory(Paths.Generator.Narration.Unpack));

            // Either rename or delete the silent audio files depending on the user's choice
            await Task.Run(() => ProcessEmptyAudio(Paths.Generator.Narration.Unpack, silent));

            // Generate the mod with UnrealPak
            await Task.Run(() => CreatePak(Paths.Generator.Narration.Unpack, Paths.Generator.Narration.Pack));

            // Move the generated mod to the mods folder and copy the signature file over
            string modRoot = $"{Paths.Config.Install}/Narration";

            if (!Directory.Exists(modRoot))
                Directory.CreateDirectory(modRoot);

            string modFileName = Path.Combine(modRoot, $"{langId}_{charId}");

            File.Move(Paths.Generator.Narration.Pack, $"{modFileName}.pak", true);
            File.Copy(Paths.Config.GameSig,           $"{modFileName}.sig", true);

            // Clear out the temp files
            Directory.Delete(Paths.Generator.Temp, true);
        }

        private static void ProcessEmptyAudio(string root, bool silence)
        {
            string[] files = Directory.GetFiles(root, "*_empty.*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; ++i)
            {
                if (silence)
                    File.Move(files[i], files[i].Replace("_empty", ""), true);
                else
                    File.Delete(files[i]);
            }
        }

        public static Task GenerateVoiceMod(PatchInfo patch, Action iterationCallback = null)
        {
            if (Settings.BundleMods == true)
                return GenerateBundled(patch, iterationCallback);
            else
                return GenerateIndividual(patch, iterationCallback);
        }

        private static async Task GenerateIndividual(PatchInfo patch, Action iterationCallback)
        {
            // Make sure the temp folder is clear before generating or it could break things
            if (Directory.Exists(Paths.Generator.Temp))
                Directory.Delete(Paths.Generator.Temp, true);

            foreach (var langPatch in patch)
            {
                Paths.Properties.Character = langPatch.Character;
                Paths.Properties.Language  = langPatch.UseLang;

                using Stream stream = await DownloadManager.DownloadAsset(Paths.Assets.Voice.URL, Paths.Assets.Voice.Cache);
                using ZipArchive zip = new ZipArchive(stream);

                Directory.CreateDirectory(Paths.Generator.Voice.Unpack);

                // Unpack the asset bundle into a temporary directory
                await Task.Run(() => zip.ExtractToDirectory(Paths.Generator.Voice.Unpack));

                // Rename the relevant directories to the new language
                await Task.Run(() => RecursiveRename(Paths.Generator.Voice.Unpack, langPatch.UseLang.ToString(), langPatch.OverLang.ToString()));

                // Generate the mod with UnrealPak
                await Task.Run(() => CreatePak(Paths.Generator.Voice.Unpack, Paths.Generator.Voice.Pack));

                // Move the generated mod to the mods folder and copy the signature file over
                string modRoot = $"{Paths.Config.Install}/{langPatch.Character}";

                if (!Directory.Exists(modRoot))
                    Directory.CreateDirectory(modRoot);

                string modFileName = Path.Combine(modRoot, $"{langPatch.UseLang} over {langPatch.OverLang}");

                File.Move(Paths.Generator.Voice.Pack, $"{modFileName}.pak", true);
                File.Copy(Paths.Config.GameSig,       $"{modFileName}.sig", true);

                // Invoke the callback so we can do updates inbetween iterations
                iterationCallback?.Invoke();
            }

            // Clear out the temp files
            Directory.Delete(Paths.Generator.Temp, true);
        }

        private static async Task GenerateBundled(PatchInfo patch, Action iterationCallback)
        {
            // Make sure the temp folder is clear before generating or it could break things
            if (Directory.Exists(Paths.Generator.Temp))
                Directory.Delete(Paths.Generator.Temp, true);

            // If we're doing a bundled mod then any already installed mods should be deleted first
            if (Directory.Exists(Paths.Config.Install))
                Directory.Delete(Paths.Config.Install, true);

            foreach (var langPatch in patch)
            {
                Paths.Properties.Character = langPatch.Character;
                Paths.Properties.Language  = langPatch.UseLang;

                using Stream stream = await DownloadManager.DownloadAsset(Paths.Assets.Voice.URL, Paths.Assets.Voice.Cache);
                using ZipArchive zip = new ZipArchive(stream);

                Directory.CreateDirectory(Paths.Generator.Voice.Unpack);

                // Unpack the asset bundle into a temporary directory
                await Task.Run(() => zip.ExtractToDirectory(Paths.Generator.Voice.Unpack));

                // Rename the relevant directories to the new language
                await Task.Run(() => RecursiveRename(Paths.Generator.Voice.Unpack, langPatch.UseLang.ToString(), langPatch.OverLang.ToString()));

                // Move the renamed assets into the bundle directory
                await Task.Run(() => MoveSubDirectories(Paths.Generator.Voice.Unpack, Paths.Generator.Bundle.Unpack));

                // Invoke the callback so we can do updates inbetween iterations
                iterationCallback?.Invoke();
            }

            // Generate the mod with UnrealPak
            await Task.Run(() => CreatePak(Paths.Generator.Bundle.Unpack, Paths.Generator.Bundle.Pack));

            // Move the generated mod to the mods folder and copy the signature file over
            string modRoot = $"{Paths.Config.Install}";

            if (!Directory.Exists(modRoot))
                Directory.CreateDirectory(modRoot);

            string modFileName = Path.Combine(modRoot, "Bundle");

            File.Move(Paths.Generator.Bundle.Pack, $"{modFileName}.pak", true);
            File.Copy(Paths.Config.GameSig,        $"{modFileName}.sig", true);

            // Clear out the temp files
            Directory.Delete(Paths.Generator.Temp, true);
        }

        private static void RecursiveRename(string root, string oldName, string newName)
        {
            string[] subDirs = Directory.GetDirectories(root);

            for (int i = 0; i < subDirs.Length; ++i)
            {
                string subDirName = Path.GetRelativePath(root, subDirs[i]);

                if (subDirName == oldName)
                {
                    string newDir = Path.Combine(root, newName);
                    Directory.Move(subDirs[i], newDir);

                    subDirs[i] = newDir;
                }
            }

            for (int i = 0; i < subDirs.Length; ++i)
                RecursiveRename(subDirs[i], oldName, newName);
        }

        private static void MoveSubDirectories(string root, string destination)
        {
            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            string[] files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; ++i)
            {
                string relativePath = Path.GetRelativePath(root, files[i]);
                string newPath      = Path.Combine(destination, relativePath);
                string fileDir      = Path.GetDirectoryName(newPath);

                if (!Directory.Exists(fileDir))
                    Directory.CreateDirectory(fileDir);

                File.Move(files[i], newPath);
            }
        }

        private static void CreatePak(string root, string destination)
        {
            string filelist = $"\"{root}/*.*\" \"../../../*.*\""; // I'll be honest I don't really know what this is but it works :>
            File.WriteAllText(Paths.UnrealPak.Filelist, filelist);

            string arguments = $"\"{destination}\" -create={Paths.UnrealPak.Filelist} -compress";

            ProcessStartInfo startInfo = new ProcessStartInfo() {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = Paths.UnrealPak.Exe,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            Process unrealPak = Process.Start(startInfo);
            unrealPak.WaitForExit();
        }

        #endregion
    }
}
