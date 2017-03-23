using System;
using System.IO;
using System.Text;

namespace StegoPrint {
    public class Fingerprinter {
        private Stream GetMessageStream(string message) {
            BinaryWriter messageWriter = new BinaryWriter(new MemoryStream());
            messageWriter.Write(message.Length);
            messageWriter.Write(Encoding.ASCII.GetBytes(message));
            messageWriter.Seek(0, SeekOrigin.Begin);
            return messageWriter.BaseStream;
        }

        public void AddMessage(string message, string keyFile, string inputFile, string outputFile) {
            Stream sourceStream = null;
            FileStream destinationStream = null;
            WaveStream audioStream = null;

            //create a stream that contains the message, preceeded by its length
            Stream messageStream = GetMessageStream(message);
            //open the key file
            Stream keyStream = new FileStream(keyFile, FileMode.Open);

            try {

                //how man samples do we need?
                long countSamplesRequired = WaveUtility.CheckKeyForMessage(keyStream, messageStream.Length);

                if (countSamplesRequired > Int32.MaxValue) {
                    throw new Exception("Message too long, or bad key! This message/key combination requires " + countSamplesRequired + " samples, only " + Int32.MaxValue + " samples are allowed.");
                }

                sourceStream = new FileStream(inputFile, FileMode.Open);

                //create an empty file for the carrier wave
                destinationStream = new FileStream(outputFile, FileMode.Create);

                //copy the carrier file's header
                audioStream = new WaveStream(sourceStream, destinationStream);
                if (audioStream.Length <= 0) {
                    throw new Exception("Invalid WAV file");
                }

                //are there enough samples in the carrier wave?
                if (countSamplesRequired > audioStream.CountSamples) {
                    String errorReport = "The carrier file is too small for this message and key!\r\n" +
                        "Samples available: " + audioStream.CountSamples + "\r\n" +
                        "Samples needed: " + countSamplesRequired;
                    throw new Exception(errorReport);
                }

                //hide the message
                WaveUtility utility = new WaveUtility(audioStream, destinationStream);
                utility.Hide(messageStream, keyStream);
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            } finally {
                if (keyStream != null) { keyStream.Dispose(); }
                if (messageStream != null) { messageStream.Dispose(); }
                if (audioStream != null) { audioStream.Dispose(); }
                if (sourceStream != null) { sourceStream.Dispose(); }
                if (destinationStream != null) { destinationStream.Dispose(); }
            }
        }
        public void ExtractMessage(string keyFile, string inputFile) {
            FileStream sourceStream = null;
            WaveStream audioStream = null;
            //create an empty stream to receive the extracted message
            MemoryStream messageStream = new MemoryStream();
            //open the key file
            Stream keyStream = new FileStream(keyFile, FileMode.Open);

            try {
                //open the carrier file
                sourceStream = new FileStream(inputFile, FileMode.Open);
                audioStream = new WaveStream(sourceStream);
                WaveUtility utility = new WaveUtility(audioStream);

                //exctract the message from the carrier wave
                utility.Extract(messageStream, keyStream);

                messageStream.Seek(0, SeekOrigin.Begin);
                string extractedMessage = new StreamReader(messageStream).ReadToEnd();

                Console.WriteLine("Message is.....");
                Console.WriteLine(extractedMessage);

                Console.ReadKey();
            } catch (Exception ex) {
                Console.WriteLine($"Error extracting message {ex.Message}");
            } finally {
                if (keyStream != null) { keyStream.Dispose(); }
                if (messageStream != null) { messageStream.Dispose(); }
                if (audioStream != null) { audioStream.Dispose(); }
                if (sourceStream != null) { sourceStream.Dispose(); }
            }
        }

    }
}