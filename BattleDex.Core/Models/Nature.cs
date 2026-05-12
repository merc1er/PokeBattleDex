#nullable enable
namespace BattleDex.Core.Models;

public class Nature
{
    public required string English { get; init; }
    public required string French { get; init; }
    public required string German { get; init; }
    public required string Japanese { get; init; }
    public string? IncreasedStat { get; init; }
    public string? DecreasedStat { get; init; }
    public bool IsNeutral => IncreasedStat is null;

    // Listed in internal Pokémon game order (IDs 0–24).
    // Translations sourced from https://www.pokepedia.fr/Nature
    public static readonly IReadOnlyList<Nature> All = new Nature[]
    {
        new() { English = "Hardy",   French = "Hardi",    German = "Robust",   Japanese = "がんばりや" },
        new() { English = "Lonely",  French = "Solo",     German = "Solo",     Japanese = "さみしがり", IncreasedStat = "Atk",    DecreasedStat = "Def"    },
        new() { English = "Brave",   French = "Brave",    German = "Mutig",    Japanese = "ゆうかん",  IncreasedStat = "Atk",    DecreasedStat = "Spe"    },
        new() { English = "Adamant", French = "Rigide",   German = "Hart",     Japanese = "いじっぱり", IncreasedStat = "Atk",    DecreasedStat = "Sp.Atk" },
        new() { English = "Naughty", French = "Mauvais",  German = "Frech",    Japanese = "やんちゃ",  IncreasedStat = "Atk",    DecreasedStat = "Sp.Def" },
        new() { English = "Bold",    French = "Assuré",   German = "Kühn",     Japanese = "ずぶとい",  IncreasedStat = "Def",    DecreasedStat = "Atk"    },
        new() { English = "Docile",  French = "Docile",   German = "Sanft",    Japanese = "すなお"   },
        new() { English = "Relaxed", French = "Relax",    German = "Locker",   Japanese = "のんき",   IncreasedStat = "Def",    DecreasedStat = "Spe"    },
        new() { English = "Impish",  French = "Malin",    German = "Pfiffig",  Japanese = "わんぱく",  IncreasedStat = "Def",    DecreasedStat = "Sp.Atk" },
        new() { English = "Lax",     French = "Lâche",    German = "Lasch",    Japanese = "のうてんき", IncreasedStat = "Def",    DecreasedStat = "Sp.Def" },
        new() { English = "Timid",   French = "Timide",   German = "Scheu",    Japanese = "おくびょう", IncreasedStat = "Spe",    DecreasedStat = "Atk"    },
        new() { English = "Hasty",   French = "Pressé",   German = "Hastig",   Japanese = "せっかち",  IncreasedStat = "Spe",    DecreasedStat = "Def"    },
        new() { English = "Serious", French = "Sérieux",  German = "Ernst",    Japanese = "まじめ"   },
        new() { English = "Jolly",   French = "Jovial",   German = "Froh",     Japanese = "ようき",   IncreasedStat = "Spe",    DecreasedStat = "Sp.Atk" },
        new() { English = "Naive",   French = "Naïf",     German = "Naiv",     Japanese = "むじゃき",  IncreasedStat = "Spe",    DecreasedStat = "Sp.Def" },
        new() { English = "Modest",  French = "Modeste",  German = "Mäßig",    Japanese = "ひかえめ",  IncreasedStat = "Sp.Atk", DecreasedStat = "Atk"    },
        new() { English = "Mild",    French = "Doux",     German = "Mild",     Japanese = "おっとり",  IncreasedStat = "Sp.Atk", DecreasedStat = "Def"    },
        new() { English = "Quiet",   French = "Discret",  German = "Ruhig",    Japanese = "れいせい",  IncreasedStat = "Sp.Atk", DecreasedStat = "Spe"    },
        new() { English = "Bashful", French = "Pudique",  German = "Zaghaft",  Japanese = "てれや"   },
        new() { English = "Rash",    French = "Foufou",   German = "Hitzig",   Japanese = "うっかりや", IncreasedStat = "Sp.Atk", DecreasedStat = "Sp.Def" },
        new() { English = "Calm",    French = "Calme",    German = "Still",    Japanese = "おだやか",  IncreasedStat = "Sp.Def", DecreasedStat = "Atk"    },
        new() { English = "Gentle",  French = "Gentil",   German = "Zart",     Japanese = "おとなしい", IncreasedStat = "Sp.Def", DecreasedStat = "Def"    },
        new() { English = "Sassy",   French = "Malpoli",  German = "Forsch",   Japanese = "なまいき",  IncreasedStat = "Sp.Def", DecreasedStat = "Spe"    },
        new() { English = "Careful", French = "Prudent",  German = "Sacht",    Japanese = "しんちょう", IncreasedStat = "Sp.Def", DecreasedStat = "Sp.Atk" },
        new() { English = "Quirky",  French = "Bizarre",  German = "Kauzig",   Japanese = "きまぐれ"  },
    };
}
