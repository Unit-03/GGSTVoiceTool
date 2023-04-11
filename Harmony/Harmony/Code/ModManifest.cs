using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Harmony
{
    public class ModManifest
    {
        #region Properties

        public Language  NarrationLanguage  { get; set; }
        public Character NarrationCharacter { get; set; }
        public bool SilencedNarration { get; set; }

        public Character[] Characters => voiceSettings.Keys.ToArray();

        public LanguageSettings this[Character key] {
            get => voiceSettings[key];
            set => voiceSettings[key] = value;
        }

        public Dictionary<Character, LanguageSettings> VoiceSettings {
            get {
                Dictionary<Character, LanguageSettings> copy = new(voiceSettings.Count);

                foreach (var pair in voiceSettings)
                    copy.Add(pair.Key, pair.Value.Clone());

                return copy;
            }
        }

        #endregion

        #region Fields

        private Dictionary<Character, LanguageSettings> voiceSettings;

        #endregion

        #region Constructor

        public ModManifest()
        {
            NarrationLanguage  = Language .DEF;
            NarrationCharacter = Character.DEF;

            voiceSettings = new();

            foreach (Character charID in Data.Voice.Characters)
                voiceSettings.Add(charID, new LanguageSettings(charID));
        }

        public ModManifest(Language narrLang, Character narrChar, bool silenced, Dictionary<Character, LanguageSettings> voices)
        {
            NarrationLanguage  = narrLang;
            NarrationCharacter = narrChar;
            SilencedNarration  = silenced;

            voiceSettings = new();

            foreach (var pair in voices)
                voiceSettings.Add(pair.Key, pair.Value.Clone());
        }

        public ModManifest(ModManifest clone) : this(clone.NarrationLanguage, clone.NarrationCharacter, clone.SilencedNarration, clone.voiceSettings)
        {
        }

        public ModManifest(string filePath)
        {
            Load(filePath);
        }

        #endregion

        #region Methods

        public void Load(string filePath)
        {
            Dictionary<Character, LanguageSettings> manifest = new();

            NarrationLanguage  = Language .DEF;
            NarrationCharacter = Character.DEF;

            foreach (Character charId in Data.Voice.Characters)
                manifest.Add(charId, new LanguageSettings(charId));

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; ++i)
                {
                    string[] parts = lines[i].Split('=');

                    if (parts.Length < 2)
                        continue;

                    string key = parts[0].Trim().ToUpper();

                    if (key == "NARR")
                    {
                        string[] values = parts[1].Split('_');
                        string langId = values[0].Trim().ToUpper();
                        string charId = values[1].Trim().ToUpper();
                        string silent = values[2].Trim().ToLower();

                        NarrationLanguage  = Enum.Parse<Language> (langId);
                        NarrationCharacter = Enum.Parse<Character>(charId);

                        if (bool.TryParse(silent, out bool silenced))
                            SilencedNarration = silenced;
                        else
                            SilencedNarration = true;
                    }
                    else
                    {
                        string[] keys = parts[0].Split('_');
                        string charId = keys [0].Trim().ToUpper();
                        string langId = keys [1].Trim().ToUpper();
                        string useId  = parts[1].Trim().ToUpper();

						Character chara = Enum.Parse<Character>(charId);
						Language  lang  = Enum.Parse<Language> (langId);
						Language  use   = Enum.Parse<Language> (useId );

                        if (manifest.ContainsKey(chara))
							manifest[chara][lang] = use;
                    }
                }
            }

            voiceSettings = manifest;
        }

        public void Save(string filePath)
        {
            string manifestDir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(manifestDir))
                Directory.CreateDirectory(manifestDir);

            using StreamWriter writer = File.CreateText(filePath);

            foreach (Character charId in voiceSettings.Keys)
            {
                foreach (Language langId in Data.Voice.Languages)
                {
                    // No point writing languages that haven't been changed
                    if (langId == voiceSettings[charId][langId])
                        continue;

                    writer.WriteLine($"{charId}_{langId}={voiceSettings[charId][langId]}");
                }
            }

            if (NarrationLanguage != Language.DEF)
                writer.WriteLine($"NARR={NarrationLanguage}_{NarrationCharacter}_{SilencedNarration}");
        }

        #endregion
    }
}
