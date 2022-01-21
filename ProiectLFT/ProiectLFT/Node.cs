using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectLFT
{
    abstract class Node
    {
        public abstract TipAtomLexical tip { get; }
        public abstract IEnumerable<Node> getchildrens();
    }
}
