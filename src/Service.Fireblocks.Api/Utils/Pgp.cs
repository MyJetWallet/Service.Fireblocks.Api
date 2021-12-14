using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.IO;

namespace Service.Fireblocks.Api.Utils
{
    public class PgpEncryptor
    {
        /**
        * A simple routine that opens a key ring file and loads the first available key suitable for
        * encryption.
        *
        * @param in
        * @return
        * @m_out
        * @
        */
        private static PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);
            PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(inputStream);
            //
            // we just loop through the collection till we find a key suitable for encryption, in the real
            // world you would probably want to be a bit smarter about this.
            //
            //
            // iterate through the key rings.
            //
            foreach (PgpPublicKeyRing kRing in pgpPub.GetKeyRings())
            {
                foreach (PgpPublicKey k in kRing.GetPublicKeys())
                {
                    if (k.IsEncryptionKey)
                        return k;
                }
            }

            throw new ArgumentException("Can't find encryption key in key ring.");
        }


        /**
        * Encrypt the data.
        *
        * @param inputData - byte array to encrypt
        * @param passPhrase - the password returned by "ReadPublicKey"
        * @param withIntegrityCheck - check the data for errors
        * @param armor - protect the data streams
        * @return - encrypted byte array
        */
        private static byte[] Encrypt(byte[] inputData, PgpPublicKey passPhrase, bool withIntegrityCheck, bool armor)
        {
            var processedData = Compress(inputData, PgpLiteralData.Console, CompressionAlgorithmTag.Uncompressed);

            var bOut = new MemoryStream();
            Stream output = bOut;

            if (armor)
                output = new ArmoredOutputStream(output);

            var encGen = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck,
                new SecureRandom());
            encGen.AddMethod(passPhrase);

            var encOut = encGen.Open(output, processedData.Length);

            encOut.Write(processedData, 0, processedData.Length);
            encOut.Close();

            if (armor)
                output.Close();

            return bOut.ToArray();
        }

        public static byte[] Encrypt(byte[] inputData, byte[] publicKey)
        {
            var publicKeyStream = new MemoryStream(publicKey);

            var encKey = ReadPublicKey(publicKeyStream);

            return Encrypt(inputData, encKey, true, true);
        }

        private static byte[] Compress(byte[] clearData, string fileName, CompressionAlgorithmTag algorithm)
        {
            var bOut = new MemoryStream();

            var comData = new PgpCompressedDataGenerator(algorithm);
            var cos = comData.Open(bOut); // open it with the final destination
            var lData = new PgpLiteralDataGenerator();

            // we want to Generate compressed data. This might be a user option later,
            // in which case we would pass in bOut.
            var pOut = lData.Open(
                cos, // the compressed output stream
                PgpLiteralData.Binary,
                fileName, // "filename" to store
                clearData.Length, // length of clear data
                DateTime.UtcNow // current time
            );

            pOut.Write(clearData, 0, clearData.Length);
            pOut.Close();

            comData.Close();

            return bOut.ToArray();
        }
    }
}
