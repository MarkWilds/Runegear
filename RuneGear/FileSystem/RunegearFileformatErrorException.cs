using System;

namespace RuneGear.FileSystem
{
    public class RunegearFileformatErrorException : Exception
    {
        public RunegearFileformatErrorException(string text)
            :base(text)
        {
            
        }

        public override string Message => "Runemap fileformat could not loaded\n\nDetails:\n" + base.Message;
    }
}
