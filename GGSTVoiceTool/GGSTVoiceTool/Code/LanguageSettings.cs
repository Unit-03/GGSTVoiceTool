using System.Collections.Generic;

namespace GGSTVoiceTool
{
    public class LanguageSettings
    {
        #region Properties

        public Character CharacterID { get; }

        public Language this[Language key] {
            get => languages[key];
            set => languages[key] = value;
        }

        #endregion

        #region Fields

        private Dictionary<Language, Language> languages;

        #endregion

        #region Constructor

        public LanguageSettings(Character charId)
        {
            CharacterID = charId;
            languages = new();

            foreach (Language langId in Data.Voice.Languages)
                languages.Add(langId, langId);
        }

        private LanguageSettings(Character charId, Dictionary<Language, Language> settings)
        {
            CharacterID = charId;
            languages = new(settings);
        }

        #endregion

        #region Methods

        public LanguageSettings Clone() => new LanguageSettings(CharacterID, languages);

        #endregion
    }
}
