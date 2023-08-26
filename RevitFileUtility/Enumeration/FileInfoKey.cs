﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.RevitFileUtility.Enumeration
{
    public enum FileInfoKey
    {
        Worksharing,
        Username,
        CentralModelPath,
        RevitBuild,
        LastSavePath,
        OpenWorksetDefault,
        RevitLTFile,
        CentralModelIdentity,
        LocaleWhenSaved,
        Invalid = -1
    }
}
