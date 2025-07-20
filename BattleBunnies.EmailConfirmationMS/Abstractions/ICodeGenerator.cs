using System;

namespace BattleBunnies.EmailConfirmationMS.Abstractions;

public interface ICodeGenerator
{
    string Generate(int length = 32);
}
