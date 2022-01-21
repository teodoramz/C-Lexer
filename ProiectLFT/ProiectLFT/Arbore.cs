using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectLFT
{
    class Arbore
    {
        public Arbore(Expresie root, AtomLexical end)
        {
            Root = root;
            End = end;
        }

        public Expresie Root { get; }
        public AtomLexical End { get; }
    }
}
