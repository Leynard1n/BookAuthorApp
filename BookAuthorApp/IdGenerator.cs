using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAuthorApp
{
    public static class IdGenerator
    {
        private static int _currentAuthorId = 0;
        private static int _currentBookId = 0;

        public static int GenerateAuthorId()
        {
            return ++_currentAuthorId; // Увеличиваем и возвращаем новый ID для автора
        }

        public static int GenerateBookId()
        {
            return ++_currentBookId; // Увеличиваем и возвращаем новый ID для книги
        }
    }
}

