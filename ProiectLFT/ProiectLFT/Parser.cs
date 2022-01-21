using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProiectLFT
{
    class Parser
    {
        private List<AtomLexical> atomsList;
        private int currentIndex;
        private List<string> errorList;
        private bool atribuire = false;
        private int indexEgal = 0;
        private AtomLexical currAtomLexical => getAtom(0);
        public Parser(string filename)
        {

            atomsList = new List<AtomLexical>();
            errorList = new List<string>();
            string line;
            if (File.Exists(filename))
            {
                StreamReader streamReader = new StreamReader(filename);
                do
                {
                    line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    Console.WriteLine("> " + line);
                    parse(line);
                } while (line != null);
            }
            else
            {
                Console.WriteLine($"Error at reading from  file {filename}");
                return;
            }
                Console.WriteLine("You can still give input after the > ");
                while (true)
                {
                    Console.Write("> ");
                    string line1 = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line1))
                        continue;
                    parse(line1);
                }


        }
        private AtomLexical getAtom(int offset)
        {
            if (currentIndex + offset >= atomsList.Count)
                return this.atomsList[atomsList.Count - 1];
            return this.atomsList[currentIndex + offset];
        }
        private AtomLexical getAtomForName(string name)
        {
            foreach (var x in atomsList)
            {
                if (x.name == name)
                    return x;
            }
            return null;
        }

        private void parse(string text)
        {
            Lexer lexer = new Lexer(text);
            List<AtomLexical> currAtoms;
            atribuire = false;
            int nrOper = 0;

            do
            {

                currAtoms = lexer.getAtomLexical();
                
                foreach (var x in currAtoms)
                {
                        if (x.type != TipAtomLexical.SpatiuAtom)
                        {
                            lookForAtomInList(x);
                        }
                        if (x.type == TipAtomLexical.EgalAtom)
                        {
                            atribuire = true;
                            indexEgal = atomsList.Count - 1;
                        }

                    if (x.type == TipAtomLexical.ImpartireAtom || x.type == TipAtomLexical.InmultireAtom || x.type == TipAtomLexical.MinusAtom || x.type == TipAtomLexical.PlusAtom)
                    {
                        nrOper++;
                    }

                }
            }
            while (currAtoms[0].type != TipAtomLexical.TerminatorAtom);
            if (nrOper == 0 && atribuire == true)
            {
                atribuire = false;
            }
            if (atribuire)
            {
                
                try
                {
                    var arbore = parseExpression();
                    Arbore aux = arbore;
                    setVariableType(arbore);
                    AfiseazaArbore(aux.Root);
                    Console.WriteLine();

                }
                catch(Exception e)
                {
                    errorList.Add(e.Message);
                }
            }
            if (lexer.getErrorList().Count > 0)
            {
                this.errorList = lexer.getErrorList();

            }
            if (errorList.Count > 0)
            {
                foreach (var x in errorList)
                    Console.Write("Error: " + x);
                Console.WriteLine();
                Console.WriteLine();
                errorList.Clear();
            }

        }
        private void setVariableType(Arbore arbore)
        {

            var evaluare = new EvaluateExpression(arbore.Root);
            if (atomsList[indexEgal - 1].tip == TipAtomLexical.StringConst)
            {
                errorList.Add(new string(atomsList[indexEgal - 1].name + " isn t declared"));
                return;
            }
            if (atomsList[indexEgal - 1].type == TipAtomLexical.IntAtom)
            {
                atomsList[indexEgal - 1].value = (int)evaluare.getEvaluationNumber;
            }
            if (atomsList[indexEgal - 1].type == TipAtomLexical.FloatAtom)
            {
                atomsList[indexEgal - 1].value = (float)evaluare.getEvaluationNumber;
            }
            if (atomsList[indexEgal - 1].type == TipAtomLexical.DoubleAtom)
            {
                atomsList[indexEgal - 1].value = (double)evaluare.getEvaluationNumber;
            }
            if (atomsList[indexEgal - 1].type == TipAtomLexical.StringAtom)
            {
                atomsList[indexEgal - 1].value = (string)evaluare.getEvaluationString;
            }
            Console.WriteLine(atomsList[indexEgal - 1].name + " = " + atomsList[indexEgal - 1].value);
        }
        private void lookForAtomInList(AtomLexical x)
        {
            if (x.tip == TipAtomLexical.FloatAtom || x.tip == TipAtomLexical.FloatAtom ||
                x.tip == TipAtomLexical.StringAtom || x.tip == TipAtomLexical.IntAtom)
            {
                AtomLexical atom = getAtomForName(x.name);
                if (atom == null)
                    atomsList.Add(x);
                else
                {
                    errorList.Add(new string(x.name + " was declared before"));
                    return;
                }
            }
            if (x.tip == TipAtomLexical.StringConst)
            {
                AtomLexical atom = getAtomForName(x.name);
                if (atom == null)
                {
                    atomsList.Add(x);
                }
                else
                {
                    atomsList.Add(atom);
                }
                return;
            }
            atomsList.Add(x);
        }
        private AtomLexical getCurrentAtomAndIncrement()
        {
            var cur = currentIndex;
            currentIndex++;
            return this.atomsList[cur];
        }
        public AtomLexical checkAtomType(TipAtomLexical tip)
        {
            if (tip == TipAtomLexical.NumarAtom)
            {
                if (currAtomLexical.tip == TipAtomLexical.IntAtom || currAtomLexical.tip == TipAtomLexical.FloatAtom ||
                    currAtomLexical.tip == TipAtomLexical.DoubleAtom || currAtomLexical.tip == TipAtomLexical.StringAtom ||
                    currAtomLexical.tip == TipAtomLexical.NumarAtom || currAtomLexical.tip == TipAtomLexical.StringConst)

                    return getCurrentAtomAndIncrement();
            }
            if (currAtomLexical.tip == tip)
            {
                return getCurrentAtomAndIncrement();
            }
            errorList.Add("Error: No match for atom type: (" + currAtomLexical.tip + ") -> (" + tip.ToString() + ")");
            return new AtomLexical(tip, null, null);
        }


        private Expresie parseFirstExpression()
        {
            if (currAtomLexical.tip == TipAtomLexical.ParantezaDeschisaAtom)
            {
                var stg = getCurrentAtomAndIncrement();
                var exp = parseTerms();
                var dr = checkAtomType(TipAtomLexical.ParantezaInchisaAtom);
                return new ExpresieParanteze(stg, exp, dr);
            }
            var numar = checkAtomType(TipAtomLexical.NumarAtom);
            return new ExpresieNumerica(numar);
        }
        public Expresie parseFactors()
        {
            var stanga = parseFirstExpression();
            while (currAtomLexical.tip == TipAtomLexical.ImpartireAtom ||
                currAtomLexical.tip == TipAtomLexical.InmultireAtom)
            {
                var op = getCurrentAtomAndIncrement();
                var dr = parseFirstExpression();
                stanga = new ExpresieBinara(stanga, dr, op);
            }
            return stanga;
        }
        public Expresie parseTerms()
        {
            var stanga = parseFactors();
            while (currAtomLexical.tip == TipAtomLexical.PlusAtom ||
                currAtomLexical.tip == TipAtomLexical.MinusAtom)
            {
                var op = getCurrentAtomAndIncrement();
                var dr = parseFactors();
                stanga = new ExpresieBinara(stanga, dr, op);
            }
            return stanga;
        }
        public Arbore parseExpression()
        {
            currentIndex = indexEgal + 1;
            var exp = parseTerms();
            var end = checkAtomType(TipAtomLexical.TerminatorAtom);
            return new Arbore(exp, end);
        }



        public void AfiseazaArbore(Node nod, string indentare = "", bool ultimulNod = true)
        {
            var aux = ultimulNod ? "└──" : "├──";
            Console.Write(indentare);
            Console.Write(aux);
            Console.Write(nod.tip);

            if (nod is AtomLexical atom)
            {
                if(atom.name == "+" || atom.name == "-" || 
                    atom.name == "/" || atom.name == "*" || atom.name == ")" || atom.name == "("  )
                {
                    Console.Write(":");
                    Console.Write(" ");
                    Console.Write(atom.name);
                }
                else
                {
                    Console.Write(" ");
                    Console.Write(atom.name);
                    Console.Write(":");
                    Console.Write(atom.value);
                }
            }

            Console.WriteLine();

            indentare += ultimulNod ? "    " : "|   ";

            var lastChild = nod.getchildrens().LastOrDefault();

            foreach (var copil in nod.getchildrens())
            {
                AfiseazaArbore(copil, indentare, copil == lastChild);
            }
        }

    }
}
