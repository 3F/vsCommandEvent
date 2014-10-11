﻿/* 
 * Boost Software License - Version 1.0 - August 17th, 2003
 * 
 * Copyright (c) 2013 Developed by reg <entry.reg@gmail.com>
 * 
 * Permission is hereby granted, free of charge, to any person or organization
 * obtaining a copy of the software and accompanying documentation covered by
 * this license (the "Software") to use, reproduce, display, distribute,
 * execute, and transmit the Software, and to prepare derivative works of the
 * Software, and to permit third-parties to whom the Software is furnished to
 * do so, all subject to the following:
 * 
 * The copyright notices in the Software and this entire statement, including
 * the above license grant, this restriction and the following disclaimer,
 * must be included in all copies of the Software, in whole or in part, and
 * all derivative works of the Software, unless such copies or derivative
 * works are solely in the form of machine-executable object code generated by
 * a source language processor.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
 * SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
 * FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.r_eg.vsSBE.Events
{
    public interface ISolutionEvent
    {
        /// <summary>
        /// Unique name for manually identification
        /// </summary>
        string name { get; set; }

        /// <summary>
        /// Mixed command to handling
        /// </summary>
        string command { get; set; }

        /// <summary>
        /// Short header about this
        /// </summary>
        string caption { get; set; }

        /// <summary>
        /// status of activate
        /// </summary>
        bool enabled { get; set; }

        /// <summary>
        /// Hide Process
        /// </summary>
        bool processHide { get; set; }

        /// <summary>
        /// not close after completion
        /// </summary>
        bool processKeep { get; set; }

        /// <summary>
        /// processing mode
        /// </summary>
        TModeCommands mode { get; set; }

        /// <summary>
        /// stream processor
        /// </summary>
        string interpreter { get; set; }

        /// <summary>
        /// treat newline as
        /// </summary>
        string newline { get; set; }

        /// <summary>
        /// symbol wrapper for commands or script
        /// </summary>
        string wrapper { get; set; }

        /// <summary>
        /// Wait until terminates script handling
        /// </summary>
        bool waitForExit { get; set; }

        /// <summary>
        /// support of MSBuild environment variables (properties)
        /// </summary>
        bool parseVariablesMSBuild { get; set; }

        /// <summary>
        /// Ignore all actions if the build failed
        /// </summary>
        bool buildFailedIgnore { get; set; }

        /// <summary>
        /// Run only for a specific configuration of solution
        /// strings format as:
        ///   'configname'|'platformname'
        ///   Compatible with: http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivscfg.get_displayname.aspx
        /// </summary>
        string[] toConfiguration { get; set; }

        /// <summary>
        /// Run for selected projects with execution order
        /// </summary>
        TExecutionOrder[] executionOrder { get; set; }

        /// <summary>
        /// Common Environment Visual Studio. Executes the specified commands
        /// TODO: custom list
        /// </summary>
        TOperation dteExec { get; set; }
    }

    /// <summary>
    /// Processing mode
    /// </summary>
    public enum TModeCommands
    {
        /// <summary>
        /// external commands
        /// </summary>
        File,
        /// <summary>
        /// command script
        /// </summary>
        Interpreter,
        /// <summary>
        /// DTE commands
        /// </summary>
        Operation,
    }

    /// <summary>
    /// Atomic DTE operation
    /// </summary>
    public class TOperation
    {
        /// <summary>
        /// exec-command
        /// </summary>
        public string[] cmd = null;
        /// <summary>
        /// optional ident
        /// </summary>
        public string caption = String.Empty;
        /// <summary>
        /// Abort operations on first error
        /// </summary>
        public bool abortOnFirstError = false;
    }

    public struct TExecutionOrder
    {
        public string project;
        public Type order;

        public enum Type
        {
            Before,
            After
        }
    }
}