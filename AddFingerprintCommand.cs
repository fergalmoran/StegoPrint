using Microsoft.Extensions.CommandLineUtils;
using System;

namespace StegoPrint {
    public class AddFingerprintCommand : CommandLineApplication {
        CommandOption _message;
        CommandOption _keyfile;
        CommandOption _input;
        CommandOption _output;

        public AddFingerprintCommand() {
            Name = "add";
            Description = "Adds a fingerprint to an audio file";

            _message = Option("-$|-m |--message <message>", "The message to embed in the file", CommandOptionType.SingleValue);
            _keyfile = Option("-$|-k |--keyfile <keyfile>", "The keyfile for the fingerprint", CommandOptionType.SingleValue);
            _input = Option("-$|-i |--input <input>", "The input audio file to process", CommandOptionType.SingleValue);
            _output = Option("-$|-o |--output <output>", "The output file", CommandOptionType.SingleValue);

            HelpOption("-h | -? | --help");
            OnExecute((Func<int>)RunCommand);
        }

        private int RunCommand() {
            new Fingerprinter().AddMessage(_message.Value(), _keyfile.Value(), _input.Value(), _output.Value());
            return -1;
        }
    }
}
