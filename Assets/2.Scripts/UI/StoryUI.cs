using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryUI : MonoBehaviour
{
    private List<string> storyText = new List<string>();

    private void Start()
    {
        storyText.Clear();
        storyText.Add("������.\n\n�� ���迡 �տ��� �߻��ߴ�.");
        storyText.Add("���� ���� �տ��� ������ Ƣ��� �°� �������� �׾߸��� �ڿ����ؿ���.");
        storyText.Add("������,\n�η��� ������� �ʾҴ�.\n\n�տ����� ���� ������ ������ �޾Ƶ鿩 Ư���� ���� ���ϰ� �� ����� �����̾���.");
        storyText.Add("���� �ٷ� �� ���� ����\n\n'����'��.");
        storyText.Add("���� ��ȸ�� �Ҽӵ� ���ʹ�.\n�װ͵�....\n���Ϳ� ���õ� ��࿡ ������ ���� ������ ����Ͽ� �뿹�� �ٸ� ���� ���...");
        storyText.Add("ū ������ �����Ͽ� ���ݿ� Ȧ�� ���͵��� ��ȸ�� ������� ����, �ž��� ���� ���� �ϴ� ���.");
        storyText.Add("�׷��� ���� �� ���� ���� ���� ���ϸ����� �ο��Ϳ��� �پ�ٳ�� �Ѵ١�");
        storyText.Add("");
        storyText.Add("�����Դ� �Ƴ��� �� ���� ���� �ϳ� �ִµ� ���� ���ϴ��� ������ ���ڸ��� ä������ ���ؼ� �̾��ϴ١�");
        storyText.Add("������ ���� ���� ���ؼ��� ��� ���� �ؾ� �Ѵ�.");
        storyText.Add("(���̷� �Ҹ�)�ϡ� �� �տ��� �߻��ߴ�.\n�� ���� ������ �̷��� �Ϸ簡 �ִ� �ϰ� ���� �������� ����� ���ܿ���");
        storyText.Add("(��ȭ���Ҹ�)��?\n(�߾�Ÿ���) �ƴ�...�б����� �ʿ��� �غ��� �ִµ�..\n������ �������� ����, ī��� ��\n(�� ����)\n");
        storyText.Add("�ϡ������ס�");
    }
}
