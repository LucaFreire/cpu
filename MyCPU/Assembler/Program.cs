using System;
using System.IO;
using System.Linq;

using System;
using System.Linq;

if (args.Length == 0)
{
    Console.WriteLine("Você precisa passar um parâmetro para o arquivo a ser montado.");
    return;
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    Console.WriteLine("O arquivo especifiado não existe.");
    return;
}

StreamWriter writer = null;
StreamReader reader = null;
try
{
    writer = new StreamWriter("memory");
    writer.WriteLine("v2.0 raw");
    reader = new StreamReader(filePath);

    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        line = processLine(line);
        writer.Write(line);
        writer.Write(" ");
    }
}
catch (Exception ex)
{
    Console.WriteLine("O seguinte erro ocorreu durante o processo:");
    Console.WriteLine(ex.Message);
}
finally
{
    reader.Close();
    writer.Close();
}

string processLine(string line)
{
    byte[] opCode = new byte[16];

    string linha = line.Replace(" ", "");

    for (int j = 0; j < linha.Length; j++)
    {

        if (linha.Substring(0, 3) == "nop")
            for (int i = 0; i < opCode.Length; i++)
                opCode[i] = 0;

        else if (linha.Substring(0, 3) == "ret")
            for (int i = 0; i < opCode.Length; i++)
                opCode[i] = 1;

        else if (linha.Substring(0, 3) == "mov")
            mov(linha, opCode);

        else if (line.Substring(0, 3) == "add")
            add(linha, opCode);

        else if (line.Substring(0, 3) == "cmp")
            cmp(linha, opCode);

        else if (line.Substring(0, 3) == "inc")
            inc(linha, opCode);

        else if (line.Substring(0, 3) == "and")
            and(linha, opCode);

        else if (line.Substring(0, 2) == "or")
            or(linha, opCode);

        else if (line.Substring(0, 3) == "xor")
            xor(linha, opCode);

        else if (line.Substring(0, 3) == "not")
            not(linha, opCode);

        else if (line.Substring(0, 3) == "dec")
            dec(linha, opCode);

        else if (line.Substring(0, 4) == "push")
            push(linha, opCode);

        else if (line.Substring(0, 4) == "pop")
            pop(linha, opCode);

    }

    return ShowBin(opCode);
    // return toHex(opCode);
}




//  -- Instructions -- \\
void mov(string linha, byte[] opCode)
{
    string consta;
    string value;
    byte[] mov = new byte[] { 0, 0, 1, 1 }; // 0011 aaaa cccc cccc

    if (linha.Length == 10)
    {
        var a = linha.Substring(4, 2);
        var b = linha.Substring(7, 3);

        consta = ToBinary(byte.Parse(a));
        value = ToBinary(byte.Parse(b));
    }
    else
    {
        var a = linha.Substring(4, 1);
        var b = linha.Substring(6, 3);

        consta = ToBinary(byte.Parse(a));
        value = ToBinary(byte.Parse(b));
    }


    int ind = 0;
    int ind2 = 0;
    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 3)
            opCode[k] = mov[k];

        else if (k <= 7)
        {
            opCode[k] = Convert.ToByte((byte)consta[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = Convert.ToByte((byte)value[ind2] - 48);
            ind2++;
        }
    }
}

void add(string linha, byte[] opCode)
{
    byte[] add = new byte[] { 0, 0, 0, 1, 0, 0, 0, 0 };

    var myLine = linha.Replace("$", "").Replace("add", "");

    int virg = myLine.IndexOf(",");

    string valStr1 = string.Concat(myLine.TakeWhile(x => x != ',').ToArray());
    string valStr2 = string.Concat(myLine.Skip(virg + 1).Take(myLine.Length - virg).ToArray());

    var val1 = ToBinary(byte.Parse(valStr1));
    var val2 = ToBinary(byte.Parse(valStr2));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = add[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = Convert.ToByte((byte)val2[ind2] - 48);
            ind2++;
        }
    }
}

void cmp(string linha, byte[] opCode) // 2
{
    byte[] cmp = new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 }; // 0100 xxxx aaaa bbbb	
    var myLine = linha.Replace("$", "").Replace("cmp", "");

    int virg = myLine.IndexOf(",");

    string valStr1 = string.Concat(myLine.TakeWhile(x => x != ',').ToArray());
    string valStr2 = string.Concat(myLine.Skip(virg + 1).Take(myLine.Length - virg).ToArray());

    string val1 = ToBinary(byte.Parse(valStr1));
    string val2 = ToBinary(byte.Parse(valStr1));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = cmp[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = Convert.ToByte((byte)val2[ind2] - 48);
            ind2++;
        }
    }
}

void inc(string linha, byte[] opCode) // 1
{
    byte[] inc = new byte[] { 0, 0, 0, 1, 1, 0, 1, 1 };

    var Strval = linha.Replace("$", "").Replace("inc", "");

    string val1 = ToBinary(byte.Parse(Strval));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = inc[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = 0;
            ind2++;
        }
    }
}

void and(string linha, byte[] opCode)
{
    byte[] and = new byte[] { 0, 0, 0, 1, 0, 1, 1, 1 };
    var myLine = linha.Replace("and", "");

    int virg = myLine.IndexOf(",");

    string valStr1 = string.Concat(myLine.TakeWhile(x => x != ',').ToArray());
    string valStr2 = string.Concat(myLine.Skip(virg + 1).Take(myLine.Length - virg).ToArray());

    var val1 = ToBinary(byte.Parse(valStr1));
    var val2 = ToBinary(byte.Parse(valStr2));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = and[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = Convert.ToByte((byte)val2[ind2] - 48);
            ind2++;
        }
    }
}

void or(string linha, byte[] opCode)
{
    byte[] or = new byte[] { 0,0,0,1, 1,0,0,0 };
    var myLine = linha.Replace("or", "");

    int virg = myLine.IndexOf(",");

    string valStr1 = string.Concat(myLine.TakeWhile(x => x != ',').ToArray());
    string valStr2 = string.Concat(myLine.Skip(virg + 1).Take(myLine.Length - virg).ToArray());

    var val1 = ToBinary(byte.Parse(valStr1));
    var val2 = ToBinary(byte.Parse(valStr2));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = or[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = Convert.ToByte((byte)val2[ind2] - 48);
            ind2++;
        }
    }
}

void xor(string linha, byte[] opCode)
{
    byte[] xor = new byte[] { 0,0,0,1, 1,0,1,0 };
    var myLine = linha.Replace("xor", "");

    int virg = myLine.IndexOf(",");

    string valStr1 = string.Concat(myLine.TakeWhile(x => x != ',').ToArray());
    string valStr2 = string.Concat(myLine.Skip(virg + 1).Take(myLine.Length - virg).ToArray());

    var val1 = ToBinary(byte.Parse(valStr1));
    var val2 = ToBinary(byte.Parse(valStr2));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = xor[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = Convert.ToByte((byte)val2[ind2] - 48);
            ind2++;
        }
    }
}

void not(string linha, byte[] opCode)
{
    byte[] not = new byte[] { 0,0,0,1, 1,0,0,1 };

    var Strval = linha.Replace("$", "").Replace("inc", "");

    string val1 = ToBinary(byte.Parse(Strval));

    int ind = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = not[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
            opCode[k] = 0;

        
    }
}

void dec(string linha, byte[] opCode)
{
    byte[] dec = new byte[] { 0,0,0,1, 1,1,0,0 };

    var Strval = linha.Replace("$", "").Replace("dec", "");

    string val1 = ToBinary(byte.Parse(Strval));

    int ind = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = dec[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
            opCode[k] = 0;
    }
}

void push(string linha, byte[] opCode)
{
    byte[] push = new byte[] { 0,0,1,0, 0,0,1,1 };

    var Strval = linha.Replace("$", "").Replace("push", "");

    string val1 = ToBinary(byte.Parse(Strval));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = push[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = 0;
            ind2++;
        }
    }
}

void pop(string linha, byte[] opCode)
{
    byte[] pop = new byte[] { 0,0,1,0, 0,1,0,0 };

    var Strval = linha.Replace("$", "").Replace("pop", "");

    string val1 = ToBinary(byte.Parse(Strval));

    int ind = 0;
    int ind2 = 0;

    for (int k = 0; k < opCode.Length; k++)
    {
        if (k <= 7)
            opCode[k] = pop[k];

        else if (k <= 11)
        {
            opCode[k] = Convert.ToByte((byte)val1[ind] - 48);
            ind++;
        }
        else if (k <= 15)
        {
            opCode[k] = 0;
            ind2++;
        }
    }
}

// -- More -- \\
string ShowBin(byte[] code)
{
    string s = "";
    for (int i = 1; i < code.Length + 1; i++)
    {
        s += $"{code[i - 1]}";
        if (i % 4 == 0)
            s += " ";
    }
    return s;
}

string ToBinary(int N)
{
    int d = N;
    int r = -1;

    string binNumber = string.Empty;
    while (d > 0)
    {
        r = d % 2;
        d = d / 2;
        binNumber = r.ToString() + binNumber;
    }
    while (binNumber.Length < 4)
        binNumber = "0" + binNumber;
    return binNumber;
}

