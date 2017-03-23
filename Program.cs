using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;

namespace StegoPrint {
    class Program {
        static int Main(string[] args) {
            try {
                return new App().Execute(args);
            } catch (Exception ex) {
                Console.Write(ex);
                return 1;
            }
        }
    }
}