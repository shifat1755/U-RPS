using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using ConsoleTables;
using System.Net.Mail;

class Program
{
    static string userInput(string[] moves)
    {
        Console.WriteLine("Available moves:");
        for (int i = 0; i < moves.Length; i++)
        {
            Console.WriteLine($"{i + 1} - {moves[i]}");
        }
        Console.WriteLine("0 - Exit\n? - Help\nEnter your move:");
        string input = Console.ReadLine();
        return input;

    }

    static int PcInput(int len)
    {
        Random rand = new Random();
        int pcMoveInd = rand.Next(len);
        return pcMoveInd;
    }

    static string GameLogic(int usrMoveInd, int pcMoveInd, int len)
    {
        string res = "";
        int winRange = pcMoveInd + len / 2;
        if (pcMoveInd == usrMoveInd)
        {
            res = "draw";
        }
        else if (winRange < len)
        {
            res = (usrMoveInd > pcMoveInd && usrMoveInd <= winRange) ? "win" : "lose";
        }
        else
        {
            res = (usrMoveInd >= pcMoveInd - (int)len / 2 && usrMoveInd < pcMoveInd) ? "lose" : "win";
        }
        return res;
    }

    static string GetHmac()
    {
        using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
        {
            byte[] bytes = new byte[32];
            rnd.GetBytes(bytes);
            string value = BitConverter.ToString(bytes).Replace("-", "");
            return value;
        }
    }

    static string GetHash(string a)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(a);
        IDigest digest = new Sha3Digest(256);
        digest.BlockUpdate(inputBytes, 0, inputBytes.Length);
        byte[] bytes = new byte[digest.GetDigestSize()];
        digest.DoFinal(bytes, 0);
        string res = BitConverter.ToString(bytes).Replace("-", "");
        return res;
    }

    static void ResTable(string[] arr)
    {
        string[] moves = new string[arr.Length + 1];
        moves[0] = "v Pc/User >";
        Array.Copy(arr, 0, moves, 1, arr.Length);
        var table = new ConsoleTable(moves);
        for (int i = 0; i < arr.Length; i++)
        {
            string[] row = new string[arr.Length + 1];
            row[0] = arr[i];
            for (int j = 0; j < arr.Length; j++)
            {
                row[j + 1] = GameLogic(j, i, arr.Length);
            }
            table.AddRow(row);
        }
        table.Write(Format.Alternative);
        return;
    }

    static bool checkArgs(string[] args)
    {
        int count = args.Distinct().Count();
        bool valid = true;
        if (count < args.Length)
        {
            Console.WriteLine("Elements must be distinct.");
            valid = false;
        }
        if (count < 3)
        {
            Console.WriteLine("Number of elements must be greater than two.");
            valid = false;
        }
        if ((count & 1) == 0)
        {
            Console.WriteLine("Count of elements cannot be even.");
            valid = false;
        }
        return valid;
    }

    static void Main(string[] args)
    {
        bool isValid = checkArgs(args);
        if (!isValid) { return; }
        while (true)
        {
            int pcMoveInd = PcInput(args.Length);
            string hmacKey = GetHmac();
            string hash = GetHash(hmacKey + pcMoveInd.ToString());
            Console.WriteLine($"\n\nHMAC: {hash}");
            string usrMove = userInput(args);
            if (usrMove == "0")
            {
                Console.WriteLine("Exiting game..");
                break;
            }
            if (usrMove == "?")
            {
                ResTable(args);
                continue;
            }
            if (!int.TryParse(usrMove, out int usrMoveInd) || (usrMoveInd > args.Length || usrMoveInd < 0))
            {
                Console.WriteLine("Invalid Input");
                continue;
            }
            Console.WriteLine($"Your move: {args[usrMoveInd - 1]}.\nPc move: {args[pcMoveInd]}.");
            string res = GameLogic(usrMoveInd - 1, pcMoveInd, args.Length);
            string dialogue = res == "win" ? "You Win!" : res == "draw" ? "It's a draw." : "You lose.";
            Console.WriteLine($"{dialogue}\nKey: {hmacKey}");
        }
    }
}