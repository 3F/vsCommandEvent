using System;
using Microsoft.VisualStudio;

namespace net.r_eg.vsCE
{
    public static class GuidList
    {
        /// <summary>
        /// General package identifier
        /// </summary>
        public const string PACKAGE_STRING      = "DA5CEB32-1E09-44A5-A6AA-71D3149A53B7";

        /// <summary>
        /// Identifier for used logger
        /// </summary>
        public const string PACKAGE_LOGGER      = "0440C4E8-5557-4E9C-9DB2-778851CA08F5";

        /// <summary>
        /// Command of main UI
        /// </summary>
        public const string MAIN_CMD_STRING     = "C587CC93-95F6-4E7D-B6D2-A5B5A854A9FC";

        /// <summary>
        /// Command of StatusPanel
        /// </summary>
        public const string PANEL_CMD_STRING    = "EC56D265-4229-46BC-9E88-07DDC1F07444";

        /// <summary>
        /// Identifier for StatusPanel
        /// </summary>
        public const string PANEL_STRING        = "56FED82A-2850-445B-AA29-49F19C6B9CC5";

        /// <summary>
        /// Main item for OutputWindow
        /// </summary>
        public const string OWP_SBE_STRING      = "2CCEC80E-9253-4573-8031-D17DBCEABC62";

        /// <summary>
        /// BuildOutput pane
        /// </summary>
        public const string OWP_BUILD_STRING    = "1BD8A850-02D1-11d1-BEE7-00A0C913D1F8";

        /// <summary>
        /// Guid of main UI command
        /// </summary>
        public static readonly Guid MAIN_CMD_SET = new Guid(MAIN_CMD_STRING);

        /// <summary>
        /// Guid of StatusPanel command
        /// </summary>
        public static readonly Guid PANEL_CMD_SET = new Guid(PANEL_CMD_STRING);

        /// <summary>
        /// Guid of main item for OutputWindow
        /// </summary>
        public static readonly Guid OWP_SBE = new Guid(OWP_SBE_STRING);



        /// <summary>
        /// Guid alias to StandardCommandSet97 
        /// </summary>
        public static readonly Guid VSStd97CmdID = VSConstants.CMDSETID.StandardCommandSet97_guid;

        /// <summary>
        /// Guid alias to StandardCommandSet2K
        /// </summary>
        public static readonly Guid VSStd2KCmdID = VSConstants.CMDSETID.StandardCommandSet2K_guid;
        
    }
}