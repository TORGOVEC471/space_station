using System.Linq;
using System.Text.RegularExpressions;

namespace Content.Server.Chat.Systems;

public sealed partial class ChatSystem
{
    private static readonly Dictionary<string, string> SlangReplace = new()
    {
        // Game
        { "хос", "гсб" },
        { "хоса", "гсб" },
        { "смо", "главрач" },
        // { "гв", "главрач" },
        { "се", "си" },
        { "хоп", "гп" },
        { "хопа", "гп" },
        // { "рд", "научрук" },
        // { "нр", "научрук" },
        { "вард", "смотритель" },
        // { "варден", "смотритель" },
        // { "вардена", "смотрителя" },
        { "геник", "генератор" },
        // { "кк", "красный код" },
        // { "ск", "синий код" },
        // { "зк", "зелёный код" },
        { "пда", "кпк" },
        { "корп", "корпоративный" },
        { "дэк", "детектив" },
        { "дэку", "детективу" },
        { "дэка", "детектива" },
        { "дек", "детектив" },
        { "деку", "детективу" },
        { "дека", "детектива" },
        { "мш", "имплант защиты разума" },
        { "трейтор", "предатель" },
        { "инж", "инженер" },
        { "инжи", "инженеры" },
        { "инжы", "инженеры" },
        { "инжу", "инженеру" },
        { "инжам", "инженерам" },
        { "инжинер", "инженер" },
        // { "яо", "ядерные оперативники" },
        { "нюк", "ядерный оперативник" },
        // { "нюкеры", "ядерные оперативники" },
        // { "нюкер", "ядерный оперативник" },
        { "нюкеровец", "ядерный оперативник" },
        { "нюкеров", "ядерных оперативников" },
        { "аирлок", "шлюз" },
        { "аирлоки", "шлюзы" },
        { "айрлок", "шлюз" },
        { "айрлоки", "шлюзы" },
        { "визард", "волшебник" },
        { "дизарм", "толчок" },
        { "синга", "сингулярность" },
        { "сингу", "сингулярность" },
        { "синги", "сингулярности" },
        { "сингой", "сингулярностью" },
        { "разгермой", "разгерметизацией" },
        { "бикардин", "бикаридин" },
        { "бика", "бикаридин" },
        { "бику", "бикаридин" },
        { "декса", "дексалин" },
        { "дексу", "дексалин" },
        // IC
        { "норм", "нормально" },
        { "хз", "не знаю" },
        { "синд", "синдикат" },
        // { "пон", "понятно" },
        // { "непон", "не понятно" },
        // { "нипон", "не понятно" },
        { "кста", "кстати" },
        { "кст", "кстати" },
        { "плз", "пожалуйста" },
        { "пж", "пожалуйста" },
        { "спс", "спасибо" },
        { "сяб", "спасибо" },
        { "прив", "привет" },
        { "ок", "окей" },
        { "чел", "мужик" },
        { "лан", "ладно" },
        { "збс", "заебись" },
        { "мб", "может быть" },
        { "оч", "очень" },
        { "омг", "боже мой" },
        { "нзч", "не за что" },
        { "пок", "пока" },
        { "бб", "пока" },
        { "пох", "плевать" },
        { "ясн", "ясно" },
        { "всм", "всмысле" },
        { "чзх", "что за херня?" },
        { "изи", "легко" },
        { "гг", "хорошо сработано" },
        { "пруф", "доказательство" },
        { "пруфани", "докажи" },
        { "пруфанул", "доказал" },
        { "имба", "круто" },
        { "разлокать", "разблокировать" },
        { "юзать", "использовать" },
        { "юзай", "используй" },
        { "юзнул", "использовал" },
        { "хилл", "лечение" },
        { "подхиль", "полечи" },
        { "хильни", "полечи" },
        { "хелп", "помоги" },
        { "хелпани", "помоги" },
        { "хелпанул", "помог" },
        { "рофл", "прикол" },
        { "рофлишь", "шутишь" },
        { "крч", "короче говоря" },
        { "шатл", "шаттл" },
        { "т.д", "так далее" },
        { "тд", "так далее" },
        { "сщ", "синий щит" },
        { "осщ", "синий щит" },
        { "пр", "привет" },
        { "бб", "пока" },
        { "кринж", "стыдоба" },
        { "пздц", "пиздец" },
        { "пон ", "понятно" },
        { "брух", "безумие" },
        // { "меда", "медицинского отсека" },
        // { "меду", "медицинскому отсеку" },
        // { "ио", "инженерный отдел" },
        // { "каргонец", "снабженец" },
        // { "каргонцы", "снабженцы" },
        // { "каргония", "повстанцы" },
        // { "км", "квартирмейстер" },
        // { "гсб", "глава службы безопасности" },
        // { "си", "старший инженер" },
        // { "бм", "бригмед" },
        // OOC
        { "афк", "ссд" },
        { "админ", "бог" },
        { "админы", "боги" },
        { "админов", "богов" },
        { "забанят", "покарают" },
        { "бан", "наказание" },
        { "пермач", "наказание" },
        { "перм", "наказание" },
        { "запермили", "наказание" },
        { "запермят", "накажут" },
        { "нонрп", "плохо" },
        { "нрп", "плохо" },
        { "ерп", "ужас" },
        { "рдм", "плохо" },
        { "дм", "плохо" },
        { "гриф", "плохо" },
        { "фрикил", "плохо" },
        { "фрикилл", "плохо" },
        { "лкм", "левая рука" },
        { "пкм", "правая рука" },
    };

    private string ReplaceWords(string message)
    {
        if (string.IsNullOrEmpty(message))
            return message;

        return Regex.Replace(message, "\\b(\\w+)\\b", match =>
        {
            bool isUpperCase = match.Value.All(Char.IsUpper);

            if (SlangReplace.TryGetValue(match.Value.ToLower(), out var replacement))
                return isUpperCase ? replacement.ToUpper() : replacement;
            return match.Value;
        });
    }
}
