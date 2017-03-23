using Microsoft.Extensions.CommandLineUtils;
using System;

namespace StegoPrint {
    class App : CommandLineApplication {
        public App() {
            Commands.Add(new AddFingerprintCommand());
            Commands.Add(new ExtractFingerprintCommand());
        }
    }
}
