﻿/*
    * The MIT License (MIT)
    * 
    * Copyright (c) 2013 Developed by reg <entry.reg@gmail.com>
    * 
    * Permission is hereby granted, free of charge, to any person obtaining a copy
    * of this software and associated documentation files (the "Software"), to deal
    * in the Software without restriction, including without limitation the rights
    * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    * copies of the Software, and to permit persons to whom the Software is
    * furnished to do so, subject to the following conditions:
    * 
    * The above copyright notice and this permission notice shall be included in
    * all copies or substantial portions of the Software.
    * 
    * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace reg.ext.vsSolutionBuildEvent
{
    class Command
    {
        public static void basic(ISolutionEvent evt, bool isHidden = true)
        {
            basic(evt, ".", isHidden);
        }

        //TODO:
        public static void basic(ISolutionEvent evt, string context, bool isHidden = true)
        {
            if (!evt.enabled)
            {
                return;
            }

            ProcessStartInfo cmd = new ProcessStartInfo("cmd.exe");
            if (isHidden)
            {
                cmd.WindowStyle = ProcessWindowStyle.Hidden;
            }

            //TODO: capture message...
            cmd.Arguments = @"/C cd " + context + " & " + _letDisk(context) + ": & " + evt.command;
            Process.Start(cmd);
        }

        private static string _letDisk(string path)
        {
            return path.Substring(0, 1);
        }

        private Command() { }
    }
}
