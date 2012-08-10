//
using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
//
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;


namespace DE.Nant.Extensions
{
    /// <summary>
    /// <setVersionAssembly
    ///     file=""
    ///     buildNumber=""
    /// />
    /// </summary>
    [TaskName("setVersionAssembly")]
    public class CxSetVersionAssembly : NAnt.Core.Task
    {
        //
        private static string msAssemblyFile = string.Empty;
        private static string msBuildNumber = string.Empty;
        private static string msVersion = string.Empty;


        [TaskAttribute("file", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string file
        {
            get { return msAssemblyFile; }
            set { msAssemblyFile = value; }
        }

        [TaskAttribute("buildNumber", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string buildNumber
        {
            get { return msBuildNumber; }
            set { msBuildNumber = value; }
        } 

        // Executes the nant task
        protected override void ExecuteTask()
        {
            // Don't catch errors, it will display in the build log if any
            Project.Log(Level.Info, "start setVersionAssembly");
            ChangeAssemblyVersionNumber(msBuildNumber, msAssemblyFile);
        }


        // Convienience function for version replace 
        public static void ChangeAssemblyVersionNumber(string sNewVersionNum, string sAssemblyFile)
        {
            // Will replace both for 
            // AssemblyVersion 
            // and 
            // AssemblyFileVersion
            ChangeFileNumber(sNewVersionNum, 
                             sAssemblyFile,
                             "Version\\(\"[0-9.*]*", 
                             "Version(\"");
        }


        // Updates the version number in the file 
        private static void ChangeFileNumber(string sNewVersionNum, string sFile,
                                            string sSearchExpr, string sNewVersionPrefix)
        {
            //
			// All files in VS IDE are saved as UTF8 by default
            Encoding encEncoder = ASCIIEncoding.UTF8;
            byte[] bytOldAssembly;
            byte[] bytNewAssembly;
            msVersion = sNewVersionPrefix + sNewVersionNum;

            //Retrieve byte array from file
            FileStream fileRead = new FileStream(sFile, FileMode.Open, FileAccess.Read);
            bytOldAssembly = new byte[fileRead.Length];
            fileRead.Read(bytOldAssembly, 0, Convert.ToInt32(fileRead.Length));
            fileRead.Close();

            //convert the byte array to a string
            string sAssembly = encEncoder.GetString(bytOldAssembly, 0,
                    bytOldAssembly.Length);

            //Replace the old version number in the assembly file
            sAssembly = Regex.Replace(sAssembly, sSearchExpr,
                    new MatchEvaluator(ReplaceMatch));

            //Convert the assembly string back to a byte array
            bytNewAssembly = encEncoder.GetBytes(sAssembly);

            //Write the assembly back to file
            FileStream fileWrite = new FileStream(sFile, FileMode.Open, FileAccess.Write);
            fileWrite.Write(bytNewAssembly, 0, bytNewAssembly.Length);
            fileWrite.Close();

        }

        //
        static string ReplaceMatch(Match m )
        {
            string sMatch = m.ToString();

            System.Console.WriteLine(string.Format("  found |{0}| replacing with |{1}|", sMatch, msVersion));

            return msVersion;
        }


    }  // EOC
}
