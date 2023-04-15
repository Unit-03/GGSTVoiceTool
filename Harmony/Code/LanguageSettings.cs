using System.Collections.Generic;

namespace Harmony
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

        private readonly Dictionary<Language, Language> languages;

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

        public LanguageSettings Clone() => new(CharacterID, languages);

        #endregion
    }
}
