using System.IO;
using SMA = System.Management.Automation;

namespace VisioPowerShell.Commands
{
    public class VisioCmdlet : SMA.Cmdlet
    {
        // this static _client variable is what allows
        // the various visiops cmdlets to share state (for example
        // to share which instance of Visio they are attached to)
        // 
        // To prevent confustion this should be the only static 
        // variable defined in VisioPS
        private static VisioAutomation.Scripting.Client _client;

        // Attached Visio Application represents the Visio instance
        //
        // that will be used for the cmdlet
        // NOTE that there are three cases - all are valid - to think about:
        // AttachedApplication = null
        // AttachedApplication != null && it is a usable instance
        // AttachedApplication != null && it is an unusable instance. For example
        //                     it might have been manually deleted

        public VisioAutomation.Scripting.Client client
        {
            get
            {
                // if a scripting client is not available create one and cache it
                // for the lifetime of this cmdlet

                VisioCmdlet._client = VisioCmdlet._client ?? new VisioAutomation.Scripting.Client(null);
                VisioCmdlet._client.Context = new Model.VisioPsContext(this);
                return VisioCmdlet._client;

                // Must always setup the client output
                // if we try to do this only once per new client then we'll
                // get this message:
                //
                //    "The WriteObject and WriteError methods cannot be
                //     called from outside the overrides of the BeginProcessing
                //     ProcessRecord, and EndProcessing methods, and only
                //     from that same thread."

            }
        }

        public void WriteVerbose(string fmt, params object[] items)
        {
            string s = string.Format(fmt, items);
            base.WriteVerbose(s);
        }
        
        protected bool CheckFileExists(string file)
        {
            if (!File.Exists(file))
            {
                this.WriteVerbose("Filename: {0}",file);
                this.WriteVerbose("Abs Filename: {0}", Path.GetFullPath(file));
                var exc = new FileNotFoundException(file);
                var er = new SMA.ErrorRecord(exc, "FILE_NOT_FOUND", SMA.ErrorCategory.ResourceUnavailable, null);
                this.WriteError(er);
                return false;
            }
            return true;
        }

        protected void DumpValues(CellValueDictionary cellvalues)
        {
            this.WriteVerbose($"CellValues contains {cellvalues.CellNames.Count} items");
            foreach (var cellname in cellvalues.CellNames)
            {
                string cell_value = cellvalues[cellname];
                this.WriteVerbose("{0} = {1}", cellname, cell_value);
            }
        }
    }
}