using System;
using System.Collections.Generic;

namespace Clones
{/*метод Execute, принимающий на вход описание команды в виде строки
    и возвращающий результат в виде строки.

Поддерживаемые команды:

    learn ci pi. Обучить клона с номером ci по программе pi.
    rollback ci. Откатить последнюю программу у клона с номером ci.
    relearn ci. Переусвоить последний откат у клона с номером ci.
    clone ci. Клонировать клона с номером ci.
    check ci. Вернуть программу, которой клон с номером ci владеет 
    и при этом усвоил последней. 
    Если клон владеет только базовыми знаниями, верните "basic".
*/
    public class ListStack
    {
        List<int> list = new List<int>();

        public void Push(int value)
        {
            list.Add(value);
        }

        public int Pop()
        {
            if (IsEmpty()) throw new InvalidOperationException();
            var result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }

        private bool IsEmpty()
        {
           return  list.Count == 0;
        }

        public string Peek()
        {
            return IsEmpty() ? null : list[list.Count - 1].ToString();
        }

        public ListStack Copy() //new
        {
            return new ListStack { list = new List<int>(list) };
        }
    }

    public class Clone
    {
        public ListStack LearnedProgramms;
        public ListStack RollbackedProgramms;
        private bool clonned = false;

        public void Learn(int program)
        {
            if (clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                clonned = false;
            }
            RollbackedProgramms = new ListStack();
            LearnedProgramms.Push(program);
        }

        public void RollBack()
        {
            if (clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                RollbackedProgramms = RollbackedProgramms.Copy();
                clonned = false;
            }
            RollbackedProgramms.Push(LearnedProgramms.Pop());
        }

        public void Realern()
        {
            if (clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                RollbackedProgramms = RollbackedProgramms.Copy();
                clonned = false;
            }
            LearnedProgramms.Push(RollbackedProgramms.Pop());
        }

        public string Check()
        {
            return LearnedProgramms.Peek() == null
              ? "basic"
              : LearnedProgramms.Peek();
        }

        public Clone MakeCopy()
        {
            var cln = new Clone
            {
                LearnedProgramms = LearnedProgramms,
                RollbackedProgramms = RollbackedProgramms,
                clonned = true
            };
            this.clonned = true;
            return cln;
        }
    }

    public class My_CloneVersionSystem : ICloneVersionSystem
    {
        private List<Clone> clns = new List<Clone>();

        public string Execute(string query)
        {

            var commands = query.Split();
            var cloneNumber = int.Parse(commands[1]) - 1;
            if (cloneNumber + 1 > clns.Count)
                clns.Add(new Clone { LearnedProgramms = new ListStack(), RollbackedProgramms = new ListStack() });
            return ExecuteCommand(commands, cloneNumber);
        }

        private string ExecuteCommand(string[] commands, int cloneNumber)
        {
            string message = null;
            switch (commands[0])
            {
                case "learn":
                    clns[cloneNumber].Learn(int.Parse(commands[2]));
                    break;
                case "rollback":
                    clns[cloneNumber].RollBack();
                    break;
                case "relearn":
                    clns[cloneNumber].Realern();
                    break;
                case "clone":
                    clns.Add(clns[cloneNumber].MakeCopy());
                    break;
                case "check":
                    message = clns[cloneNumber].Check();
                    break;
            }
            return message;
        }
    }
}
