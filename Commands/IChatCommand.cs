using System;

namespace EnglishTrainerBot
{
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
