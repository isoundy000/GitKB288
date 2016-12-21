using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    #region �˲���OcTree
    /// <summary>
    /// �˲���
    /// </summary>
    internal unsafe class OcTree
   {
       #region ˽���ֶ�
       OcTreeNode _rootNode;
       int _prefixColor;
       OcTreeNode _prefixNode;
       int _colorDepth = 8;
       int _maxColors = 0;
       int leaves = 0;
       internal OcTreeNode[] ReducibleNodes;
       #endregion

       internal OcTree(int colorDepth)
       {
           this._colorDepth = colorDepth;
           _maxColors = 1<<_colorDepth;
           ReducibleNodes = new OcTreeNode[colorDepth];
           _rootNode = new OcTreeNode(colorDepth, 0, this);         
       }

       internal void TracePrevious(OcTreeNode node)
       {
           this._prefixNode = node;
       }

       internal void IncrementLeaves()
       {
           leaves++;
       }

       internal Color32[] Pallette()
       {
           while(leaves>_maxColors)
           {
               Reduce();
           }
           List<Color32> palltte = new List<Color32>();
           _rootNode.GetPalltte(palltte);
           return palltte.ToArray();
       }

       void Reduce()
       {
           int index;
           for( index = _colorDepth-1;(index>0&&ReducibleNodes[index]==null);index--)
           {

           }
           OcTreeNode node = ReducibleNodes[index];
           ReducibleNodes[index] = node.NextReducible;
           leaves -= node.Reduce();
           _prefixNode = null;
       }

       internal int GetPaletteIndex(Color32* pixel)
       {
           return _rootNode.GetPaletteIndex(pixel, 0);
       }

       internal void AddColor(Color32* pixel)
       {
           //�����ǰ������ɫ��ǰһ����ɫ��ͬ
           if (_prefixColor == pixel->ARGB)
           {
               if (_prefixNode == null)
               {
                   //����ǵ�һ�δ���
                   _prefixColor = pixel->ARGB;
                   _rootNode.AddColor(pixel, 0, this);
                   return;
               }          
               //�����ɫ��ǰһ����ͬ���Ҳ��ǵ�һ�δ������Ը��ƿ�����һ�εĴ�����
               _prefixNode.Increment(pixel);
               return;
           }
           //�������һ����ɫ��ͬ,����Ҫ�����ɫ���ص��˲���
           _prefixColor = pixel->ARGB;
           _rootNode.AddColor(pixel, 0, this);
           return;
       }
   }
    #endregion
}
