using Microsoft.Extensions.CommandLineUtils;
using System;

namespace StegoPrint {
    public class ExtractFingerprintCommand : CommandLineApplication {
        CommandOption _keyfile;
        CommandOption _input;
        public ExtractFingerprintCommand() {
            Name = "extract";
            Description = "Extracts a fingerprint from a file";

            _keyfile = Option("-$|-k |--keyfile <keyfile>", "The keyfile for the fingerprint", CommandOptionType.SingleValue);
            _input = Option("-$|-i |--input <input>", "The input audio file to process", CommandOptionType.SingleValue);

            HelpOption("-h | -? | --help");
            OnExecute((Func<int>)RunCommand);
        }

        private int RunCommand() {
            new Fingerprinter().ExtractMessage(_keyfile.Value(), _input.Value());
            return -1;
        }
    }
}
