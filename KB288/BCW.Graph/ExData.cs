using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    internal  class ExData
    {
        private static readonly byte _extensionIntroducer = 0x21;
        private static readonly byte _blockTerminator = 0;
        internal byte ExtensionIntroducer
        {
            get
            {
                return _extensionIntroducer;
            }
        }
        internal byte BlockTerminator
        {
            get
            {
                return _blockTerminator;
            }
        }
    }
}
