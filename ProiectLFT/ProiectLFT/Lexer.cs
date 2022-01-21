using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectLFT
{
    class Lexer
    {
        private readonly string TextInput;

        private int currentIndex;
        private List<string> errorList;
        public Lexer(string input)
        {
            TextInput = input;
            errorList = new List<string>();
        }

        public List<string> getErrorList()
        {
            return errorList;
        }
        private char getCurrentSymbol(int val = 0)
        {

            if (currentIndex >= TextInput.Length)
                   return '\0';
            if (val == 0)
            {
                while (TextInput[currentIndex] == ' ') { incrementIndex(); }
            }
            else
            {
                return TextInput[currentIndex];
            }
            return TextInput[currentIndex];

        }
        public void restartCount()
        {
            currentIndex = 0;
        }
        private void incrementIndex()
        {
            currentIndex++;
        }
        public string getdeclarname()
        {
            string name = "";
            char curr = getCurrentSymbol();
            while (curr != ' ' && curr != '=' && curr != ',' && curr != ';' && curr != '\0')
            {
                name += curr;
                incrementIndex();
                curr = getCurrentSymbol();

            }
            if (curr == '\0')
                errorList.Add(new string("Invalid char"));
            return name;
        }
        public string getValue(int str = 0)
        {
            string appendNr = "";
            if (str == 0)
            {
                if (getCurrentSymbol() == '\"')
                    incrementIndex();
                while (char.IsDigit(getCurrentSymbol()) || getCurrentSymbol() == '.' || char.IsLetter(getCurrentSymbol()))
                {
                    appendNr += getCurrentSymbol();
                    incrementIndex();
                }
                if (getCurrentSymbol() == '\"')
                    incrementIndex();
            }
            else
            {
                if (getCurrentSymbol() == '\"')
                    incrementIndex();
                while (char.IsDigit(getCurrentSymbol(str)) || getCurrentSymbol(str) == '.' || char.IsLetter(getCurrentSymbol(str)) || getCurrentSymbol(str) == ' ')
                {
                    appendNr += getCurrentSymbol(str);
                    incrementIndex();
                }
                if (getCurrentSymbol(str) == '\"')
                    incrementIndex();
            }
            
            return appendNr;
        }
        public List<AtomLexical> getAtomLexical()
        {
            List<AtomLexical> ceva = new List<AtomLexical> { };
            ;
            if (currentIndex >= TextInput.Length)
            {
                ceva.Add(new AtomLexical(TipAtomLexical.TerminatorAtom, "\n", "\n"));
                return ceva;
            }
            if (getCurrentSymbol() == ';')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.TerminatorAtom, ";", ";"));
                return ceva;
            }
            if (char.IsWhiteSpace(getCurrentSymbol()))
            {
                string appendNr = "";
                while (char.IsWhiteSpace(getCurrentSymbol()))
                {
                    appendNr += getCurrentSymbol();
                    incrementIndex();
                }
                ceva.Add(new AtomLexical(TipAtomLexical.SpatiuAtom, appendNr, null));
                return ceva;
            }
            if (getCurrentSymbol() == '+')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.PlusAtom, "+", null));
                incrementIndex();
                return ceva;
            }
            if (getCurrentSymbol() == '-')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.MinusAtom, "-", null));
                incrementIndex();
                return ceva;
            }
            if (getCurrentSymbol() == '*')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.InmultireAtom, "*", null));
                incrementIndex();
                return ceva;
            }
            if (getCurrentSymbol() == '/')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.ImpartireAtom, "/", null));
                incrementIndex();
                return ceva;
            }
            if (getCurrentSymbol() == '(')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.ParantezaDeschisaAtom, "(", null));
                incrementIndex();
                return ceva;
            }
            if (getCurrentSymbol() == ')')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.ParantezaInchisaAtom, ")", null));
                incrementIndex();
                return ceva;
            }
            if (getCurrentSymbol() == '=')
            {
                ceva.Add(new AtomLexical(TipAtomLexical.EgalAtom, "=", null));
                incrementIndex();
                return ceva;
            }
            if (char.IsDigit(getCurrentSymbol()))
            {
                string appendNr = "";
                appendNr += getCurrentSymbol();
                incrementIndex();
                if(getCurrentSymbol()=='.')
                {
                    appendNr += getCurrentSymbol();
                    incrementIndex();
                }
                while (char.IsDigit(getCurrentSymbol()))
                {
                    appendNr += getCurrentSymbol();
                    incrementIndex();
                }
                if (!double.TryParse(appendNr, out double result))
                    errorList.Add("Error: Number too big! Can't cast to int");

                ceva.Add(new AtomLexical(TipAtomLexical.NumarAtom, "const", result));
                return ceva;
            }
            if (getCurrentSymbol() == '\"')
            {
                string value = getValue(1);
                ceva.Add(new AtomLexical(TipAtomLexical.StringConst, " ", value));
                return ceva;
            }
            if (TextInput.Length >= 3 && TextInput.Substring(0, 3) == "int")
            {
                currentIndex += 2;
                do
                {
                    incrementIndex();
                    string name = getdeclarname();
                    if (getCurrentSymbol() == '=')
                    {
                        incrementIndex();
                        string value = getValue();
                        AtomLexical nou = new AtomLexical(TipAtomLexical.IntAtom, name, Convert.ToInt32(value));
                        ceva.Add(nou);
                    }
                    else
                    {
                        AtomLexical nou = new AtomLexical(TipAtomLexical.IntAtom, name, 0);
                        ceva.Add(nou);
                    }
                }
                while (getCurrentSymbol() == ',');
                if (getCurrentSymbol() == ';')
                {
                    incrementIndex();
                    return ceva;
                }
            }
            if (TextInput.Length >= 5 && TextInput.Substring(0, 5) == "float")
            {
                currentIndex += 4;


                do
                {
                    incrementIndex();
                    string name = getdeclarname();
                    if (getCurrentSymbol() == '=')
                    {
                        incrementIndex();
                        string value = getValue();
                        AtomLexical nou = new AtomLexical(TipAtomLexical.FloatAtom, name, Convert.ToDouble(value));
                        ceva.Add(nou);
                    }
                    else
                    {
                        AtomLexical nou = new AtomLexical(TipAtomLexical.FloatAtom, name, 0);
                        ceva.Add(nou);
                    }
                }
                while (getCurrentSymbol() == ',');
                if (getCurrentSymbol() == ';')
                {
                    incrementIndex();
                    return ceva;
                }
            }

            if (TextInput.Length >= 6 && TextInput.Substring(0, 6) == "string")
            {
                currentIndex += 5;


                do
                {
                    incrementIndex();
                    string name = getdeclarname();
                    if (getCurrentSymbol() == '=')
                    {
                        incrementIndex();

                        string value = getValue(1);
                        AtomLexical nou = new AtomLexical(TipAtomLexical.StringAtom, name, value);
                        ceva.Add(nou);
                    }
                    else
                    {
                        AtomLexical nou = new AtomLexical(TipAtomLexical.StringAtom, name, 0);
                        ceva.Add(nou);
                    }
                }
                while (getCurrentSymbol() == ',');
                if (getCurrentSymbol() == ';')
                {
                    incrementIndex();
                    return ceva;
                }
            }
            if (TextInput.Length >= 6 && TextInput.Substring(0, 6) == "double")
            {
                currentIndex += 5;


                do
                {
                    incrementIndex();
                    string name = getdeclarname();
                    if (getCurrentSymbol() == '=')
                    {
                        incrementIndex();
                        string value = getValue();
                        AtomLexical nou = new AtomLexical(TipAtomLexical.DoubleAtom, name, Convert.ToDouble(value));
                        ceva.Add(nou);
                    }
                    else
                    {
                        AtomLexical nou = new AtomLexical(TipAtomLexical.DoubleAtom, name, 0);
                        ceva.Add(nou);
                    }
                }
                while (getCurrentSymbol() == ',');
                if (getCurrentSymbol() == ';')
                {
                    incrementIndex();
                    return ceva;
                }
            }
            {
                string str = getValue();
                ceva.Add(new AtomLexical(TipAtomLexical.StringConst, str, " "));
                return ceva;

            }

            ceva = new List<AtomLexical> { new AtomLexical(TipAtomLexical.InvalidAtom, " ", null) };
            return ceva;
        }
        public AtomLexical GetAtomLexical()
        {

            return new AtomLexical(TipAtomLexical.InvalidAtom, TextInput[currentIndex - 1].ToString(), null);
        }
    }
}
