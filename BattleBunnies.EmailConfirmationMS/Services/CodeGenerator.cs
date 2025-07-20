using System;
using BattleBunnies.EmailConfirmationMS.Abstractions;

namespace BattleBunnies.EmailConfirmationMS.Services;

public class CodeGenerator : ICodeGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public string Generate(int length = 6)
    {
        var random = new Random();
        return new string([.. Enumerable.Range(0, length).Select(_ => Characters[random.Next(Characters.Length)])]);
    }
}
