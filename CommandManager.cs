using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class CommandManager
    {
        private Stack<DocumentCommand> commands = new Stack<DocumentCommand>();
        
        private bool isUndoing = false;

        public void ExecuteCommand(DocumentCommand command)
        {
            if (isUndoing)
            {
                isUndoing = false;
            }
            else
            {
                command.Execute();
                commands.Push(command);
            }
        }

        public void Undo()
        {
            if (commands.Count > 0)
            {
                DocumentCommand command = commands.Pop();
                command.Undo();
                isUndoing = true;
            }
            else
            {
                Console.WriteLine("There is nothing to undo!");
            }
        }

        public void Redo()
        {
            if (commands.Count > 0)
            {
                DocumentCommand command = commands.Pop();
                command.Redo();
                commands.Push(command);
                isUndoing = false;
            }
            else
            {
                Console.WriteLine("There is nothing to redo!");
            }
        }
    }
}
