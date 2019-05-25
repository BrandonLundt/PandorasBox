using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PandorasBox
{
    [Cmdlet(VerbsLifecycle.Invoke,"GodHammer")]
    public class InvokeGodHammerCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ParameterSetName = "Path",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Path { get; set; }
        [Parameter(
            Mandatory = true,
            Position = 1,
            ParameterSetName = "Path",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Pattern { get; set; }
        private Queue directoriesToProcess;
        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            DirectoryInfo di = new DirectoryInfo( Path);
            
            Console.WriteLine("The directory {0} contains the following files:", di.Name);
            foreach (FileInfo f in fiArr)
            {
                Console.WriteLine("The size of {0} is {1} bytes.", f.Name, f.Length);
                f.Delete();
            }

            directoriesToProcess.Enqueue(new DirectoryInfo( Path));
            while( directoriesToProcess.Count > 0  )
            {
                try
                {
                    DirectoryInfo currentDirectory = (DirectoryInfo) directoriesToProcess.Dequeue();
                    directoriesToProcess.Enqueue( currentDirectory.GetDirectories());
                    FileInfo[] fiArr = currentDirectory.GetFiles();
                }
                catch{}
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }
}
