using UnityEngine;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject warriorPrefab;
    public GameObject magePrefab;
    public GameObject wolfPrefab;

    public GameObject warriorObj { get; private set; }
    public GameObject mageObj { get; private set; }
    public GameObject wolf1Obj { get; private set; }
    public GameObject wolf2Obj { get; private set; }

    public void SpawnCharacters()
    {
        // 전사 생성
        warriorObj = Instantiate(warriorPrefab, new Vector3(-6f, -0.7f, 0), Quaternion.identity);
        var warrior = warriorObj.GetComponent<Character>();
        warrior.stats = new CharacterStats("전사", 90, 12, 8, 10);
        warrior.skills = new List<Skill>
        {
            LoadSkill("타격"),
            LoadSkill("부수기"),
            LoadSkill("방패 올리기"),
            LoadSkill("보호 진영")
        };

        // 마법사 생성
        mageObj = Instantiate(magePrefab, new Vector3(-8f, -0.7f, 0), Quaternion.identity);
        var mage = mageObj.GetComponent<Character>();
        mage.stats = new CharacterStats("마법사", 75, 15, 10, 5);
        mage.skills = new List<Skill>
        {
            LoadSkill("마법 화살"),
            LoadSkill("화염탄"),
            LoadSkill("물의 방패"),
            LoadSkill("화염구")
        };

        // 늑대 1
        wolf1Obj = Instantiate(wolfPrefab, new Vector3(6f, -0.7f, 0), Quaternion.identity);
        var wolf1 = wolf1Obj.GetComponent<Enemy>();
        wolf1.Init(new CharacterStats("늑대1", 50, 13, 0, 0));
        GameManager.Instance.enemies.Add(wolf1);

        // 늑대 2
        wolf2Obj = Instantiate(wolfPrefab, new Vector3(8f, -0.7f, 0), Quaternion.identity);
        var wolf2 = wolf2Obj.GetComponent<Enemy>();
        wolf2.Init(new CharacterStats("늑대2", 50, 14, 0, 0));
        GameManager.Instance.enemies.Add(wolf2);
    }

    private Skill LoadSkill(string name)
    {
        var skill = Resources.Load<Skill>($"Skills/{name}");
        if (skill == null)
            Debug.LogError($"Skill '{name}' not found in Resources/Skills/");
        return skill;
    }
}
