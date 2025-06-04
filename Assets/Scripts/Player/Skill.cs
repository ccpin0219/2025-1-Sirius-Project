public class Skill
{
    public string Name;   // 스킬 이름
    public int Damage;    // 데미지 값 (음수면 회복)

    // 스킬 생성자
    public Skill(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}