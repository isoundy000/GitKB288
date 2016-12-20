using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    #region ��ɫ�帨����PaletteHelper
    /// <summary>
    /// ��ɫ�帨����
    /// </summary>
    internal class PaletteHelper
    {    
         #region ���������л�ȡ��ɫ�б�
        /// <summary>
        /// ���������л�ȡ��ɫ�б�
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal static Color32[] GetColor32s(byte[] table)
        {
            Color32[] tab = new Color32[table.Length / 3];
            int i = 0;
            int j = 0;
            while (i < table.Length)
            {
                byte r = table[i++];
                byte g = table[i++];
                byte b = table[i++];
                byte a = 255;
                Color32 c = new Color32(a, r, g, b);
                tab[j++] = c;
            }
            return tab;
        }
         #endregion
    }
    #endregion
}
