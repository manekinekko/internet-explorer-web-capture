using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEScreenCapt.Classes
{
    class Debug
    {
        private bool mActive;

        public Debug(bool state){
            this.mActive = state;
        }
        public void log(string text) {
            if (this.mActive)
            {
                Console.WriteLine(text);
            }
        }
    }
}
