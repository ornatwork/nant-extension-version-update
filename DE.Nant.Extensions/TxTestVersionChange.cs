//
using System;
using System.IO;
using System.Diagnostics;
//
using MbUnit.Framework;


namespace DE.Nant.Extensions
{
    [TestFixture]
    public class TxTestVersionChange 
    {
        [Test]
        public void setVersionNumberInTestFile()
        {
            string sTestFile = "..\\..\\AssemblyTestFile.txt";
            string sTestFileCopy = "tmpAssTestFile.txt";
            string sVersion = "2.2.220.2020";
            
            // copy to tmp file 
            if( File.Exists(sTestFileCopy) ) 
                File.Delete( sTestFileCopy );
            //
            File.Copy(sTestFile, sTestFileCopy);


            // Set new version in tmp file 
            CxSetVersionAssembly.ChangeAssemblyVersionNumber( sVersion, sTestFileCopy);
            
            // Verify that the version exists in the tmp file
            Assert.IsTrue(checkForVersion(sTestFileCopy, sVersion));
        }

		[Test]
		public void setVersionNumberInAtsTestFile()
		{
			string sTestFile = "..\\..\\ATS.AssemblyTestFile.txt";
			string sTestFileCopy = "tmpAtsAssTestFile.txt";
			string sVersion = "2.2.220.2020";

			// copy to tmp file 
			if (File.Exists(sTestFileCopy))
				File.Delete(sTestFileCopy);
			//
			File.Copy(sTestFile, sTestFileCopy);


			// Set new version in tmp file 
			CxSetVersionAssembly.ChangeAssemblyVersionNumber(sVersion, sTestFileCopy);

			// Verify that the version exists in the tmp file
			Assert.IsTrue(checkForVersion(sTestFileCopy, sVersion));
		}


        // Looks for the given string in the file 
        private static bool checkForVersion(string psFile, string psLookFor)
        {
            bool bSuccess = false;

            // Read the file line by line and see if the psLookFor string is found
            using (Stream stream = File.Open(psFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf(psLookFor) > -1)
                        {
                            bSuccess = true;
                            break;
                        }
                    }
                }

            }

            return bSuccess;
        }


    }  // EOC
}
