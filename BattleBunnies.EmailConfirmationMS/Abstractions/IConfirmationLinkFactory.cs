using System;

namespace BattleBunnies.EmailConfirmationMS.Abstractions;

public interface IConfirmationLinkFactory
{
    string Create(string email, String code);
}
