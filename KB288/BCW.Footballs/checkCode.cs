using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace BCW.Footballs
{
    class checkCode
    {
        private ArrayList data;

        public int Size
        {
            get
            {
                return data.Count;
            }
        }

        public checkCode()
        {
            data = new ArrayList();
        }

        public void add(String str)
        {
            data.Add(str);
        }

        public string get(int index)
        {
            if (index < data.Count)
                return (string)data[index];
            else
                return null;
        }

        public bool remove(string str)
        {
            if (data.IndexOf(str) == -1) return false;
            data.Remove(str);
            return true;
        }

        public void remove(int index)
        {
            data.RemoveAt(index);
        }

        public char check(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class checkCodeker
    {
        public static bool check(string str)
        {
            checkCode[] unclosedTags = getUnclosedTags(str);

            if (unclosedTags[0].Size != 0)
            {
                return false;
            }
            for (int i = 0; i < unclosedTags[1].Size; i++)
            {
                if (unclosedTags[1].get(i) != null)
                    return false;
            }

            return true;
        }

        public static string fix(String str)
        {
            StringBuilder fixeds = new StringBuilder(); // 存放修复后的字符串
            checkCode[] unclosedTags = getUnclosedTags(str);

            // 生成新字符串
            for (int i = unclosedTags[0].Size - 1; i > -1; i--)
            {
                fixeds.Append("<" + unclosedTags[0].get(i) + ">");
            }

            fixeds.Append(str);

            for (int i = unclosedTags[1].Size - 1; i > -1; i--)
            {
                String s = null;
                if ((s = unclosedTags[1].get(i)) != null)
                {
                    fixeds.Append("</" + s + ">");
                }
            }

            return fixeds.ToString();
        }

        private static checkCode[] getUnclosedTags(String str)
        {
            StringBuilder temp = new StringBuilder(); // 存放标签
            checkCode[] unclosedTags = new checkCode[2];
            unclosedTags[0] = new checkCode(); // 前不闭合，如有</div>而前面没有<div>
            unclosedTags[1] = new checkCode(); // 后不闭合，如有<div>而后面没有</div>
            bool flag = false; // 记录双引号"或单引号'
            char currentJump = ' '; // 记录需要跳过''还是""

            char current = ' ', last = ' '; // 当前 & 上一个

            // 开始判断
            for (int i = 0; i < str.Length;)
            {
                current = str[i++]; // 读取一个字符
                if (current == '"' || current == '\'')
                {
                    flag = flag ? false : true; // 若为引号，flag翻转
                    currentJump = current;
                    if (flag)
                    {
                        while (i < str.Length && str[i++] != currentJump)
                            ; // 跳过引号之间的部分
                        flag = false;
                    }
                }
                else if (current == '<')
                { // 开始提取标签
                    current = str[i++];
                    if (current == '/')
                    { // 标签的闭合部分，如</div>
                        current = str[i++];

                        // 读取标签
                        while (i < str.Length && current != '>')
                        {
                            temp.Append(current);
                            current = str[i++];
                        }

                        // 从tags_bottom移除一个闭合的标签
                        if (!unclosedTags[1].remove(temp.ToString()))
                        { // 若移除失败，说明前面没有需要闭合的标签
                            unclosedTags[0].add(temp.ToString()); // 此标签需要前闭合
                        }
                        temp.Remove(0, temp.Length); // 清空temp
                    }
                    else
                    { // 标签的前部分，如<div>
                        last = current;
                        while (i < str.Length && current != ' '
                                && current != ' ' && current != '>')
                        {
                            temp.Append(current);
                            last = current;
                            current = str[i++];
                        }

                        // 已经读取到标签，跳过其他内容，如<div id=test>跳过id=test
                        while (i < str.Length && current != '>')
                        {
                            last = current;
                            current = str[i++];
                            if (current == '"' || current == '\'')
                            { // 判断双引号
                                flag = flag ? false : true;
                                currentJump = current;
                                if (flag)
                                { // 若引号不闭合，跳过到下一个引号之间的内容
                                    while (i < str.Length && str[i++] != currentJump)
                                        ;
                                    current = str[i++];
                                    flag = false;
                                }
                            }
                        }
                        if (last != '/' && current == '>') // 判断这种类型：<TagName />
                            unclosedTags[1].add(temp.ToString());
                        temp.Remove(0, temp.Length);
                    }
                }
            }
            return unclosedTags;
        }
    }

   
}
