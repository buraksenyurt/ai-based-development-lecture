namespace Gamepedia.Domain;

/*
    Enum mı Value Object mi?

    Genre basit ve sabit bir küme olduğu için enum kullanımı uygundur: az değer, sık değişmez, davranışı(behavior) yok.
    Eğer tür adı, alt tür veya tür açıklaması gibi zengin veriler içermesi gerekirse ya da iş kuralları 
    barındıracaksa, bir Value Object (Genre struct/record) daha uygun olacaktır
*/
public enum Genre
{
    Action,
    Adventure,
    RolePlaying,
    Simulation,
    Strategy,
    Sports,
    Puzzle,
    Horror,
    Racing,
    Fighting
}
