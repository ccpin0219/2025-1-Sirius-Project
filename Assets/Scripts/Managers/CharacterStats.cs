[System.Serializable]
public class CharacterStats
{
    public string characterName;
    public int maxHP;
    public int speed;
    public int attackResource;
    public int defenseResource;

    public CharacterStats(string name, int hp, int spd, int atkRes, int defRes)
    {
        characterName = name;
        maxHP = hp;
        speed = spd;
        attackResource = atkRes;
        defenseResource = defRes;
    }
}
