using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectLFT
{
    sealed class ExpresieParanteze : Expresie
    {
        private readonly AtomLexical parstg;
        public Expresie exp { get; }
        private readonly AtomLexical pardr;

        public override TipAtomLexical tip => TipAtomLexical.ExpresieParanteza;

        public override IEnumerable<Node> getchildrens()
        {
            yield return parstg;
            yield return exp;
            yield return pardr;
        }
        public ExpresieParanteze(AtomLexical parstg, Expresie exp, AtomLexical pardr)
        {
            this.parstg = parstg;
            this.exp = exp;
            this.pardr = pardr;

        }
    }
}
