using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Story
{
    public string text;
    public bool delete;

    public void SetStory(string text, bool delete)
    {
        this.text = text;
        this.delete = delete;
    }
}

public class StoryUI : MonoBehaviour
{
    public DataManager dataManager;
    private DataTable_StoryLoader storyDataBase;
    //private StoryDataBase storyDataBase;

    public Story[] story = new Story[13];

    public void Init()
    {
        storyDataBase = dataManager.dataTable_StoryLoader;

        for (int i = 0; i < story.Length; i++)
        {
            story[i] = new Story();
            story[i].SetStory(storyDataBase.GetByKey(i).Text.Replace("&", "\n"), storyDataBase.GetByKey(i).Delete);
        }

/*        story[0].SetStory("������.", false);
        story[1].SetStory("\n\n�� ���迡 �տ��� �߻��ߴ�.", true);
        story[2].SetStory("���� ���� �տ��� ������\nƢ��� �°� ��������\n�׾߸��� �ڿ����ؿ���.", true);
        story[3].SetStory("������,\n�η��� ������� �ʾҴ�.\n\n�տ����� ���� ������ ������ �޾Ƶ鿩 Ư���� ���� ���ϰ� �� ����� �����̾���.", true);
        story[4].SetStory("���� �ٷ� �� ���� ����\n\n'����'��.", true);
        story[5].SetStory("���� ��ȸ�� �Ҽӵ� ���ʹ�.\n�װ͵�....\n���Ϳ� ���õ� ��࿡ ������ ���� ������ ����Ͽ� �뿹�� �ٸ� ���� ���...", true);
        story[6].SetStory("ū ������ �����Ͽ� ���ݿ� Ȧ�� ���͵��� ��ȸ�� ������� ����, �ž��� ���� ���� �ϴ� ���.", true);
        story[7].SetStory("�׷��� ���� �� ���� ���� ���� ���ϸ����� �ο��Ϳ��� �پ�ٳ�� �Ѵ١�", true);
        story[8].SetStory("�����Դ� �Ƴ��� �� ���� ���� �ϳ� �ִµ� ���� ���ϴ��� ������ ���ڸ��� ä������ ���ؼ� �̾��ϴ١�", true);
        story[9].SetStory("������ ���� ���� ���ؼ��� ��� ���� �ؾ� �Ѵ�.", true);
        story[10].SetStory("(���̷� �Ҹ�)�ϡ� �� �տ��� �߻��ߴ�.\n�� ���� ������ �̷��� �Ϸ簡 �ִ� �ϰ� ���� �������� ����� ���ܿ���", true);
        story[11].SetStory("(��ȭ���Ҹ�)��?\n(�߾�Ÿ���) �ƴ�...�б����� �ʿ��� �غ��� �ִµ�..\n������ �������� ����, ī��� ��\n(�� ����)\n", true);
        story[12].SetStory("�ϡ������ס�", false);*/
    }
}
